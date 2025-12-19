using Lines.Application.Features.FavoriteLocations;
using Lines.Application.Features.FavoriteLocations.Queries;
using Lines.Application.Features.FavoriteLocations.Shared.DTOs;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.FavoriteLocations;

public class GetAllFavoriteLocationsEndpoint : BaseController<GetAllFavoriteLocationsRequest, List<GetAllFavoriteLocationsResponse>>
{
    private BaseControllerParams<GetAllFavoriteLocationsRequest> _dependencyCollection;
    public GetAllFavoriteLocationsEndpoint(BaseControllerParams<GetAllFavoriteLocationsRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("/FavoriteLocation/GetAll")]
    public async Task<ApiResponse<List<GetAllFavoriteLocationsResponse>>> GetAll([FromQuery] GetAllFavoriteLocationsRequest request)
    {
        var res = await _mediator.Send(new GetAllFavoriteLocationsQuery(request.Name));
        return HandleResult<List<GetAllFavoriteLocations>, List<GetAllFavoriteLocationsResponse>>(res);
    }
}