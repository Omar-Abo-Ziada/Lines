using Lines.Application.Features.FavoriteLocations.GetFavoriteLocationById.DTOs;
using Lines.Application.Features.FavoriteLocations.GetFavoriteLocationById.Queries;

namespace Lines.Presentation.Endpoints.FavoriteLocations;

public class GetFavoriteLocationByIdEndpoint : BaseController<GetFavoriteLocationByIdRequest, GetFavoriteLocationByIdResponse>
{
    private BaseControllerParams<GetFavoriteLocationByIdRequest> _dependencyCollection;
    public GetFavoriteLocationByIdEndpoint(BaseControllerParams<GetFavoriteLocationByIdRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("/FavoriteLocation/getbyid")]
    public async Task<ApiResponse<GetFavoriteLocationByIdResponse>> GetAll([FromQuery] GetFavoriteLocationByIdRequest request)
    {
        var res = await _mediator.Send(new GetFavoriteLocationByIdQuery(request.Id));
        return HandleResult<GetFavoriteLocationByIdDto, GetFavoriteLocationByIdResponse>(res);
    }
}