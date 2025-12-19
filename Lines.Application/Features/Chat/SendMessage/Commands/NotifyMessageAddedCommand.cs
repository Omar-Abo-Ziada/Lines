using Lines.Application.Hubs;
using Lines.Domain.Models.Chats;
using Microsoft.AspNetCore.SignalR;


namespace Lines.Application.Features.Chat.SendMessage.Commands
{
    public record NotifyMessageAddedCommand(Guid tripId, Guid senderId, Guid recipientId, string? content, Guid messageID, string? imagePath = null)
         : IRequest<Unit>;

    public class NotifyMessageAddedCommandHandler : RequestHandlerBase<NotifyMessageAddedCommand, Unit>
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IFcmNotifier _notifier;

        public NotifyMessageAddedCommandHandler(RequestHandlerBaseParameters parameters, IHubContext<ChatHub> hubContext, IFcmNotifier notifier) : base(parameters)
        {
            _hubContext = hubContext;
            _notifier = notifier;
        }

        public override async Task<Unit> Handle(NotifyMessageAddedCommand request, CancellationToken cancellationToken)
        {
            var messageData = new
            {
                TripId = request.tripId,
                SenderId = request.senderId,
                RecipientId = request.recipientId,
                Content = request.content,
                MessageId = request.messageID,
                ImagePath = request.imagePath
            };

            await _hubContext.Clients.Group($"trip_{request.tripId}")
                .SendAsync("ReceiveMessage", messageData);


            #region FCM Firebase
            // ======================================================
            // 2) Prepare FCM MESSAGE BODY
            // ======================================================
            string fcmBody = request.imagePath != null
                ? "📷 صورة جديدة في الدردشة"
                : (!string.IsNullOrWhiteSpace(request.content)
                        ? (request.content.Length > 30 ? request.content[..30] + "..." : request.content)
                        : "لديك رسالة جديدة");

            var fcmData = new
            {
                TripId = request.tripId,
                SenderId = request.senderId,
                MessageId = request.messageID,
                IsImage = request.imagePath != null,
                Preview = request.content
            };


            // ======================================================
            // 3) Send FCM Push Notification
            // ======================================================
            await _notifier.SendToUserAsync(
                request.recipientId,
                NotificationTemplates.NewChatMessageTitle,
                fcmBody,
                fcmData
            );

            #endregion
            return Unit.Value;
        }
    }
}
