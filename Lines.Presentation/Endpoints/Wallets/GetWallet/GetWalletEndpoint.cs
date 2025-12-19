using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Features.Wallets.GetWalletBalance.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Wallets.GetWallet;

public class GetWalletEndpoint : BaseController<GetWalletRequest, GetWalletResponse>
{
    private readonly BaseControllerParams<GetWalletRequest> _dependencyCollection;

    public GetWalletEndpoint(BaseControllerParams<GetWalletRequest> dependencyCollection)
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    /// <summary>
    /// Get wallet balance and recent transactions for the logged-in user
    /// </summary>
    /// <returns>Wallet balance and recent transactions</returns>
    /// <response code="200">Returns wallet details</response>
    /// <response code="401">Unauthorized - missing or invalid JWT token</response>
    [HttpGet("wallet")]
    [Authorize]
    public async Task<ApiResponse<GetWalletResponse>> GetWallet()
    {
        var userId = GetCurrentUserId();
        //var driverId = GetCurrentDriverId();

        if (userId == Guid.Empty)
        {
            return ApiResponse<GetWalletResponse>.ErrorResponse(
                new Error("AUTH:INVALID", "USER ID not found in token.", ErrorType.UnAuthorized),
                401);
        }

        var result = await _mediator.Send(new GetWalletBalanceOrchestrator(userId));

        return HandleResult<WalletBalanceDto, GetWalletResponse>(result);
    }
}

