using Lines.Domain.Models.Chats;
using Lines.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {

            builder.ToTable("Messages", "Chat")
                    .HasDiscriminator(m => m.MessageType)
                    .HasValue<ChatMessage>(Domain.Enums.MessageType.ChatMsg)
                    .HasValue<SupportMsg>(Domain.Enums.MessageType.SupportMsg);

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Content)
                .HasMaxLength(2000);

            builder.Property(m => m.ImagePath)
                .HasMaxLength(500);

            builder.Property(a => a.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");

            builder.Property(m => m.ReadAt)
                   .HasDefaultValue(new DateTime(2024, 1, 1, 8, 30, 0));
            

            builder.Property(m => m.IsRead)
                .HasDefaultValue(false);

            builder.Property(m => m.MessageType)
                .IsRequired();

            builder.Property(m => m.SenderId)
                .IsRequired();

            builder.Property(m => m.RecipientId)
                .IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithMany(u => u.MessagesSent)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<ApplicationUser>()
                .WithMany(u => u.MessagesRecieved)
                .HasForeignKey(m => m.RecipientId)
                .OnDelete(DeleteBehavior.NoAction);


            // Data seeding (example)
            //builder.HasData(
            //    new Message
            //    {
            //        Id = 1,
            //        Content = "Hello, your trip is confirmed!",
            //        SentAt = new DateTime(2024, 1, 1, 9, 0, 0),
            //        ReadAt = null,
            //        MessageType = MessageType.ChatMsg,
            //        SenderId = 1,
            //        RecipientId = 3,
            //        TripId = 1
            //    },
            //    new Message
            //    {
            //        Id = 2,
            //        Content = "Thank you! See you soon.",
            //        SentAt = new DateTime(2024, 1, 1, 9, 5, 0),
            //        ReadAt = new DateTime(2024, 1, 1, 9, 6, 0),
            //        MessageType = MessageType.ChatMsg,
            //        SenderId = 3,
            //        RecipientId = 1,
            //        TripId = 1
            //    }
            //);
        }
    }
}