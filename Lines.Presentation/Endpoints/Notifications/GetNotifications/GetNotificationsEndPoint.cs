using Lines.Application.Features.Notifications.GetNotifications.Orchestrator;
using Lines.Application.Features.Offers.CreateOffer.DTOs;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.Orchestrator;
using Lines.Application.Shared;
using Lines.Domain.Shared;
using Lines.Presentation.Common;
using Lines.Presentation.Endpoints.Offers.CreateOffer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Notifications.GetNotifications;

[AllowAnonymous]
public class GetNotificationsEndPoint : BaseController<GetNotificationsRequest, GetNotificationsResponse>
{
    private readonly BaseControllerParams<GetNotificationsRequest> _dependencyCollection;

    public GetNotificationsEndPoint(BaseControllerParams<GetNotificationsRequest> dependencyCollection) : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;

    }
    [HttpGet("GetNotifications/GetAll")]
    [Authorize]
    public async Task<ApiResponse<GetNotificationsResponse>> GetNotifications([FromQuery] GetNotificationsRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return HandleResult<GetNotificationsRequest, GetNotificationsResponse>(
                Result<GetNotificationsRequest>.Failure(new Error(Code: "UNAUTHORIZED", Description: "User not authenticated", Type: ErrorType.Validation)));
        }
        var res = await _mediator.Send(new GetNotificationsOrchestrator(userId, request.IsRead)).ConfigureAwait(false);

        if (res.IsSuccess)
        {
            var response = new GetNotificationsResponse
            {
                Notifications = res.Value
            };
            return ApiResponse<GetNotificationsResponse>.SuccessResponse(response);
        }

        return ApiResponse<GetNotificationsResponse>.ErrorResponse(res.Error, 400);
    }
}

