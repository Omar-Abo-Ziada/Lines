using Lines.Application.Features.DriverEmergencyContacts.UpdateDriverEmergencyContact.DTOs;
using Lines.Application.Features.DriverEmergencyContacts.UpdateDriverEmergencyContact.Orchestrators;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.EmergencyContacts.UpdateDriverEmergencyContact;

[Authorize]
public class UpdateDriverEmergencyContactEndpoint : BaseController<UpdateDriverEmergencyContactRequest, UpdateDriverEmergencyContactResponse>
{
    private readonly BaseControllerParams<UpdateDriverEmergencyContactRequest> _dependencyCollection;
    
    public UpdateDriverEmergencyContactEndpoint(BaseControllerParams<UpdateDriverEmergencyContactRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPut("driver/emergency-contacts/{id}")]
    public async Task<ApiResponse<UpdateDriverEmergencyContactResponse>> UpdateEmergencyContact(
        [FromRoute] Guid id,
        [FromBody] UpdateDriverEmergencyContactRequest request)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var driverId = GetCurrentDriverId();
        
        if (driverId == Guid.Empty)
        {
            return ApiResponse<UpdateDriverEmergencyContactResponse>.ErrorResponse(
                new Error("UNAUTHORIZED", "Driver not authenticated", ErrorType.UnAuthorized), 
                401);
        }

        var result = await _mediator.Send(
            new UpdateDriverEmergencyContactOrchestrator(id, driverId, request.Name, request.PhoneNumber));

        if (!result.IsSuccess)
        {
            return ApiResponse<UpdateDriverEmergencyContactResponse>.ErrorResponse(result.Error, 400);
        }

        var response = new UpdateDriverEmergencyContactResponse
        {
            EmergencyContact = result.Value
        };

        return ApiResponse<UpdateDriverEmergencyContactResponse>.SuccessResponse(response);
    }
}

