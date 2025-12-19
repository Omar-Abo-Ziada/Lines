using Lines.Application.Common;
using Lines.Application.Features.Messages.DeleteMessagesByUserId.Commands;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Messages.DeleteMessagesByUserId.Orchestrator
{
    public record DeleteMessagesByUserIdOrchestrator(Guid UserId) : IRequest<Result<bool>>;

    public class DeleteMessagesByUserIdOrchestratorHandler(
           RequestHandlerBaseParameters parameters)
           : RequestHandlerBase<DeleteMessagesByUserIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteMessagesByUserIdOrchestrator request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteMessagesByUserIdCommand(request.UserId), cancellationToken)
                           .ConfigureAwait(false);

            return Result<bool>.Success(true);
        }
    }
} 