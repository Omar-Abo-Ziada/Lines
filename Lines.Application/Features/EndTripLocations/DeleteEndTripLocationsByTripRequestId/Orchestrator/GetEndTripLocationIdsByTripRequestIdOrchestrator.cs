using Lines.Application.Common;
using Lines.Application.Features.EndTripLocations.DeleteEndTripLocationsByTripId.Queries;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.EndTripLocations.DeleteEndTripLocationsByTripId.Orchestrator
{
    public record GetEndTripLocationIdsByTripRequestIdOrchestrator(Guid TripRequestId) : IRequest<Result<List<Guid>>>;


    public class GetEndTripLocationIdsByTripRequestIdOrchestratorHandler(
                   RequestHandlerBaseParameters parameters)
            : RequestHandlerBase<GetEndTripLocationIdsByTripRequestIdOrchestrator, Result<List<Guid>>>(parameters)
    {
        public override async Task<Result<List<Guid>>> Handle(GetEndTripLocationIdsByTripRequestIdOrchestrator request, CancellationToken cancellationToken)
        {
            var endTripLocationIds = await _mediator.Send(new GetEndTripLocationIdsByTripRequestIdQuery(request.TripRequestId), cancellationToken)
                                                    .ConfigureAwait(false);

            return Result<List<Guid>>.Success(endTripLocationIds);
        }
    }
}
