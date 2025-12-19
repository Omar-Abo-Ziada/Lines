using Lines.Application.Features.Notifications.AddNotifications.Orchestrator;
using Lines.Application.Features.Offers.GetOfferById.Orchestrators;
using Lines.Application.Features.PaymentMethods.GetPaymentMethodById.Queries;
using Lines.Application.Features.TripRequests.DTOs;
using Lines.Application.Features.TripRequests.Queries;
using Lines.Application.Features.UserReward.GetUserRewardById.Orchestrators;
using Lines.Application.Helpers;

namespace Lines.Application.Features.Offers.AcceptOffer.Orchestrators;

/// <summary>
/// Orchestrator responsible for handling the acceptance of an offer by a driver.
/// This process involves validating the driver, retrieving the offer and trip request,
/// updating their statuses, creating a trip, notifying related users, and recording a notification.
/// </summary>
public record AcceptOfferOrchestrator(Guid OfferId, Guid UserId)
    : IRequest<Result<CreatedTripForDriverDto>>;

public class AcceptOfferOrchestratorHandler(
    RequestHandlerBaseParameters parameters,
    IHubContext<DriverHub> _hubContext,
     IFcmNotifier _notifier,
    IDriverConnectionService _driverConnectionService,
    IApplicationUserService applicationUserService)
    : RequestHandlerBase<AcceptOfferOrchestrator, Result<CreatedTripForDriverDto>>(parameters)
{
    public override async Task<Result<CreatedTripForDriverDto>> Handle(AcceptOfferOrchestrator request, CancellationToken cancellationToken)
    {
        // STEP 1: Validate that the user exists and is a passenger
        var passengerId = await applicationUserService.IsPaasengerAsync(request.UserId);
        if (passengerId is null || passengerId == Guid.Empty)
            return Result<CreatedTripForDriverDto>.Failure(
                new Error("PASSENGER.NOTEXISITED", $"No Passenger Found with this Id : {request.UserId}", ErrorType.NotFound));

        // STEP 2: Retrieve the offer by OfferId
        var offerResult = await _mediator.Send(new GetOfferByIdOrchestrator(request.OfferId), cancellationToken);
        if (offerResult.IsFailure || offerResult.Value == null)
            return Result<CreatedTripForDriverDto>.Failure(
                new Error("OFFER.NOTEXISITED", $"No Offer Found with this Id : {request.OfferId}", ErrorType.NotFound));

        // STEP 3: Ensure the offer is linked to a valid trip request
        if (offerResult.Value?.TripRequestId == null)
            return Result<CreatedTripForDriverDto>.Failure(
                new Error("OFFER.NOTTRIPREQUEST", $"No TripRequest Found for the offer with this Id : {request.OfferId}", ErrorType.NotFound));

        // accept offer
        offerResult.Value.MarkAsAccepted();

        // STEP 4: Retrieve the TripRequest details for acceptance
        var tripRequest = await _mediator.Send(new GetTripRequestForAcceptQuery(offerResult.Value.TripRequestId), cancellationToken)
                                         .ConfigureAwait(false);

        // STEP 5: Validate that the TripRequest is still valid and pending
        var validationResult = ValidateRequest(tripRequest);
        if (!validationResult.IsSuccess)
            return validationResult;

        // STEP 6: Update the TripRequest to mark it as accepted by the passenger
        var updateTripRequest = await _mediator
            .Send(new AcceptTripRequestCommand(offerResult.Value.TripRequestId, offerResult.Value.DriverId), cancellationToken)
            .ConfigureAwait(false);

        // STEP 7: Retrieve the payment method associated with this trip
        var paymentMethod = await _mediator
            .Send(new GetPaymentMethodByIdQuery(tripRequest.PaymentMethodId), cancellationToken)
            .ConfigureAwait(false);


        decimal? tripRequestFareAfterRewardApplied = null;
        decimal? offerFareAfterRewardApplied = null;

        if (tripRequest.UserRewardId != null)
        {
            var userReward = await _mediator.Send(new GetUserRewardByIdOrchestrator((Guid)tripRequest.UserRewardId));
            if (userReward.IsSuccess && userReward.Value != null)
            {
                userReward.Value.MarkAsUsed(tripRequest.Id);

                tripRequestFareAfterRewardApplied = FareAfterRewardAppliedCalculator.Calculate(tripRequest.EstimatedPrice, userReward.Value.Reward.DiscountPercentage, userReward.Value.Reward.MaxValue);
                offerFareAfterRewardApplied = FareAfterRewardAppliedCalculator.Calculate(offerResult.Value.Price, userReward.Value.Reward.DiscountPercentage, userReward.Value.Reward.MaxValue);
            }
        }

        bool isRewardApplied = tripRequestFareAfterRewardApplied != null;


        // STEP 8: Create a new Trip record in the system
        var createdTrip = await _mediator.Send(new CreateTripCommand(
            offerResult.Value.DriverId,
            tripRequest.PassengerId,
            tripRequest.StartLocation,
            tripRequest.Distance,
            offerResult.Value.Price,   // take price from offer in this case
            tripRequest.PaymentMethodId,
            tripRequest.Id,
            paymentMethod.Type,
            offerFareAfterRewardApplied, isRewardApplied), cancellationToken).ConfigureAwait(false);      

        // STEP 9: Update the TripRequest with the new trip's end location
        var updateEndLocations = await _mediator
            .Send(new UpdateEndTripLocationWithTripCommand(offerResult.Value.TripRequestId, createdTrip.Id), cancellationToken)
            .ConfigureAwait(false);

        // STEP 10: Notify the passenger and nearby drivers via SignalR
        await NotifyDriverAndPassenger(tripRequest, createdTrip, tripRequestFareAfterRewardApplied , cancellationToken);
        
        #region FCM Firebase

        // STEP 10.1: Notify driver (FCM + DB)
        await _notifier.SendToUserAsync(
            offerResult.Value.DriverId,
            NotificationTemplates.OfferAcceptedTitle,
            NotificationTemplates.OfferAcceptedBodyDriver,
            new
            {
                TripId = createdTrip.Id,
                TripRequestId = tripRequest.Id,
                PassengerId = tripRequest.PassengerId,
                OfferFareAfterRewardApplied = offerFareAfterRewardApplied  
            });

        await _mediator.Send(new AddNotificationsOrchestrator(
            offerResult.Value.DriverId,
            NotificationTemplates.OfferAcceptedTitle,
            NotificationTemplates.OfferAcceptedBodyDriver,
            NotificationType.DriverOfferAccepted),
            cancellationToken);

        // STEP 10.2: Notify passenger (FCM only)
        await _notifier.SendToUserAsync(
            tripRequest.PassengerId,
            NotificationTemplates.OfferAcceptedTitle,
            NotificationTemplates.TripConfirmedBodyPassenger,
            new
            {
                TripId = createdTrip.Id,
                DriverId = offerResult.Value.DriverId,
                OfferFareAfterRewardApplied = offerFareAfterRewardApplied
            });

        #endregion

        // STEP 11: Prepare and return the final trip data for the driver
        var tripForDriver = new CreatedTripForDriverDto
        {
            PassengerId = tripRequest.PassengerId,
            Id = createdTrip.Id,
            PickUpLocation = tripRequest.StartLocation
        };

        // STEP 12: Add a system notification for the passenger
        await _mediator.Send(new AddNotificationsOrchestrator(
            tripRequest.PassengerId,
            "TripConfirmed",
            "Your trip has been confirmed by the driver.",
            NotificationType.TripRequestAccepted),
            cancellationToken);

        // STEP 13: Return the final success result
        return Result<CreatedTripForDriverDto>.Success(tripForDriver);
    }

    /// <summary>
    /// Sends SignalR notifications to both the passenger and other drivers when a trip is accepted.
    /// </summary>
    private async Task NotifyDriverAndPassenger(GetTripRequestForAcceptDto tripRequest, CreatedTripDto createdTrip, decimal? tripRequestFareAfterRewardApplied, CancellationToken cancellationToken)
    {
        // Calculate km price for filtering nearby drivers >> use price from trip request not the offer, as the other drivers were notified by this request for the first time based on trip request price not offer price
        decimal kmPrice = tripRequest.EstimatedPrice / (decimal)tripRequest.Distance;

        // STEP A: Get nearby drivers based on km price and vehicle type
        var nearbyDriverConnections = await _mediator.Send(new GetNearbyDriverConnectionsFilteredByKmPriceAndVehicleTypeOrchestrator(
            tripRequest.StartLocation.Latitude,
            tripRequest.StartLocation.Longitude,
            kmPrice,
            tripRequest.VehicleTypeId,
            tripRequest.PassengerId,
            isAnonymous: false,
            radiusKm: 10.0));

        // STEP B: Get driver connection IDs for the accepted driver
        var acceptedDriverConnections = await _driverConnectionService.GetDriverConnectionsAsync(createdTrip.DriverId);

        // STEP C: Exclude the accepted driver's connections from the notification list
        var driversToNotify = nearbyDriverConnections
            .Where(c => !acceptedDriverConnections.Contains(c))
            .ToList();

        // STEP D: Notify nearby drivers that the trip request is closed
        if (driversToNotify.Any())
        {
            await _hubContext.Clients.Clients(driversToNotify)
                .SendAsync("TripRequestClosed", tripRequest.Id, tripRequestFareAfterRewardApplied, cancellationToken);
        }

        // STEP E: Notify the driver that the offer has been accepted   
        await _hubContext.Clients.Group($"Driver_{createdTrip.DriverId}")
            .SendAsync("OfferAccepted", createdTrip, cancellationToken); 
    }

    /// <summary>
    /// Validates that the TripRequest exists and is in a Pending state.
    /// </summary>
    private Result<CreatedTripForDriverDto> ValidateRequest(GetTripRequestForAcceptDto tripRequest)
    {
        if (tripRequest == null)
            return Result<CreatedTripForDriverDto>.Failure(TripRequestErrors.TripRequestNotFound);

        if (tripRequest.Status != TripRequestStatus.Pending)
            return Result<CreatedTripForDriverDto>.Failure(TripRequestErrors.TripRequestAcceptedOrCanceled);

        return Result<CreatedTripForDriverDto>.Success(null);
    }
}
