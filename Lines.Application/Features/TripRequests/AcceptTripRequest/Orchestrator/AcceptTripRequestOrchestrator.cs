using Lines.Application.Features.Notifications.AddNotifications.Orchestrator;
using Lines.Application.Features.PaymentMethods.GetPaymentMethodById.Queries;
using Lines.Application.Features.TripRequests.DTOs;
using Lines.Application.Features.TripRequests.GetTripRequestById.Orchestrators;
using Lines.Application.Features.UserReward.GetUserRewardById.Orchestrators;
using Lines.Application.Helpers;

namespace Lines.Application.Features.TripRequests;

public record AcceptTripRequestOrchestrator(Guid TripRequestId, Guid DriverId) : IRequest<Result<CreatedTripForDriverDto>>;
public class AcceptTripRequestOrchestratorHandler : RequestHandlerBase<AcceptTripRequestOrchestrator, Result<CreatedTripForDriverDto>>
{
    private readonly IHubContext<DriverHub> _hubContext;
    private readonly IDriverConnectionService _driverConnectionService;
    private readonly IFcmNotifier _notifier;
    private readonly IApplicationUserService _appUserService;

    public AcceptTripRequestOrchestratorHandler(RequestHandlerBaseParameters parameters, IHubContext<DriverHub> hubContext, IDriverConnectionService driverConnectionService, IFcmNotifier notifier, IApplicationUserService appUserService) : base(parameters)
    {
        _hubContext = hubContext;
        _driverConnectionService = driverConnectionService;
        _notifier = notifier;
        _appUserService = appUserService;
    }

    public override async Task<Result<CreatedTripForDriverDto>> Handle(AcceptTripRequestOrchestrator request, CancellationToken cancellationToken)
    {   
        var tripRequest = await _mediator.Send(new GetTripRequestByIdOrchestrator(request.TripRequestId), cancellationToken).ConfigureAwait(false);

        var validationResult = ValidateRequest(tripRequest.Value);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var updateTripRequest = await _mediator
            .Send(new AcceptTripRequestCommand(request.TripRequestId, request.DriverId), cancellationToken)
            .ConfigureAwait(false);

   

        var paymentMethod = await _mediator.Send(new GetPaymentMethodByIdQuery(tripRequest.Value.PaymentMethodId), cancellationToken).ConfigureAwait(false);

        decimal? fareAfterRewardApplied = null;
        if (tripRequest.Value.UserRewardId != null)
        {
            var userReward = await _mediator.Send(new GetUserRewardByIdOrchestrator((Guid)tripRequest.Value.UserRewardId));
            if (userReward.IsSuccess && userReward.Value != null)
            {
                userReward.Value.MarkAsUsed(tripRequest.Value.Id);

                fareAfterRewardApplied = FareAfterRewardAppliedCalculator.Calculate(tripRequest.Value.EstimatedPrice, userReward.Value.Reward.DiscountPercentage, userReward.Value.Reward.MaxValue);
            }
        }

        bool isRewardApplied = fareAfterRewardApplied != null;

        var createdTrip = await _mediator
            .Send(new CreateTripCommand(
                request.DriverId, tripRequest.Value.PassengerId, tripRequest.Value.StartLocation,
                tripRequest.Value.Distance, tripRequest.Value.EstimatedPrice, tripRequest.Value.PaymentMethodId,
                tripRequest.Value.Id, paymentMethod.Type, fareAfterRewardApplied, isRewardApplied), cancellationToken).ConfigureAwait(false);

        tripRequest.Value.AssignTrip(createdTrip.Id);

        var updateEndLocations = await _mediator.Send(new UpdateEndTripLocationWithTripCommand(request.TripRequestId, createdTrip.Id), cancellationToken).ConfigureAwait(false);

        await NotifyDriverAndPassenger(tripRequest.Value, createdTrip, fareAfterRewardApplied , cancellationToken);

        var passengerUserId = await _appUserService
          .GetUserIdByPassengerIdAsync(tripRequest.Value.PassengerId);



        if (passengerUserId == null)
        {
            // log + كمل عادي
            return Result<CreatedTripForDriverDto>.Success(new CreatedTripForDriverDto
            {
                PassengerId = tripRequest.Value.PassengerId,
                Id = createdTrip.Id,
                PickUpLocation = tripRequest.Value.StartLocation
            });
        }

        var tripForDriver = new CreatedTripForDriverDto()
        { PassengerId = tripRequest.Value.PassengerId, Id = createdTrip.Id, PickUpLocation = tripRequest.Value.StartLocation };
        await _mediator.Send(new AddNotificationsOrchestrator(passengerUserId.Value, "TripConfirmed", "Your trip has been confirmed by the driver.", NotificationType.TripRequestAccepted), cancellationToken);
        //await _mediator.Send(new AddNotificationsOrchestrator(tripRequest.Value.PassengerId, "TripConfirmed", "Your trip has been confirmed by the driver.", NotificationType.TripRequestAccepted), cancellationToken);
        return Result<CreatedTripForDriverDto>.Success(tripForDriver);
    }

    private async Task NotifyDriverAndPassenger(TripRequest tripRequest, CreatedTripDto createdTrip, decimal? fareAfterRewardApplied, CancellationToken cancellationToken)
    {
        decimal kmPrice = tripRequest.EstimatedPrice / (decimal)tripRequest.Distance;

        var nearbyDriverConnections = await _mediator.Send(new GetNearbyDriverConnectionsFilteredByKmPriceAndVehicleTypeOrchestrator(
            tripRequest.StartLocation.Latitude,
            tripRequest.StartLocation.Longitude,
            kmPrice,
            tripRequest.VehicleTypeId,
            tripRequest.PassengerId,
            isAnonymous: false,
            radiusKm: 10.0));

        var acceptedDriverConnections = await _driverConnectionService.GetDriverConnectionsAsync(createdTrip.DriverId);
        var driversToNotify = nearbyDriverConnections
                                .Where(c => !acceptedDriverConnections.Contains(c)) // exclude accepted driver�s connections
                                .ToList();
        // ============================================================
        // 2) SignalR: close request for other drivers
        // ============================================================
        if (driversToNotify.Any())
        {
            await _hubContext.Clients.Clients(driversToNotify)
                .SendAsync("TripRequestClosed", tripRequest.Id, fareAfterRewardApplied , cancellationToken);
        }

        // ============================================================
        // 3) SignalR: notify passenger that driver accepted the request
        // ============================================================
        #region Resolve UserIds

        var passengerUserId = await _appUserService
            .GetUserIdByPassengerIdAsync(tripRequest.PassengerId);

        var driverUserId = await _appUserService
            .GetUserIdByDriverIdAsync(createdTrip.DriverId);

        if (passengerUserId == null || driverUserId == null)
        {
            // هنا log بس، ومكمّل السيستم عادي
            return;
        }

        #endregion


        await _hubContext.Clients.Group($"Rider_{tripRequest.PassengerId}")
            .SendAsync("TripRequestAccepted", createdTrip, fareAfterRewardApplied , cancellationToken);

        #region FCM Firebase
        // ============================================================
        // 4) FCM: notify driver
        // ============================================================
        await _notifier.SendToUserAsync(
            driverUserId.Value,
            //createdTrip.DriverId,
            NotificationTemplates.TripAcceptedTitle,
            NotificationTemplates.TripAcceptedBodyForDriver,
            new
            {
                TripId = createdTrip.Id,
                TripRequestId = tripRequest.Id,
                PassengerId = tripRequest.PassengerId,
                FareAfterRewardApplied = fareAfterRewardApplied
            });

        await _mediator.Send(new AddNotificationsOrchestrator(
            driverUserId.Value,
            //createdTrip.DriverId,
            NotificationTemplates.TripAcceptedTitle,
            NotificationTemplates.TripAcceptedBodyForDriver,
            NotificationType.TripRequestAccepted), cancellationToken);

        // ============================================================
        // 5) FCM: notify Passenger
        // ============================================================
        await _notifier.SendToUserAsync(
            passengerUserId.Value,
            //tripRequest.PassengerId,
            NotificationTemplates.TripAcceptedTitle,
            NotificationTemplates.TripAcceptedBodyForPassenger,
            new
            {
                TripId = createdTrip.Id,
                DriverId = createdTrip.DriverId,
                FareAfterRewardApplied = fareAfterRewardApplied
            });

        await _mediator.Send(new AddNotificationsOrchestrator(
            passengerUserId.Value,
            //tripRequest.PassengerId,
            NotificationTemplates.TripAcceptedTitle,
            NotificationTemplates.TripAcceptedBodyForPassenger,
            NotificationType.TripRequestAccepted),
            cancellationToken);
        #endregion

    }

    private Result<CreatedTripForDriverDto> ValidateRequest(TripRequest tripRequest)
    {
        if (tripRequest == null)
        {
            return Result<CreatedTripForDriverDto>.Failure(TripRequestErrors.TripRequestNotFound);
        }

        if (tripRequest.Status != TripRequestStatus.Pending)
        {
            return Result<CreatedTripForDriverDto>.Failure(TripRequestErrors.TripRequestAcceptedOrCanceled);
        }
        return Result<CreatedTripForDriverDto>.Success(null);
    }
}