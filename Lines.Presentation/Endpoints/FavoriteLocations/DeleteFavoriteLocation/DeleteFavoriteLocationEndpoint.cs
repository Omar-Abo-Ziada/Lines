using Lines.Application.Features.FavoriteLocations;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.FavoriteLocations;

public class DeleteFavoriteLocationEndpoint : BaseController<DeleteFavoriteLocationRequest, bool>
{
    private BaseControllerParams<DeleteFavoriteLocationRequest> _dependencyCollection;
    public DeleteFavoriteLocationEndpoint(BaseControllerParams<DeleteFavoriteLocationRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpDelete("/FavoriteLocation/Delete")]
    public async Task<ApiResponse<bool>> Delete([FromBody] DeleteFavoriteLocationRequest request)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }
        var res = await _mediator.Send(new DeleteFavoriteLocationCommand( request.Id));
        return HandleResult<bool, bool>(res);
    }
}