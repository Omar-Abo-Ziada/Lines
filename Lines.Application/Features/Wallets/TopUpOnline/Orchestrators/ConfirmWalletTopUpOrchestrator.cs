using Lines.Application.Common;
using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Features.Wallets.TopUpOnline.Commands;

namespace Lines.Application.Features.Wallets.TopUpOnline.Orchestrators;

public record ConfirmWalletTopUpOrchestrator(
    Guid UserId,
    string PaymentIntentId
) : IRequest<Result<TopUpWalletDto>>;

public class ConfirmWalletTopUpOrchestratorHandler
    : RequestHandlerBase<ConfirmWalletTopUpOrchestrator, Result<TopUpWalletDto>>
{
    public ConfirmWalletTopUpOrchestratorHandler(RequestHandlerBaseParameters parameters)
        : base(parameters)
    { }

    public override async Task<Result<TopUpWalletDto>> Handle(
        ConfirmWalletTopUpOrchestrator request,
        CancellationToken cancellationToken)
    {
        return await _mediator.Send(
            new ConfirmWalletTopUpCommand(request.UserId, request.PaymentIntentId),
            cancellationToken);
    }
}
