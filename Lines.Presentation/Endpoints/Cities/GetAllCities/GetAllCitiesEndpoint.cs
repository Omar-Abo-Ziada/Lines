using Lines.Application.Features.Cities.DTOs;
using Lines.Application.Features.Cities.GetAllCities.Orchestrator;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Cities;

public class GetAllCitiesEndpoint : BaseController<GetAllCitiesRequest, PagingDto<GetAllCitiesResponse>>
{
    private readonly BaseControllerParams<GetAllCitiesRequest> _dependencyCollection;
    public GetAllCitiesEndpoint(BaseControllerParams<GetAllCitiesRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }


    [HttpGet("city/getall")]
    [Authorize]
    public async Task<ApiResponse<PagingDto<GetAllCitiesResponse>>> Create([FromQuery] GetAllCitiesRequest request)
    {
        var res = await _mediator.Send(new GetAllCitiesOrchestrator(request.Name, request.PageSize, request.PageNumber));
        return HandleResult<PagingDto<GetAllCitiesDto>, PagingDto<GetAllCitiesResponse>>(res);
    }
}