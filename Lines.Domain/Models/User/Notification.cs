using Lines.Domain.Enums;
using Lines.Domain.Models.Common;

namespace Lines.Domain.Models.Users
{
    public class Notification : BaseModel
    {
        public Guid UserId { get;  set; }
        public string Title { get;  set; }
        public string Message { get;  set; }
        public DateTime CreatedAt { get;  set; }
        public bool IsRead { get;  set; }
        public NotificationType NotificationType { get;  set; }

        // Constructor
        public Notification(Guid userId, string title, string message, NotificationType notificationType)
        {
            if (userId == Guid.Empty)
            //if (UserId == Guid.Empty)
                throw new ArgumentException("UserId must be valid.", nameof(userId));
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be empty.", nameof(message));
            if (notificationType <= 0)
                throw new ArgumentException("NotificationTypeId must be valid.", nameof(notificationType));

            UserId = userId;
            Title = title;
            Message = message;
            CreatedAt = DateTime.UtcNow;
            IsRead = false;
            NotificationType = notificationType;
        }


        // Just for data seeding
        public Notification()
        {

        }
        // Business Methods

        public void MarkAsRead()
        {
            IsRead = true;
        }

        public void UpdateMessage(string newMessage)
        {
            if (string.IsNullOrWhiteSpace(newMessage))
                throw new ArgumentException("Message cannot be empty.", nameof(newMessage));
            Message = newMessage;
        }

        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Title cannot be empty.", nameof(newTitle));
            Title = newTitle;
        }
    }
}