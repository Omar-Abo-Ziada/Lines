using Lines.Application.Features.DriverEmergencyContacts.DeleteDriverEmergencyContact.Orchestrators;
using Lines.Application.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.EmergencyContacts.DeleteDriverEmergencyContact;

[Authorize]
public class DeleteDriverEmergencyContactEndpoint : BaseController<object, DeleteDriverEmergencyContactResponse>
{
    private readonly BaseControllerParams<object> _dependencyCollection;
    
    public DeleteDriverEmergencyContactEndpoint(BaseControllerParams<object> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpDelete("driver/emergency-contacts/{id}")]
    public async Task<ApiResponse<DeleteDriverEmergencyContactResponse>> DeleteEmergencyContact(
        [FromRoute] Guid id)
    {
        var driverId = GetCurrentDriverId();
        
        if (driverId == Guid.Empty)
        {
            return ApiResponse<DeleteDriverEmergencyContactResponse>.ErrorResponse(
                new Error("UNAUTHORIZED", "Driver not authenticated", ErrorType.UnAuthorized), 
                401);
        }

        var result = await _mediator.Send(
            new DeleteDriverEmergencyContactOrchestrator(id, driverId));

        if (!result.IsSuccess)
        {
            return ApiResponse<DeleteDriverEmergencyContactResponse>.ErrorResponse(result.Error, 400);
        }

        var response = new DeleteDriverEmergencyContactResponse
        {
            Success = result.Value
        };

        return ApiResponse<DeleteDriverEmergencyContactResponse>.SuccessResponse(response);
    }
}

