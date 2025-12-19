using Lines.Domain.Enums;
using Lines.Domain.Models.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    public class ChatMessageConfig : IEntityTypeConfiguration<ChatMessage>
    {

        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
   

            builder.Property(cm => cm.TripId)
                    .IsRequired();

            builder.HasOne(cm => cm.Trip)
             .WithMany(t => t.Messages)
             .HasForeignKey(cm => cm.TripId)
             .OnDelete(DeleteBehavior.NoAction);

            // Data seeding example
            builder.HasData(
                new ChatMessage
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Content = "Hello, I'll be arriving in 5 minutes for our trip.",
                    IsRead = true,
                    SenderId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    RecipientId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    TripId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    MessageType = MessageType.ChatMsg
                },
                new ChatMessage
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Content = "Great, I'm at the pickup point.",
                    IsRead = true,
                    SenderId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    RecipientId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    TripId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    MessageType = MessageType.ChatMsg
                }
            );
        }
    }
}