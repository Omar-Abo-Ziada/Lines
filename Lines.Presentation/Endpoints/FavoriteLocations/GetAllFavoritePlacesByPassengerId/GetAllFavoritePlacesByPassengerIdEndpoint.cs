using Lines.Application.Features.FavoriteLocations.Shared.DTOs;
using Lines.Application.Features.TripRequests.Orchestrator;

namespace Lines.Presentation.Endpoints.FavoriteLocations;

public class GetAllFavoritePlacesByPassengerIdEndpoint : BaseController<GetAllFavoritePlacesByPassengerIdRequest, List<GetAllFavoritePlacesByPassengerIdResponse>>
{
    private BaseControllerParams<GetAllFavoritePlacesByPassengerIdRequest> _dependencyCollection;
    public GetAllFavoritePlacesByPassengerIdEndpoint(BaseControllerParams<GetAllFavoritePlacesByPassengerIdRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("/FavoriteLocation/GetAllByPassengerId")]
    public async Task<ApiResponse<List<GetAllFavoritePlacesByPassengerIdResponse>>> GetAllByPassengerId([FromQuery] GetAllFavoritePlacesByPassengerIdRequest request)
    {
        var passengerId = GetCurrentPassengerId();
        var res = await _mediator.Send(new GetAllFavoritePlacesByPassengerIdOrchestrator(passengerId));
        return HandleResult<List<GetAllFavoriteLocations>, List<GetAllFavoritePlacesByPassengerIdResponse>>(res);
    }
}