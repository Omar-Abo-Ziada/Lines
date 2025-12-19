using Lines.Application.Features.FCMNotifications.Queries;
using Lines.Application.Features.Notifications.AddNotifications.Orchestrator;
using Lines.Application.Features.TripRequests.Command;
using Lines.Application.Features.TripRequests.DTOs;
using Lines.Application.Features.TripRequests.Queries;
using Lines.Application.Features.UserReward.GetUserRewardById.Orchestrators;
using Lines.Application.Helpers;

namespace Lines.Application.Features.TripRequests.Orchestrator;

public record CancelTripRequestOrchestrator(Guid TripRequestId, string CancellationReason, Guid passengerId) :  IRequest<Result<bool>>;
public class CancelTripRequestOrchestratorHandler : RequestHandlerBase<CancelTripRequestOrchestrator, Result<bool>>
{
    private readonly IDriverConnectionService _driverConnectionService;
    private readonly IHubContext<DriverHub> _hubContext;
    private readonly IFcmNotifier _notifier;

    public CancelTripRequestOrchestratorHandler(RequestHandlerBaseParameters parameters, IDriverConnectionService driverConnectionService, IHubContext<DriverHub> hubContext, IFcmNotifier notifier) : base(parameters)
    {
        _driverConnectionService = driverConnectionService;
        _hubContext = hubContext;
        _notifier = notifier;
    }
  
    public override async Task<Result<bool>> Handle(CancelTripRequestOrchestrator request, CancellationToken cancellationToken)
    {
        var tripRequest = await _mediator
            .Send(new GetTripRequestForCancelingQuery(request.TripRequestId), cancellationToken).ConfigureAwait(false);
        
        var validationResult = await ValidateRequest(tripRequest);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        if(tripRequest.PassengerId != request.passengerId)
        {
            return Result<bool>.Failure(TripRequestErrors.NotYourTripRequest);
        }

        await _mediator.Send(new CancelTripRequestCommand(request.TripRequestId, request.CancellationReason), cancellationToken).ConfigureAwait(false);

        decimal? fareAfterRewardApplied = null;

        if (tripRequest.UserRewardId is not null)
        {
            var userReward = await _mediator.Send(new GetUserRewardByIdOrchestrator((Guid)tripRequest.UserRewardId));
            if(userReward.IsSuccess)
            {
                userReward.Value?.MarkAsUnUsed();   
                // you can comment the above line as the user reward is being marked as used only after request acceptance and in cancellation case >> 100 % the request has not been accepted by any driver, but keep the line uncommented as this business may change later and u may forget to add this line again + there is no extra cost from keeping it as u get the user reward record from db to calc fare after reward applied anyway
            }

            fareAfterRewardApplied = FareAfterRewardAppliedCalculator.Calculate(tripRequest.EstimatedPrice, 
                                     userReward.Value.Reward.DiscountPercentage, userReward.Value.Reward.MaxValue);
        }

        

        await NotifyDriverAndPassenger(tripRequest, fareAfterRewardApplied, cancellationToken);
        await _mediator.Send(new AddNotificationsOrchestrator(tripRequest.PassengerId, "TripCancelld", "Your have been cancelled your trip request.", NotificationType.TripRequestCancelled), cancellationToken);

        return Result<bool>.Success(true);
    }
    
    private async Task NotifyDriverAndPassenger(TripRequestForCancelingDto tripRequest, decimal? fareAfterRewardApplied, CancellationToken cancellationToken)
    {
        decimal kmPrice = tripRequest.EstimatedPrice / (decimal)tripRequest.Distance;

        // STEP 1 — Get all drivers that had the trip request
        var nearbyDriverConnections = await _mediator.Send(new GetNearbyDriverConnectionsFilteredByKmPriceAndVehicleTypeOrchestrator(
            tripRequest.StartLocation.Latitude, 
            tripRequest.StartLocation.Longitude,
            kmPrice,
            tripRequest.VehicleTypeId,
            tripRequest.PassengerId,
            isAnonymous: false,
            radiusKm: 10.0));

        // STEP 2 — SignalR: close the request for ONLINE drivers
        if (nearbyDriverConnections.Any())
        {
            await _hubContext.Clients.Clients(nearbyDriverConnections)
                .SendAsync("TripRequestClosed", tripRequest.Id, fareAfterRewardApplied, cancellationToken);
        }

        #region FCM Firebase
        // STEP 3 — FCM: notify OFFLINE drivers
        var driverIds = _driverConnectionService.GetDriverIdsFromConnections(nearbyDriverConnections);

        if (driverIds.Any())
        {
            var tokens = await _mediator.Send(new GetFcmTokensByUserIdsQuery(driverIds.ToList()));

            if (tokens.Any())
            {
                await _notifier.SendToManyAsync(
                    tokens,
                    NotificationTemplates.TripCancelledTitle,
                    NotificationTemplates.TripCancelledBodyDriver,
                    new { TripRequestId = tripRequest.Id, FareAfterRewardApplied = fareAfterRewardApplied });
            }

            // save notification in DB
            foreach (var driverId in driverIds)
            {
                await _mediator.Send(new AddNotificationsOrchestrator(
                    driverId,
                    NotificationTemplates.TripCancelledTitle,
                    NotificationTemplates.TripCancelledBodyDriver,
                    NotificationType.TripRequestCancelled),
                    cancellationToken);
            }
        }

        // STEP 4 — Notify Passenger (FCM)
        await _notifier.SendToUserAsync(
            tripRequest.PassengerId,
            NotificationTemplates.TripCancelledTitle,
            NotificationTemplates.TripCancelledBodyPassenger,
            new { TripRequestId = tripRequest.Id, FareAfterRewardApplied = fareAfterRewardApplied });
        #endregion
    }

    private async Task<Result<bool>> ValidateRequest(TripRequestForCancelingDto tripRequest)
    {
        if (tripRequest == null)
        {
            return Result<bool>.Failure(TripRequestErrors.TripRequestNotFound);
        }

        if (tripRequest.Status != TripRequestStatus.Pending)
        {
            return Result<bool>.Failure(TripRequestErrors.TripRequestAcceptedOrCanceled);
        }
        return  Result<bool>.Success(true);
    }
}