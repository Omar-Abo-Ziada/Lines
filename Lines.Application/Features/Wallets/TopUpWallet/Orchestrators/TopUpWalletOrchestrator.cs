using Lines.Application.Common;
using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Features.Wallets.TopUpWallet.Commands;

namespace Lines.Application.Features.Wallets.TopUpWallet.Orchestrators;

public record TopUpWalletOrchestrator(Guid UserId, decimal Amount) : IRequest<Result<TopUpWalletDto>>;

public class TopUpWalletOrchestratorHandler 
    : RequestHandlerBase<TopUpWalletOrchestrator, Result<TopUpWalletDto>>
{
    public TopUpWalletOrchestratorHandler(RequestHandlerBaseParameters parameters) 
        : base(parameters)
    {
    }

    public override async Task<Result<TopUpWalletDto>> Handle(
        TopUpWalletOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new TopUpWalletCommand(request.UserId, request.Amount), 
            cancellationToken);
        
        return result;
    }
}

