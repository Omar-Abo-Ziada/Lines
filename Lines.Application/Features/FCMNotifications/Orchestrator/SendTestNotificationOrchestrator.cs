using Lines.Application.DTOs.Notifications;
using Lines.Application.Features.FCMNotifications.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.FCMNotifications.Orchestrator
{
    public class SendTestNotificationOrchestrator
     : RequestHandlerBase<SendTestNotificationCommand, Result<NotificationResponse>>
    {
        private readonly IFcmNotifier _notifier;
        public SendTestNotificationOrchestrator(RequestHandlerBaseParameters p, IFcmNotifier notifier)
            : base(p) { _notifier = notifier; }

        public override async Task<Result<NotificationResponse>> Handle(SendTestNotificationCommand request, CancellationToken ct)
        {
            var res = await _notifier.SendToUserAsync(request.UserId, request.Title, request.Body, new { type = "test" });
            return Result<NotificationResponse>.Success(res);
        }
    }
}
