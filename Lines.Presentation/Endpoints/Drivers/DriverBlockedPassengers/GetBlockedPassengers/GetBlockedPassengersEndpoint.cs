using Lines.Application.Features.DriverBlockedPassengers.GetBlockedPassengers.DTOs;
using Lines.Application.Features.DriverBlockedPassengers.GetBlockedPassengers.Queries;

namespace Lines.Presentation.Endpoints.Drivers.DriverBlockedPassengers.GetBlockedPassengers;

public class GetBlockedPassengersEndpoint : BaseController<object, GetBlockedPassengersResponse>
{
    public GetBlockedPassengersEndpoint(BaseControllerParams<object> dependencyCollection) 
        : base(dependencyCollection)
    {
    }

    [Authorize]
    [HttpGet("driver-blocked-passengers")]
    public async Task<ApiResponse<GetBlockedPassengersResponse>> GetBlockedPassengers()
    {
        var driverId = GetCurrentDriverId();
        
        if (driverId == Guid.Empty)
        {
            return ApiResponse<GetBlockedPassengersResponse>.ErrorResponse(
                new Error("UNAUTHORIZED", "Driver not authenticated", ErrorType.UnAuthorized), 
                401);
        }

        var result = await _mediator.Send(new GetBlockedPassengersQuery(driverId));

        if (!result.IsSuccess)
        {
            return ApiResponse<GetBlockedPassengersResponse>.ErrorResponse(result.Error, 400);
        }

        var response = new GetBlockedPassengersResponse
        {
            BlockedPassengers = result.Value,
            TotalCount = result.Value.Count
        };

        return ApiResponse<GetBlockedPassengersResponse>.SuccessResponse(response);
    }
}

