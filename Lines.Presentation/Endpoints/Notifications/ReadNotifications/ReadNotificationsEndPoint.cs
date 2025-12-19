using Lines.Application.Features.Notifications.GetNotifications.Orchestrator;
using Lines.Application.Features.Notifications.ReadNotifications.Orchestrator;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.Orchestrator;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Notifications.ReadNotifications;

[AllowAnonymous]
public class ReadNotificationsEndPoint : BaseController<ReadNotificationsRequest, ReadNotificationsResponse>
{
    private readonly BaseControllerParams<ReadNotificationsRequest> _dependencyCollection;

    public ReadNotificationsEndPoint(BaseControllerParams<ReadNotificationsRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;

    }
    [HttpGet("ReadNotifications/Read")]
    public async Task<ApiResponse<ReadNotificationsResponse>> ReadNotifications([FromQuery] ReadNotificationsRequest request)
    {
        var res = await _mediator.Send(new ReadNotificationsOrchestrator(request.Id)).ConfigureAwait(false);

        if (res.IsSuccess)
        {
            var response = new ReadNotificationsResponse
            {
                Notification = res.Value,
            };
            return ApiResponse<ReadNotificationsResponse>.SuccessResponse(response);
        }

        return ApiResponse<ReadNotificationsResponse>.ErrorResponse(res.Error, 400);
    }
}

