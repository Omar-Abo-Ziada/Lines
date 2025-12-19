using Lines.Application.Features.Wallets.GetWalletTransactions.Orchestrators;
using Lines.Application.Features.Wallets.GetWalletTransactions.Queries;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Wallets.GetWalletTransactions;

public class GetWalletTransactionsEndpoint : BaseController<GetWalletTransactionsRequest, GetWalletTransactionsResponse>
{
    private readonly BaseControllerParams<GetWalletTransactionsRequest> _dependencyCollection;

    public GetWalletTransactionsEndpoint(BaseControllerParams<GetWalletTransactionsRequest> dependencyCollection)
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    /// <summary>
    /// Get paginated wallet transaction history for the logged-in user
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 20, max: 100)</param>
    /// <returns>Paginated list of wallet transactions</returns>
    /// <response code="200">Returns transaction history</response>
    /// <response code="400">Validation error (invalid pagination params)</response>
    /// <response code="401">Unauthorized - missing or invalid JWT token</response>
    /// <response code="404">Wallet not found</response>
    [HttpGet("wallet/transactions")]
    [Authorize]
    public async Task<ApiResponse<GetWalletTransactionsResponse>> GetWalletTransactions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var userId = GetCurrentUserId();
        //var driverId = GetCurrentDriverId();

        if (userId == Guid.Empty)
        {
            return ApiResponse<GetWalletTransactionsResponse>.ErrorResponse(
                new Error("AUTH:INVALID", "USER ID not found in token.", ErrorType.UnAuthorized),
                401);
        }

        var request = new GetWalletTransactionsRequest(page, pageSize);

        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var result = await _mediator.Send(new GetWalletTransactionsOrchestrator(userId, page, pageSize));

        return HandleResult<PaginatedWalletTransactionsDto, GetWalletTransactionsResponse>(result);
    }
}

