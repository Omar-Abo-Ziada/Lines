 

using Lines.Application.DTOs.Notifications;
using Lines.Application.Features.FCMNotifications.Commands;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.FCMNotifications;

public class RegisterDeviceTokenEndpoint : BaseController<DeviceTokenRequest, bool>
{
    private readonly BaseControllerParams<DeviceTokenRequest> _dependencyCollection;

    public RegisterDeviceTokenEndpoint(BaseControllerParams<DeviceTokenRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpPost("/FCM/Register")]
    public async Task<ApiResponse<bool>> Register([FromBody] DeviceTokenRequest request)
    {
        var validationResult = await ValidateRequestAsync(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var res = await _mediator.Send(new RegisterOrUpdateTokenCommand(request));
        return HandleResult<bool, bool>(res);
    }

    

}

