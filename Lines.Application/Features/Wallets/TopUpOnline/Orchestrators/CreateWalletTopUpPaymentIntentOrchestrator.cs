using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Features.Wallets.TopUpOnline.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.Wallets.TopUpOnline.Orchestrators
{
    
    public record CreateWalletTopUpPaymentIntentOrchestrator(
        Guid UserId,
        decimal Amount,
        string Currency
    ) : IRequest<Result<CreateWalletTopUpPaymentIntentDto>>;

    public class CreateWalletTopUpPaymentIntentOrchestratorHandler
        : RequestHandlerBase<CreateWalletTopUpPaymentIntentOrchestrator, Result<CreateWalletTopUpPaymentIntentDto>>
    {
        public CreateWalletTopUpPaymentIntentOrchestratorHandler(RequestHandlerBaseParameters parameters)
            : base(parameters)
        { }

        public override async Task<Result<CreateWalletTopUpPaymentIntentDto>> Handle(
            CreateWalletTopUpPaymentIntentOrchestrator request,
            CancellationToken cancellationToken)
        {
            return await _mediator.Send(
                new CreateWalletTopUpPaymentIntentCommand(request.UserId, request.Amount, request.Currency),
                cancellationToken);
        }
    }

}
