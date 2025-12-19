using Lines.Application.Common;
using Lines.Application.Features.RewardOffers.DTOs;
using Lines.Application.Features.RewardOffers.GetAvailableOffers.Queries;

namespace Lines.Application.Features.RewardOffers.GetAvailableOffers.Orchestrators;

public record GetAvailableOffersOrchestrator() : IRequest<Result<List<AvailableOfferDto>>>;

public class GetAvailableOffersOrchestratorHandler 
    : RequestHandlerBase<GetAvailableOffersOrchestrator, Result<List<AvailableOfferDto>>>
{
    public GetAvailableOffersOrchestratorHandler(RequestHandlerBaseParameters parameters) 
        : base(parameters)
    {
    }

    public override async Task<Result<List<AvailableOfferDto>>> Handle(
        GetAvailableOffersOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAvailableOffersQuery(), cancellationToken);
        return result;
    }
}

