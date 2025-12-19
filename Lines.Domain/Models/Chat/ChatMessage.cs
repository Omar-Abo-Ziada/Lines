using Lines.Domain.Models.Trips;

namespace Lines.Domain.Models.Chats
{
    public class ChatMessage : Message
    {
        public Guid TripId { get; set; }
        public virtual Trip Trip { get; set; }

        public ChatMessage(Guid tripId, Guid senderId , Guid recipientId , string? content, string? imagePath = null)
            : base(senderId , recipientId , Enums.MessageType.ChatMsg , imagePath , content)
        {
            TripId = tripId;
        }

        public ChatMessage()
        {
            // Parameterless constructor for EF Core

        }

        protected override void Validate() 
        {
            if (TripId == Guid.Empty)
                throw new ArgumentException("Trip ID is required.", nameof(TripId));

            base.Validate();
        }

    }
}