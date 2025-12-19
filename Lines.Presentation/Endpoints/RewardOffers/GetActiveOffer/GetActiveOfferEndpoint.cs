using Lines.Application.Features.RewardOffers.DTOs;
using Lines.Application.Features.RewardOffers.GetActiveOffer.Orchestrators;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.RewardOffers.GetActiveOffer;

public class GetActiveOfferEndpoint : BaseController<GetActiveOfferRequest, GetActiveOfferResponse>
{
    private readonly BaseControllerParams<GetActiveOfferRequest> _dependencyCollection;

    public GetActiveOfferEndpoint(BaseControllerParams<GetActiveOfferRequest> dependencyCollection)
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    /// <summary>
    /// Get the currently active offer for the logged-in driver
    /// </summary>
    /// <returns>Active offer details or null if no active offer</returns>
    /// <response code="200">Returns active offer or null</response>
    /// <response code="401">Unauthorized - missing or invalid JWT token</response>
    [HttpGet("reward-offers/active")]
    [Authorize]
    public async Task<ApiResponse<GetActiveOfferResponse>> GetActiveOffer()
    {
        var driverId = GetCurrentDriverId();

        if (driverId == Guid.Empty)
        {
            return ApiResponse<GetActiveOfferResponse>.ErrorResponse(
                new Error("AUTH:INVALID", "Driver ID not found in token.", ErrorType.UnAuthorized),
                401);
        }

        var result = await _mediator.Send(new GetActiveOfferOrchestrator(driverId));

        return HandleResult<ActiveOfferDto, GetActiveOfferResponse>(result);
    }
}

