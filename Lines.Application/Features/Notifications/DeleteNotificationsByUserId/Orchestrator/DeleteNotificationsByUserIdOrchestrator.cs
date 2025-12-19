using Lines.Application.Common;
using Lines.Application.Features.Notifications.DeleteNotificationsByUserId.Commands;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Notifications.DeleteNotificationsByUserId.Orchestrator
{
    public record DeleteNotificationsByUserIdOrchestrator(Guid UserId) : IRequest<Result<bool>>;

    public class DeleteNotificationsByUserIdOrchestratorHandler(
           RequestHandlerBaseParameters parameters)
           : RequestHandlerBase<DeleteNotificationsByUserIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteNotificationsByUserIdOrchestrator request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteNotificationsByUserIdCommand(request.UserId), cancellationToken).ConfigureAwait(false);
            return Result<bool>.Success(true);
        }
    }
} 