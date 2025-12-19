using Lines.Application.Features.Cities.DTOs;
using Lines.Application.Features.Cities.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Cities;

public class CreateCityEndpoint : BaseController<CreateCityRequest, CreateCityResponse>
{
    private readonly BaseControllerParams<CreateCityRequest> _dependencyCollection;
    public CreateCityEndpoint(BaseControllerParams<CreateCityRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPost("city/create")]
    public async Task<ApiResponse<CreateCityResponse>> Create([FromBody] CreateCityRequest request)
    {
        var VlaidateResult = await ValidateRequestAsync(request);
        if (!VlaidateResult.IsSuccess)
            return VlaidateResult;
        
        var res = await _mediator.Send(new CreateCityOrchestrator(request.Name, request.Latitude, request.Longitude, request.VehicleTypes));
        return HandleResult<CreateCityDto, CreateCityResponse>(res);
    }
}