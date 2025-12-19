using Lines.Application.Features.FavoriteLocations;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.FavoriteLocations;

public class CreateFavoriteLocationEndpoint : BaseController<CreateFavoriteLocationRequest, bool>
{
    private BaseControllerParams<CreateFavoriteLocationRequest> _dependencyCollection;
    public CreateFavoriteLocationEndpoint(BaseControllerParams<CreateFavoriteLocationRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPost("/FavoriteLocation/Create")]
    public async Task<ApiResponse<bool>> Create([FromBody] CreateFavoriteLocationRequest request)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var res = await _mediator.Send(new CreateFavoriteLocationCommand(request.Latitude,  request.Longtude, request.Name,request.PassengerId, request.CityId, request.SectorId));
        return HandleResult<bool, bool>(res);
    }
}