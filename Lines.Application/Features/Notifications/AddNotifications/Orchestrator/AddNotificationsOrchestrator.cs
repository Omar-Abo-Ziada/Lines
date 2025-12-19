using Lines.Application.Features.Notifications.AddNotifications.Commands;

namespace Lines.Application.Features.Notifications.AddNotifications.Orchestrator
{
    public record AddNotificationsOrchestrator(
        Guid UserId,
        string Title,
        string Message,
        NotificationType NotificationType
        ) : IRequest<Result<Guid>>;

    public class AddNotificationsOrchestratorHandler : RequestHandlerBase<AddNotificationsOrchestrator, Result<Guid>>
    {
        public AddNotificationsOrchestratorHandler(RequestHandlerBaseParameters parameters) : base(parameters)
        {
        }
        public override async Task<Result<Guid>> Handle(AddNotificationsOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddNotificationsCommand(request.UserId,request.Title,request.Message,request.NotificationType), cancellationToken).ConfigureAwait(false);
            return Result<Guid>.Success(result);
        }
    }

}
