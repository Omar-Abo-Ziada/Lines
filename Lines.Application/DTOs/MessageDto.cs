namespace Lines.Application.DTOs
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime? ReadAt { get; set; }
        public bool IsRead { get; set; }
        public Guid SenderId { get; set; }
        public Guid RecipientId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? SenderName { get; set; }
        public string? RecipientName { get; set; }
    }
}
