using Lines.Application.Common;
using Lines.Application.Features.Trips.DeleteTripsByParticipantId.Queries;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Trips.DeleteTripsByParticipantId.Orchestrator
{

    public record GetTripIdsByParticipantIdOrchestrator(Guid ParticipantId) : IRequest<Result<List<Guid>>>;

    public class GetTripIdsByParticipantIdOrchestratorHandler(
                       RequestHandlerBaseParameters parameters)
            : RequestHandlerBase<GetTripIdsByParticipantIdOrchestrator, Result<List<Guid>>>(parameters)
    {
        public override async Task<Result<List<Guid>>> Handle(GetTripIdsByParticipantIdOrchestrator request, CancellationToken cancellationToken)
        {
            var tripIds = await _mediator.Send(new GetTripIdsByParticipantIdQuery(request.ParticipantId), cancellationToken).ConfigureAwait(false);
            return Result<List<Guid>>.Success(tripIds);
        }
    }
}
