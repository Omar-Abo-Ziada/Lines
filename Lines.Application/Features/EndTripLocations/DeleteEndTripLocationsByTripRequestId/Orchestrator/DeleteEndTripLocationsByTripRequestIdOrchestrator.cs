using Lines.Application.Common;
using Lines.Application.Features.EndTripLocations.DeleteEndTripLocationsByTripId.Orchestrator;
using Lines.Application.Features.EndTripLocations.DeleteEndTripLocationsByUserId.Commands;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.EndTripLocations.DeleteEndTripLocationsByUserId.Orchestrator
{
    public record DeleteEndTripLocationsByTripRequestIdOrchestrator(Guid TripRequestId) : IRequest<Result<bool>>;

    public class DeleteEndTripLocationsByTripRequestIdOrchestratorHandler(
           RequestHandlerBaseParameters parameters)
           : RequestHandlerBase<DeleteEndTripLocationsByTripRequestIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteEndTripLocationsByTripRequestIdOrchestrator request, CancellationToken cancellationToken)
        {
          var endTripLocationIds =  await _mediator.Send(new GetEndTripLocationIdsByTripRequestIdOrchestrator(request.TripRequestId), cancellationToken).ConfigureAwait(false);


            foreach(var id in endTripLocationIds.Value)
            {
                await _mediator.Send(new DeleteEndTripLocationsByIdCommand(id), cancellationToken)  
                                .ConfigureAwait(false);
            }
            return Result<bool>.Success(true);
        }
    }
} 