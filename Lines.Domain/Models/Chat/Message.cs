
using Lines.Domain.Enums;
using Lines.Domain.Models.Common;


namespace Lines.Domain.Models.Chats
{
    public abstract class Message : BaseModel // Changed to public abstract
    {
        public string? Content { get;  set; }
        public string? ImagePath { get; set; }
        public DateTime? ReadAt { get;  set; }
        public bool IsRead { get; set; } = false;
        public Guid SenderId { get;  set; }
        public Guid RecipientId { get;  set; }
        public MessageType MessageType { get; set; }

        public Message(Guid senderId , Guid recipientId , MessageType type , string? content, string? imagePath = null)
        {
            SenderId = senderId;
            RecipientId = recipientId;
            MessageType = type;
            ImagePath = imagePath;
            Content = content;
            Validate();
        }

        public Message()
        {
            // Parameterless constructor for EF Core
        }
        protected virtual void Validate()
        {
            if (SenderId == Guid.Empty)
                throw new ArgumentException("Sender ID is required.", nameof(SenderId));

            if (RecipientId == Guid.Empty)
                throw new ArgumentException("Recipient ID is required.", nameof(RecipientId));

            if (string.IsNullOrWhiteSpace(Content) && string.IsNullOrWhiteSpace(ImagePath))
                throw new ArgumentException("Either Content or ImagePath must be provided.");
        }
    }
}