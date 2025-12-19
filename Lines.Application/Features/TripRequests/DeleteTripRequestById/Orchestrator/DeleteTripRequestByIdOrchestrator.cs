using Lines.Application.Common;
using Lines.Application.Features.EndTripLocations.DeleteEndTripLocationsByUserId.Orchestrator;
using Lines.Application.Features.Offers.DeleteOffersByTripRequestId.Orchestrators;
using Lines.Application.Features.TripRequests.DeleteTripRequestById.Commands;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.TripRequests.DeleteTripRequestById.Orchestrator
{
    public record DeleteTripRequestByIdOrchestrator(Guid TripRequestId) : IRequest<Result<bool>>;

    public class DeleteTripRequestByIdOrchestratorHandler(
               RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<DeleteTripRequestByIdOrchestrator, Result<bool>>(parameters)
    {
        public override async Task<Result<bool>> Handle(DeleteTripRequestByIdOrchestrator request, CancellationToken cancellationToken)
        {
            // Call the orchestrators that deletes the related data >> here
            ///TODO: delete location after discuss with sadek how will we manage locations as primitive in trip request table or in location table

            // delete end trip location by trip request id  >> here
            await _mediator.Send(new DeleteEndTripLocationsByTripRequestIdOrchestrator(request.TripRequestId))
                           .ConfigureAwait(false);


            // delete offers
            await _mediator.Send(new DeleteOffersByTripRequestIdOrchestrator(request.TripRequestId))
                         .ConfigureAwait(false);

            // delete the trip request itself
            await _mediator.Send(new DeleteTripRequestByIdCommand(request.TripRequestId), cancellationToken);

            return Result<bool>.Success(true);
               
        }
    }
}
