using Lines.Application.Features.Notifications.AddNotifications.Orchestrator;

namespace Lines.Application.Features.Offers.CreateOffer.Notifications;

public record NotifyNewOfferCreatedCommand(
    Guid PassengerId,
    Guid TripRequestId,
    Guid OfferId,
    Guid DriverId,
    decimal Price,
    float DistanceToArriveInMeters,
    int TimeToArriveInMinutes,
    decimal? FareAfterRewardApplied
) : IRequest<Unit>;

public class NotifyNewOfferCreatedCommandHandler : RequestHandlerBase<NotifyNewOfferCreatedCommand, Unit>
{
    private readonly IHubContext<DriverHub> _hubContext;
    private readonly IFcmNotifier _notifier;

    public NotifyNewOfferCreatedCommandHandler(
        RequestHandlerBaseParameters parameters,
        IHubContext<DriverHub> hubContext
,
        IFcmNotifier notifier) : base(parameters)
    {
        _hubContext = hubContext;
        _notifier = notifier;
    }

    public override async Task<Unit> Handle(NotifyNewOfferCreatedCommand request, CancellationToken cancellationToken)
    {
        // ================================================================
        // 1) Prepare payload for SignalR
        // ================================================================
        var payload = new
        {
            offerId = request.OfferId,
            driverId = request.DriverId,
            tripRequestId = request.TripRequestId,
            price = request.Price,
            distanceToArriveInMeters = request.DistanceToArriveInMeters,
            timeToArriveInMinutes = request.TimeToArriveInMinutes,
            fareAfterRewardApplied = request.FareAfterRewardApplied
        };
        // ================================================================
        // 2) Send SignalR (if rider is online)
        // ================================================================
        await _hubContext
            .Clients
            .Group($"Rider_{request.PassengerId}")
            .SendAsync("NewOffer", payload, cancellationToken);


        #region FCM Firebase 
        // ================================================================
        // 3) Send FCM Notification
        // ================================================================
        await _notifier.SendToUserAsync(
            request.PassengerId,
            NotificationTemplates.NewOfferTitle,
            NotificationTemplates.NewOfferBody,
            new
            {
                OfferId = request.OfferId,
                TripRequestId = request.TripRequestId,
                DriverId = request.DriverId,
                Price = request.Price,
                FareAfterRewardApplied = request.FareAfterRewardApplied
            });

        // ================================================================
        // 4) Save Notification in DB
        // ================================================================
        await _mediator.Send(new AddNotificationsOrchestrator(
            request.PassengerId,
            NotificationTemplates.NewOfferTitle,
            NotificationTemplates.NewOfferBody,
            NotificationType.DriverOfferCreated));

        #endregion
        return Unit.Value;
    }
}


