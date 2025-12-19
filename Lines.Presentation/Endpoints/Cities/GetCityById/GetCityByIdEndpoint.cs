using Lines.Application.Features.Cities.DTOs;
using Lines.Application.Features.Cities.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Cities;

public class GetCityByIdEndpoint : BaseController<GetCityByIdRequest, GetCityByIdResponse>
{
    private readonly BaseControllerParams<GetCityByIdRequest> _dependencyCollection;
    public GetCityByIdEndpoint(BaseControllerParams<GetCityByIdRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("city/getbyid")]
    public async Task<ApiResponse<GetCityByIdResponse>> Create([FromQuery] GetCityByIdRequest request)
    {
        var VlaidateResult = await ValidateRequestAsync(request);
        if (!VlaidateResult.IsSuccess)
            return VlaidateResult;
        
        var res = await _mediator.Send(new GetCityByIdOrchestrator(request.Id));
        return HandleResult<CityByIdDto, GetCityByIdResponse>(res);
    }
}