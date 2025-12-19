using Lines.Application.Features.Drivers.GetDriverProfile.DTOs;
using Lines.Application.Features.Drivers.GetDriverProfile.Orchestrators;
using Lines.Domain.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.GetDriverProfile;

public class GetDriverProfileEndpoint : BaseController<GetDriverProfileRequest, GetDriverProfileResponse>
{
    private readonly BaseControllerParams<GetDriverProfileRequest> _dependencyCollection;
    
    public GetDriverProfileEndpoint(BaseControllerParams<GetDriverProfileRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("drivers/profile")]
    public async Task<ApiResponse<GetDriverProfileResponse>> GetProfile()
    {
        
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<DriverProfileDto, GetDriverProfileResponse>(
                Result<DriverProfileDto>.Failure(new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation)));
        }

        var result = await _mediator.Send(new GetDriverProfileOrchestrator(userId));
        return HandleResult<DriverProfileDto, GetDriverProfileResponse>(result);
    }
}
