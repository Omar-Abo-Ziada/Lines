using Lines.Application.DTOs.Notifications;
using Lines.Application.Features.FCMNotifications.Commands;

namespace Lines.Presentation.Endpoints.FCMNotifications
{
    public class SendTestEndpoint : BaseController<SendTestNotificationCommand, NotificationResponse>
    {
        private readonly BaseControllerParams<SendTestNotificationCommand> _requestParam;
        SendTestEndpoint(BaseControllerParams<SendTestNotificationCommand> requestParam) : base(requestParam)
        {
            _requestParam = requestParam;

        }
         
        [HttpPost("/FCM/SendTest")]
        public async Task<ApiResponse<NotificationResponse>> SendTest(
          [FromBody] SendTestNotificationCommand request)
        {
            var validation = await ValidateRequestAsync(request);
            if (!validation.IsSuccess) return validation;

            var result = await _mediator.Send(request);
            return HandleResult<NotificationResponse, NotificationResponse>(result);
        }
    }
}
