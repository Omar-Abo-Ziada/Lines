using Lines.Domain.Enums;
using Lines.Domain.Models.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    public class SupportMessageConfig : IEntityTypeConfiguration<SupportMsg>
    {
        public void Configure(EntityTypeBuilder<SupportMsg> builder)
        {

            // Data seeding example
            builder.HasData(
                new SupportMsg
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Content = "I have an issue the driver did not arrive till now.",
                    IsRead = true,
                    SenderId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    RecipientId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    MessageType = MessageType.SupportMsg
                },
                new SupportMsg
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Content = "I have an issue that I did not receive my earning yet.",
                    IsRead = true,
                    SenderId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    RecipientId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    MessageType = MessageType.SupportMsg
                }
            );
        }
    }
}
