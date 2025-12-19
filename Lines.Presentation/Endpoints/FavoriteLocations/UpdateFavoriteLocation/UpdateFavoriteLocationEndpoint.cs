using Lines.Application.Features.FavoriteLocations;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.FavoriteLocations;

public class UpdateFavoriteLocationEndpoint : BaseController<UpdateFavoriteLocationRequest, bool>
{
    private BaseControllerParams<UpdateFavoriteLocationRequest> _dependencyCollection;
    public UpdateFavoriteLocationEndpoint(BaseControllerParams<UpdateFavoriteLocationRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPut("/FavoriteLocation/Update")]
    public async Task<ApiResponse<bool>> Update([FromBody] UpdateFavoriteLocationRequest request)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var res = await _mediator.Send(new UpdateFavoriteLocationCommand( request.Id, request.Latitude,  request.Longtude, request.Name,request.PassengerId, request.CityId, request.SectorId));
        return HandleResult<bool, bool>(res);
    }
}