using Lines.Application.Common;
using Lines.Application.Features.RewardOffers.ActivateOffer.Commands;
using Lines.Application.Features.RewardOffers.DTOs;

namespace Lines.Application.Features.RewardOffers.ActivateOffer.Orchestrators;

public record ActivateOfferOrchestrator(Guid DriverId, Guid OfferId) : IRequest<Result<ActivateOfferDto>>;

public class ActivateOfferOrchestratorHandler 
    : RequestHandlerBase<ActivateOfferOrchestrator, Result<ActivateOfferDto>>
{
    public ActivateOfferOrchestratorHandler(RequestHandlerBaseParameters parameters) 
        : base(parameters)
    {
    }

    public override async Task<Result<ActivateOfferDto>> Handle(
        ActivateOfferOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ActivateOfferCommand(request.DriverId, request.OfferId), 
            cancellationToken);
        
        return result;
    }
}

