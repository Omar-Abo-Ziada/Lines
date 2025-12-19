using Lines.Application.Common;
using Lines.Application.Features.RewardOffers.DTOs;
using Lines.Application.Features.RewardOffers.GetActiveOffer.Queries;

namespace Lines.Application.Features.RewardOffers.GetActiveOffer.Orchestrators;

public record GetActiveOfferOrchestrator(Guid DriverId) : IRequest<Result<ActiveOfferDto>>;

public class GetActiveOfferOrchestratorHandler 
    : RequestHandlerBase<GetActiveOfferOrchestrator, Result<ActiveOfferDto>>
{
    public GetActiveOfferOrchestratorHandler(RequestHandlerBaseParameters parameters) 
        : base(parameters)
    {
    }

    public override async Task<Result<ActiveOfferDto>> Handle(
        GetActiveOfferOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActiveOfferQuery(request.DriverId), cancellationToken);
        return result;
    }
}

