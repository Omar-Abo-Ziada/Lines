using Lines.Application.Features.DriverEmergencyContacts.CreateDriverEmergencyContact.DTOs;
using Lines.Application.Features.DriverEmergencyContacts.CreateDriverEmergencyContact.Orchestrators;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.EmergencyContacts.CreateDriverEmergencyContact;

[Authorize]
public class CreateDriverEmergencyContactEndpoint : BaseController<CreateDriverEmergencyContactRequest, CreateDriverEmergencyContactResponse>
{
    private readonly BaseControllerParams<CreateDriverEmergencyContactRequest> _dependencyCollection;
    
    public CreateDriverEmergencyContactEndpoint(BaseControllerParams<CreateDriverEmergencyContactRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPost("driver/emergency-contacts")]
    public async Task<ApiResponse<CreateDriverEmergencyContactResponse>> CreateEmergencyContact(
        [FromBody] CreateDriverEmergencyContactRequest request)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var driverId = GetCurrentDriverId();
        
        if (driverId == Guid.Empty)
        {
            return ApiResponse<CreateDriverEmergencyContactResponse>.ErrorResponse(
                new Error("UNAUTHORIZED", "Driver not authenticated", ErrorType.UnAuthorized), 
                401);
        }

        var result = await _mediator.Send(
            new CreateDriverEmergencyContactOrchestrator(driverId, request.Name, request.PhoneNumber));

        if (!result.IsSuccess)
        {
            return ApiResponse<CreateDriverEmergencyContactResponse>.ErrorResponse(result.Error, 400);
        }

        var response = new CreateDriverEmergencyContactResponse
        {
            EmergencyContact = result.Value
        };

        return ApiResponse<CreateDriverEmergencyContactResponse>.SuccessResponse(response, 201);
    }
}

