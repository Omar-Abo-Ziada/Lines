using Lines.Application.Features.DriverBlockedPassengers.BlockPassenger.Commands;
using Lines.Application.Features.DriverBlockedPassengers.BlockPassenger.DTOs;

namespace Lines.Presentation.Endpoints.Drivers.DriverBlockedPassengers.BlockPassenger;

public class BlockPassengerEndpoint : BaseController<BlockPassengerRequest, BlockPassengerResponse>
{
    public BlockPassengerEndpoint(BaseControllerParams<BlockPassengerRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
    }

    [Authorize]
    [HttpPost("driver-blocked-passengers")]
    public async Task<ApiResponse<BlockPassengerResponse>> BlockPassenger([FromBody] BlockPassengerRequest request)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var driverId = GetCurrentDriverId();
        
        if (driverId == Guid.Empty)
        {
            return ApiResponse<BlockPassengerResponse>.ErrorResponse(
                new Error("UNAUTHORIZED", "Driver not authenticated", ErrorType.UnAuthorized), 
                401);
        }

        var result = await _mediator.Send(
            new BlockPassengerCommand(driverId, request.PassengerId, request.Reason));

        if (!result.IsSuccess)
        {
            return ApiResponse<BlockPassengerResponse>.ErrorResponse(result.Error, 400);
        }

        var response = new BlockPassengerResponse
        {
            BlockId = result.Value
        };

        return ApiResponse<BlockPassengerResponse>.SuccessResponse(response, 201);
    }
}

