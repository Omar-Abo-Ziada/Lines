using Lines.Application.Features.Vehicles.GetVehicleDetails.DTOs;
using Lines.Application.Features.Vehicles.GetVehicleDetails.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.Vehicles.GetVehicleDetails;

public class GetVehicleDetailsEndpoint : BaseController<GetVehicleDetailsRequest, GetVehicleDetailsResponse>
{
    private readonly BaseControllerParams<GetVehicleDetailsRequest> _dependencyCollection;
    
    public GetVehicleDetailsEndpoint(BaseControllerParams<GetVehicleDetailsRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("drivers/vehicles/{vehicleId}")]
    public async Task<ApiResponse<GetVehicleDetailsResponse>> GetVehicleDetails(
        [FromRoute] Guid vehicleId,
        [FromQuery] GetVehicleDetailsRequest request)
    {
        var validateResult = await ValidateRequestAsync(request);
        if (!validateResult.IsSuccess)
            return validateResult;
        
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<VehicleDetailsDto, GetVehicleDetailsResponse>(
                Result<VehicleDetailsDto>.Failure(new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation)));
        }

        var result = await _mediator.Send(new GetVehicleDetailsOrchestrator(vehicleId, userId));
        return HandleResult<VehicleDetailsDto, GetVehicleDetailsResponse>(result);
    }
}
