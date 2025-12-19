using Lines.Application.Features.Cities.DeleteCity.Orchestrator;
using Lines.Application.Features.Cities.DTOs;
using Lines.Application.Features.Cities.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Cities;

public class DeleteCityEndpoint : BaseController<DeleteCityRequest, bool>
{
    private readonly BaseControllerParams<DeleteCityRequest> _dependencyCollection;
    public DeleteCityEndpoint(BaseControllerParams<DeleteCityRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpDelete("city/delete")]
    public async Task<ApiResponse<bool>> Create([FromBody] DeleteCityRequest request)
    {
        var VlaidateResult = await ValidateRequestAsync(request);
        if (!VlaidateResult.IsSuccess)
            return VlaidateResult;
        
        var res = await _mediator.Send(new DeleteCityOrchestrator(request.Id));
        return HandleResult<bool, bool>(res);
    }
}