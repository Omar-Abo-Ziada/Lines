using Lines.Application.Features.Wallets.DTOs;
using Lines.Application.Features.Wallets.TopUpWallet.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Wallets.TopUpWallet;

public class TopUpWalletEndpoint : BaseController<TopUpWalletRequest, TopUpWalletResponse>
{
    private readonly BaseControllerParams<TopUpWalletRequest> _dependencyCollection;

    public TopUpWalletEndpoint(BaseControllerParams<TopUpWalletRequest> dependencyCollection)
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    /// <summary>
    /// Add funds to the driver's wallet
    /// </summary>
    /// <param name="request">Top-up request with amount</param>
    /// <returns>Transaction details with new balance</returns>
    /// <response code="200">Wallet topped up successfully</response>
    /// <response code="400">Validation error (invalid amount)</response>
    /// <response code="401">Unauthorized - missing or invalid JWT token</response>
    [HttpPost("wallet/topup")]
    [Authorize]
    public async Task<ApiResponse<TopUpWalletResponse>> TopUpWallet([FromBody] TopUpWalletRequest request)
    {
        var driverId = GetCurrentDriverId();

        if (driverId == Guid.Empty)
        {
            return ApiResponse<TopUpWalletResponse>.ErrorResponse(
                new Error("AUTH:INVALID", "Driver ID not found in token.", ErrorType.UnAuthorized),
                401);
        }

        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var result = await _mediator.Send(new TopUpWalletOrchestrator(driverId, request.Amount));

        return HandleResult<TopUpWalletDto, TopUpWalletResponse>(result);
    }
}

