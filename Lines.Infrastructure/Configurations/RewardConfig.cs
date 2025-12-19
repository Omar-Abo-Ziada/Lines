using Lines.Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class RewardConfig : IEntityTypeConfiguration<Reward>
    {
        public void Configure(EntityTypeBuilder<Reward> builder)
        {
            builder.ToTable("Rewards", "User");

            builder.Property(r => r.Title)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(r => r.Description)
                   .HasMaxLength(500);

            builder.Property(r => r.PointsRequired)
                   .IsRequired();

            builder.Property(r => r.DiscountPercentage)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(r => r.MaxValue)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.Property(r => r.CreatedDate)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasMany(r => r.UserRewards)
                   .WithOne(ur => ur.Reward)
                   .HasForeignKey(ur => ur.RewardId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Seed sample rewards
            builder.HasData(
                new
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Title = "15% Discount on Next Trip",
                    Description = "Redeem 100 points to get 15% off your next trip.",
                    PointsRequired = 100,
                    DiscountPercentage = 0.15m,
                    MaxValue = 12.00m,
                    IsDeleted = false
                },
                new
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Title = "Free Ride up to 50 EGP",
                    Description = "Redeem 300 points for a free ride up to 15 EGP.",
                    PointsRequired = 300,
                    DiscountPercentage = 1.00m,
                    MaxValue = 15.00m,
                    IsDeleted = false
                }
            );
        }
    }
}
