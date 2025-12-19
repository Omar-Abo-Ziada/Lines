using Lines.Application.Common;
using Lines.Application.Features.Messages.DeleteMessagesById.Commands;
using Lines.Domain.Enums;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Messages.DeleteMessagesById.Orchestrator
{
    public record DeleteMessagesByIdAndTypeOrchestrator(Guid MessageId, MessageType MessageType) : IRequest<Result<bool>>;


    public class DeleteMessagesByIdAndTypeOrchestratorHandler(
           RequestHandlerBaseParameters parameters)
           : RequestHandlerBase<DeleteMessagesByIdAndTypeOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteMessagesByIdAndTypeOrchestrator request, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteMessagesByIdAndTypeCommand(request.MessageId, request.MessageType), cancellationToken)
                           .ConfigureAwait(false);

            return Result<bool>.Success(true);
        }
    }
}
