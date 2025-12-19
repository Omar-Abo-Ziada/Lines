using Lines.Application.Features.Trips.GetTrips.DTOs;
using Lines.Application.Features.Trips.GetTrips.Queries;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Trips.GetTrips;

public class GetTripsEndpoint : BaseController<GetTripsRequest, PagingDto<GetTripsDto>>
{
    private BaseControllerParams<GetTripsRequest> _dependencyCollection;
    public GetTripsEndpoint(BaseControllerParams<GetTripsRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("/trips/getall")]
    public async Task<ApiResponse<PagingDto<GetTripsDto>>> GetAll([FromQuery]  GetTripsRequest request)
    {
        var res = await _mediator.Send(new GetTripsQuery(request.Status, request.PageIndex, request.PageSize));
        return ApiResponse<PagingDto<GetTripsDto>>.SuccessResponse(res);
    }
}