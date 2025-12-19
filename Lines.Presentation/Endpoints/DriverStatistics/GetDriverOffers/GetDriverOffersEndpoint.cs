using Lines.Application.Features.DriverStatistics.DTOs;
using Lines.Application.Features.DriverStatistics.GetDriverOffers.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.DriverStatistics.GetDriverOffers;

public class GetDriverOffersEndpoint : BaseController<GetDriverOffersRequest, List<GetDriverOffersResponse>>
{
    private readonly BaseControllerParams<GetDriverOffersRequest> _dependencyCollection;

    public GetDriverOffersEndpoint(BaseControllerParams<GetDriverOffersRequest> dependencyCollection)
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("driver-statistics/offers/{driverId}")]
    [Authorize]
    public async Task<ApiResponse<List<GetDriverOffersResponse>>> GetDriverOffers(Guid driverId)
    {
        var request = new GetDriverOffersRequest(driverId);

        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var result = await _mediator.Send(new GetDriverOffersOrchestrator(driverId));

        return HandleResult<List<DriverOfferDto>, List<GetDriverOffersResponse>>(result);
    }
}


