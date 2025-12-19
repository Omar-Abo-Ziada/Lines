using Lines.Application.Features.Trips.CollectTripFare.Orchestrators;
using Lines.Application.Interfaces;

namespace Lines.Presentation.Endpoints.Trips.CollectFare;

public class CollectTripFareEndpoint : BaseController<CollectTripFareRequest, CollectTripFareResponse>
{
    private readonly BaseControllerParams<CollectTripFareRequest> _dependencyCollection;
    
    public CollectTripFareEndpoint(BaseControllerParams<CollectTripFareRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPost("trips/{tripId}/collect-fare")]
    public async Task<ApiResponse<CollectTripFareResponse>> CollectFare(Guid tripId)
    {
        var request = new CollectTripFareRequest(tripId);
        
        var validateResult = await ValidateRequestAsync(request);
        if (!validateResult.IsSuccess)
            return validateResult;

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<bool, CollectTripFareResponse>(
                Result<bool>.Failure(new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation)));
        }

        // Get driver ID from user ID using ApplicationUserService
        var applicationUserService = HttpContext.RequestServices.GetRequiredService<IApplicationUserService>();
        var userDriverIds = await applicationUserService.GetPassengerAndDriverIdsByUserIdAsync(userId);
        
        if (userDriverIds?.DriverId == null)
        {
            return HandleResult<bool, CollectTripFareResponse>(
                Result<bool>.Failure(new Lines.Application.Shared.Error("NOT_DRIVER", "User is not a driver", Lines.Application.Shared.ErrorType.Validation)));
        }

        var result = await _mediator.Send(new CollectTripFareOrchestrator(tripId, userDriverIds.DriverId.Value));

        if (result.IsSuccess)
        {
            var response = new CollectTripFareResponse(true, "Fare collected successfully");
            return HandleResult<bool, CollectTripFareResponse>(Result<bool>.Success(true));
        }
        else
        {
            return HandleResult<bool, CollectTripFareResponse>(result);
        }
    }
}
