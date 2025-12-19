using Lines.Application.Features.Drivers.GetDriverContact.DTOs;
using Lines.Application.Features.Drivers.GetDriverContact.Orchestrators;
using Lines.Domain.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.GetDriverContact;

[Authorize]
public class GetDriverContactEndpoint : BaseController<GetDriverContactRequest, GetDriverContactResponse>
{
    private readonly BaseControllerParams<GetDriverContactRequest> _dependencyCollection;
    
    public GetDriverContactEndpoint(BaseControllerParams<GetDriverContactRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("drivers/contact")]
    public async Task<ApiResponse<GetDriverContactResponse>> GetContact()
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<DriverContactDto, GetDriverContactResponse>(
                Result<DriverContactDto>.Failure(new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation)));
        }

        var result = await _mediator.Send(new GetDriverContactOrchestrator(userId));
        return HandleResult<DriverContactDto, GetDriverContactResponse>(result);
    }
}
