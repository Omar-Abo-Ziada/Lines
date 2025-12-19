using Lines.Application.Features.Drivers.GetDriverAddress.DTOs;
using Lines.Application.Features.Drivers.GetDriverAddress.Orchestrators;
using Lines.Domain.Shared;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Drivers.GetDriverAddress;

[Authorize]
public class GetDriverAddressEndpoint : BaseController<GetDriverAddressRequest, GetDriverAddressResponse>
{
    private readonly BaseControllerParams<GetDriverAddressRequest> _dependencyCollection;
    
    public GetDriverAddressEndpoint(BaseControllerParams<GetDriverAddressRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("drivers/address")]
    public async Task<ApiResponse<GetDriverAddressResponse>> GetAddress()
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<DriverAddressDto, GetDriverAddressResponse>(
                Result<DriverAddressDto>.Failure(new Lines.Application.Shared.Error("UNAUTHORIZED", "User not authenticated", Lines.Application.Shared.ErrorType.Validation)));
        }

        var result = await _mediator.Send(new GetDriverAddressOrchestrator(userId));
        return HandleResult<DriverAddressDto, GetDriverAddressResponse>(result);
    }
}
