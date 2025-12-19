using Lines.Application.Features.RewardOffers.ActivateOffer.Orchestrators;
using Lines.Application.Features.RewardOffers.DTOs;
using Lines.Presentation.Common;
using Lines.Presentation.Endpoints.RewardOffers.GetActiveOffer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.RewardOffers.ActivateOffer;

public class ActivateOfferEndpoint : BaseController<ActivateOfferRequest, ActivateOfferResponse>
{
    private readonly BaseControllerParams<ActivateOfferRequest> _dependencyCollection;

    public ActivateOfferEndpoint(BaseControllerParams<ActivateOfferRequest> dependencyCollection)
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    /// <summary>
    /// Activate a reward offer for the logged-in driver
    /// </summary>
    /// <param name="offerId">The ID of the offer to activate</param>
    /// <returns>Activation details including payment reference and new wallet balance</returns>
    /// <response code="200">Offer activated successfully</response>
    /// <response code="400">Validation error (insufficient balance, already has active offer, etc.)</response>
    /// <response code="401">Unauthorized - missing or invalid JWT token</response>
    /// <response code="404">Offer not found</response>
    [HttpPost("reward-offers/activate/{offerId}")]
    [Authorize]
    public async Task<ApiResponse<ActivateOfferResponse>> ActivateOffer([FromRoute] Guid offerId)
    {
        var driverId = GetCurrentDriverId();

        if (driverId == Guid.Empty)
        {
            return ApiResponse<ActivateOfferResponse>.ErrorResponse(
                 new Error("AUTH:INVALID", "Driver ID not found in token.", ErrorType.UnAuthorized),
                 401);
        }

        var request = new ActivateOfferRequest(offerId);

        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var result = await _mediator.Send(new ActivateOfferOrchestrator(driverId, offerId));

        return HandleResult<ActivateOfferDto, ActivateOfferResponse>(result);
    }
}

