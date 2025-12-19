using Lines.Application.Common;
using Lines.Application.Features.Offers.DeleteOffersByTripRequestId.Queries;
using Lines.Domain.Shared;
using MediatR;

namespace Lines.Application.Features.Offers.DeleteOffersByTripRequestId.Orchestrators
{
    public record GetOfferIdsByTripRequestIdOrchestrator(Guid TripRequestId) : IRequest<Result<List<Guid>>>;



    public class GetOfferIdsByTripRequestIdOrchestratorHandler(
               RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetOfferIdsByTripRequestIdOrchestrator, Result<List<Guid>>>(parameters)
    {
        public override async Task<Result<List<Guid>>> Handle(GetOfferIdsByTripRequestIdOrchestrator request, CancellationToken cancellationToken)
        {
            var offerIds = await _mediator.Send(new GetOfferIdsByTripRequestIdQuery(request.TripRequestId), cancellationToken)
                                            .ConfigureAwait(false);

            return Result<List<Guid>>.Success(offerIds);
        }
    }
}
