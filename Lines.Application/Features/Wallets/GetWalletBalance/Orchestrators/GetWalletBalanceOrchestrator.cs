using Lines.Application.Common;
using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Features.Wallets.GetWalletBalance.Queries;

namespace Lines.Application.Features.Wallets.GetWalletBalance.Orchestrators;

public record GetWalletBalanceOrchestrator(Guid UserId) : IRequest<Result<WalletBalanceDto>>;

public class GetWalletBalanceOrchestratorHandler 
    : RequestHandlerBase<GetWalletBalanceOrchestrator, Result<WalletBalanceDto>>
{
    public GetWalletBalanceOrchestratorHandler(RequestHandlerBaseParameters parameters) 
        : base(parameters)
    {
    }

    public override async Task<Result<WalletBalanceDto>> Handle(
        GetWalletBalanceOrchestrator request,
        CancellationToken cancellationToken)
    {
        //????? ????? ?????: ??????? ?? ???? userId not driverid
        var result = await _mediator.Send(new GetWalletBalanceQuery(request.UserId), cancellationToken);
        return result;
    }
}

