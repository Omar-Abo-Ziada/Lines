using Lines.Application.Common;
using Lines.Application.Features.Messages.DeleteMessagesById.Orchestrator;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Messages.DeleteMessagesByTripId.Orchestrator
{
    public record DeleteMessagesByTripIdOrchestrator(Guid TripId) : IRequest<Result<bool>>;


    public class DeleteMessagesByTripIdOrchestratorHandler(
        RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<DeleteMessagesByTripIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteMessagesByTripIdOrchestrator request, CancellationToken cancellationToken)
        {
            var messages = await _mediator.Send(new GetMessageIdsAndTypesByTripIdOrchestrator(request.TripId), cancellationToken)
                                            .ConfigureAwait(false);

            foreach (var message in messages.Value)
            {
             await _mediator.Send(new DeleteMessagesByIdAndTypeOrchestrator(message.Id , message.Type),cancellationToken);
            }

            return Result<bool>.Success(true);
        }
    }
}
