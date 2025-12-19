using Lines.Application.Features.Cities.DTOs;
using Lines.Application.Features.Cities.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Cities;

public class UpdateCityEndpoint : BaseController<UpdateCityRequest, bool>
{
    private readonly BaseControllerParams<UpdateCityRequest> _dependencyCollection;
    public UpdateCityEndpoint(BaseControllerParams<UpdateCityRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPut("city/update")]
    public async Task<ApiResponse<bool>> Update([FromBody] UpdateCityRequest request)
    {
        var VlaidateResult = await ValidateRequestAsync(request);
        if (!VlaidateResult.IsSuccess)
            return VlaidateResult;
        
        var res = await _mediator.Send(new UpdateCityOrchestrator(request.Id, request.Name));
        return HandleResult<bool, bool>(res);
    }
}