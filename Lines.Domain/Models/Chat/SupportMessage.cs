namespace Lines.Domain.Models.Chats
{
    public class SupportMsg : Message  // in case it may have unique business in future
    {
        public SupportMsg(Guid senderId, Guid recipientId, string? imagePath, string? content) 
            : base(senderId , recipientId , Enums.MessageType.SupportMsg , imagePath , content) 
        {
            MessageType = Enums.MessageType.SupportMsg;
        }
        public SupportMsg()
        {
            // Parameterless constructor for EF Core
        }
    }

}