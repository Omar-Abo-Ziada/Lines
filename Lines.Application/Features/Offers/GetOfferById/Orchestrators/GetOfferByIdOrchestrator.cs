using Lines.Application.Features.Offers.GetOfferById.Queries;
using Lines.Domain.Models.Drivers;

namespace Lines.Application.Features.Offers.GetOfferById.Orchestrators
{
    public record GetOfferByIdOrchestrator(Guid Id) : IRequest<Result<Offer>>;


    public class GetOfferByIdOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : RequestHandlerBase<GetOfferByIdOrchestrator, Result<Offer>>(parameters)
    {
        public override async Task<Result<Offer>> Handle(GetOfferByIdOrchestrator request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetOfferByIdQuery(request.Id), cancellationToken).ConfigureAwait(false);
            return Result<Offer>.Success(result);
        }
    }
}
