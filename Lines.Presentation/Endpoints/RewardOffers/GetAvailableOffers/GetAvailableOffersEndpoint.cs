using Lines.Application.Features.RewardOffers.DTOs;
using Lines.Application.Features.RewardOffers.GetAvailableOffers.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.RewardOffers.GetAvailableOffers;

public class GetAvailableOffersEndpoint : BaseController<GetAvailableOffersRequest, List<GetAvailableOffersResponse>>
{
    private readonly BaseControllerParams<GetAvailableOffersRequest> _dependencyCollection;

    public GetAvailableOffersEndpoint(BaseControllerParams<GetAvailableOffersRequest> dependencyCollection)
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    /// <summary>
    /// Get all available reward offers
    /// </summary>
    /// <returns>List of available offers</returns>
    /// <response code="200">Returns list of available offers</response>
    [HttpGet("reward-offers")]
    public async Task<ApiResponse<List<GetAvailableOffersResponse>>> GetAvailableOffers()
    {
        var result = await _mediator.Send(new GetAvailableOffersOrchestrator());

        return HandleResult<List<AvailableOfferDto>, List<GetAvailableOffersResponse>>(result);
    }
}

