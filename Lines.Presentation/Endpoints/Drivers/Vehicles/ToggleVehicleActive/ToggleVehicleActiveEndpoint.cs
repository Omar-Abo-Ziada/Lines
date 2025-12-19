using Lines.Application.Features.Vehicles.ToggleVehicleActive.DTOs;
using Lines.Application.Features.Vehicles.ToggleVehicleActive.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.ToggleVehicleActive;

public class ToggleVehicleActiveEndpoint : BaseController<ToggleVehicleActiveRequest, ToggleVehicleActiveResponse>
{
    private readonly BaseControllerParams<ToggleVehicleActiveRequest> _dependencyCollection;
    
    public ToggleVehicleActiveEndpoint(BaseControllerParams<ToggleVehicleActiveRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPut("drivers/vehicles/{vehicleId}/toggle-active")]
    public async Task<ApiResponse<ToggleVehicleActiveResponse>> ToggleVehicleActive(
        [FromRoute] Guid vehicleId,
        [FromQuery] ToggleVehicleActiveRequest request)
    {
        var validateResult = await ValidateRequestAsync(request);
        if (!validateResult.IsSuccess)
            return validateResult;
        
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<VehicleActiveStatusDto, ToggleVehicleActiveResponse>(
                Result<VehicleActiveStatusDto>.Failure(new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation)));
        }

        var result = await _mediator.Send(new ToggleVehicleActiveOrchestrator(vehicleId, userId));
        return HandleResult<VehicleActiveStatusDto, ToggleVehicleActiveResponse>(result);
    }
}
