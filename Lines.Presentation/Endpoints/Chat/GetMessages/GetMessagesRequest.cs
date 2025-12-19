namespace Lines.Presentation.Endpoints.Chat.GetMessages
{
    public class GetMessagesRequest
    {
        public Guid TripId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
