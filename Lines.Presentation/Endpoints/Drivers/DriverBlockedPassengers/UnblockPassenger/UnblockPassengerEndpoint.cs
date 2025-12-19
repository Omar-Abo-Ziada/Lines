using Lines.Application.Features.DriverBlockedPassengers.UnblockPassenger.Commands;
using Lines.Application.Features.DriverBlockedPassengers.UnblockPassenger.DTOs;
using Lines.Domain.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.DriverBlockedPassengers.UnblockPassenger;

public class UnblockPassengerEndpoint : BaseController<object, UnblockPassengerResponse>
{
    public UnblockPassengerEndpoint(BaseControllerParams<object> dependencyCollection) 
        : base(dependencyCollection)
    {
    }

    [Authorize]
    [HttpDelete("driver-blocked-passengers/{passengerId}")]
    public async Task<ApiResponse<UnblockPassengerResponse>> UnblockPassenger([FromRoute] Guid passengerId)
    {
        var driverId = GetCurrentDriverId();
        
        if (driverId == Guid.Empty)
        {
            return ApiResponse<UnblockPassengerResponse>.ErrorResponse(
                new Error("UNAUTHORIZED", "Driver not authenticated", ErrorType.UnAuthorized), 
                401);
        }

        var result = await _mediator.Send(new UnblockPassengerCommand(driverId, passengerId));

        if (!result.IsSuccess)
        {
            return ApiResponse<UnblockPassengerResponse>.ErrorResponse(result.Error, 400);
        }

        var response = new UnblockPassengerResponse
        {
            Success = result.Value
        };

        return ApiResponse<UnblockPassengerResponse>.SuccessResponse(response);
    }
}

