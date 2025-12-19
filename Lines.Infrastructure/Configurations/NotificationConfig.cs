using Lines.Domain.Enums;
using Lines.Domain.Models;
using Lines.Domain.Models.Users;
using Lines.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class NotificationConfig : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications", "User");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.UserId)
                .IsRequired();

            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(n => n.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(n => n.IsRead)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(n => n.NotificationType)  
                .IsRequired();

            builder.HasOne<ApplicationUser>()
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Data seeding (example)
            builder.HasData(
                new Notification
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Title = "Trip Confirmed",
                    Message = "Your trip has been confirmed by the driver.",
                    IsRead = false,
                    NotificationType = NotificationType.TripRequestAccepted
                },
                new Notification
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Title = "Payment Received",
                    Message = "Your payment for the trip has been received.",
                    IsRead = true,
                    NotificationType = NotificationType.PaymentReceived
                }
            );
        }
    }
}