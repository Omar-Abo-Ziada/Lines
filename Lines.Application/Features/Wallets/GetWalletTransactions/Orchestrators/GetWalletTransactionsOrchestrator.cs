using Lines.Application.Common;
using Lines.Application.Features.Wallets.GetWalletTransactions.Queries;

namespace Lines.Application.Features.Wallets.GetWalletTransactions.Orchestrators;

public record GetWalletTransactionsOrchestrator(Guid UserId, int Page, int PageSize) 
    : IRequest<Result<PaginatedWalletTransactionsDto>>;

public class GetWalletTransactionsOrchestratorHandler 
    : RequestHandlerBase<GetWalletTransactionsOrchestrator, Result<PaginatedWalletTransactionsDto>>
{
    public GetWalletTransactionsOrchestratorHandler(RequestHandlerBaseParameters parameters) 
        : base(parameters)
    {
    }

    public override async Task<Result<PaginatedWalletTransactionsDto>> Handle(
        GetWalletTransactionsOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetWalletTransactionsQuery(request.UserId, request.Page, request.PageSize), 
            cancellationToken);
        
        return result;
    }
}

