using Lines.Domain.Models.Trips;
using Lines.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class FeedbackConfig : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedbacks", "Trip");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.TripId).IsRequired();
            builder.Property(f => f.FromUserId).IsRequired();
            builder.Property(f => f.ToUserId).IsRequired();
            builder.Property(f => f.Rating).IsRequired();
            builder.Property(f => f.Comment).HasMaxLength(1000);
            builder.Property(f => f.CreatedDate).IsRequired().HasDefaultValueSql("GETDATE()");

            builder.HasOne(f => f.Trip)
                .WithMany(t => t.Feedbacks)
                .HasForeignKey(f => f.TripId)
                .OnDelete(DeleteBehavior.NoAction);    

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(f => f.FromUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(f => f.ToUserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Data seeding
            builder.HasData(
                new Feedback
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    TripId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    FromUserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    ToUserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Rating = 5,
                    Comment = "Great trip, very friendly driver!"
                },
                new Feedback
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    TripId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    FromUserId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    ToUserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Rating = 4,
                    Comment = "Smooth ride, but arrived a bit late."
                }
            );
        }
    }
}