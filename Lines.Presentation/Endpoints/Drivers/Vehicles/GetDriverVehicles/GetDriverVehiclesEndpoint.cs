using Lines.Application.Features.Vehicles.GetDriverVehicles.DTOs;
using Lines.Application.Features.Vehicles.GetDriverVehicles.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.GetDriverVehicles;

public class GetDriverVehiclesEndpoint : BaseController<GetDriverVehiclesRequest, GetDriverVehiclesResponse>
{
    private readonly BaseControllerParams<GetDriverVehiclesRequest> _dependencyCollection;
    
    public GetDriverVehiclesEndpoint(BaseControllerParams<GetDriverVehiclesRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("drivers/vehicles")]
    public async Task<ApiResponse<GetDriverVehiclesResponse>> GetVehicles(
        [FromQuery] GetDriverVehiclesRequest request)
    {
        var validateResult = await ValidateRequestAsync(request);
        if (!validateResult.IsSuccess)
            return validateResult;
        
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<List<DriverVehicleDto>, GetDriverVehiclesResponse>(
                Result<List<DriverVehicleDto>>.Failure(new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation)));
        }

        var result = await _mediator.Send(new GetDriverVehiclesOrchestrator(userId));
        return HandleResult<List<DriverVehicleDto>, GetDriverVehiclesResponse>(result);
    }
}
