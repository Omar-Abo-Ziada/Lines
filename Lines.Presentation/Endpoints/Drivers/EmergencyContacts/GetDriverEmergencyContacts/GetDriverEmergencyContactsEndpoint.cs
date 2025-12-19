using Lines.Application.Features.DriverEmergencyContacts.GetDriverEmergencyContacts.Orchestrators;
using Lines.Application.Features.DriverEmergencyContacts.Shared.DTOs;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.EmergencyContacts.GetDriverEmergencyContacts;

[Authorize]
public class GetDriverEmergencyContactsEndpoint : BaseController<GetDriverEmergencyContactsRequest, GetDriverEmergencyContactsResponse>
{
    private readonly BaseControllerParams<GetDriverEmergencyContactsRequest> _dependencyCollection;
    
    public GetDriverEmergencyContactsEndpoint(BaseControllerParams<GetDriverEmergencyContactsRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("driver/emergency-contacts")]
    public async Task<ApiResponse<GetDriverEmergencyContactsResponse>> GetEmergencyContacts(
        [FromQuery] GetDriverEmergencyContactsRequest request)
    {
        var validateResult = await ValidateRequestAsync(request);
        if (!validateResult.IsSuccess)
            return validateResult;
        
        var driverId = GetCurrentDriverId();
        if (driverId == Guid.Empty)
        {
            return HandleResult<List<DriverEmergencyContactDto>, GetDriverEmergencyContactsResponse>(
                Result<List<DriverEmergencyContactDto>>.Failure(new Error("UNAUTHORIZED", "Driver not authenticated", ErrorType.UnAuthorized)));
        }

        // Use provided driverId or authenticated driver's ID
        var targetDriverId = request.DriverId ?? driverId;
        
        // Ensure driver can only access their own contacts
        if (targetDriverId != driverId)
        {
            return HandleResult<List<DriverEmergencyContactDto>, GetDriverEmergencyContactsResponse>(
                Result<List<DriverEmergencyContactDto>>.Failure(new Error("UNAUTHORIZED", "You can only access your own emergency contacts", ErrorType.UnAuthorized)));
        }

        var result = await _mediator.Send(new GetDriverEmergencyContactsOrchestrator(targetDriverId));
        return HandleResult<List<DriverEmergencyContactDto>, GetDriverEmergencyContactsResponse>(result);
    }
}

