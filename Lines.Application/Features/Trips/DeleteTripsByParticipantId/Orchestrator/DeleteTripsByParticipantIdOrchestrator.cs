using Lines.Application.Common;
using Lines.Application.Features.Trips.DeleteTripById.Orchestrator;
using Lines.Application.Features.Trips.DeleteTripsByParticipantId.Orchestrator;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Trips.DeleteTripsByUserId.Orchestrator
{
    public record DeleteTripsByParticipantIdOrchestrator(Guid ParticipantId) : IRequest<Result<bool>>;

    public class DeleteTripsByParticipantIdOrchestratorHandler(
           RequestHandlerBaseParameters parameters)
           : RequestHandlerBase<DeleteTripsByParticipantIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteTripsByParticipantIdOrchestrator request, CancellationToken cancellationToken)
        {
            var tripIds = await _mediator.Send(new GetTripIdsByParticipantIdOrchestrator(request.ParticipantId), cancellationToken).ConfigureAwait(false);

            foreach(var tripId in tripIds.Value)
            {
                await _mediator.Send(new DeleteTripByIdOrchestrator(tripId), cancellationToken)
                               .ConfigureAwait(false);
            }
                return Result<bool>.Success(true);
            
        }
    }
} 