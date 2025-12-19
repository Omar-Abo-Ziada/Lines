using Lines.Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class UserRewardConfig : IEntityTypeConfiguration<UserReward>
    {
        public void Configure(EntityTypeBuilder<UserReward> builder)
        {
            builder.ToTable("UserRewards", "User");

            builder.Property(ur => ur.IsUsed)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.HasOne(ur => ur.Reward)
                   .WithMany(r => r.UserRewards)
                   .HasForeignKey(ur => ur.RewardId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ur => ur.TripRequest)
                   .WithOne(tr => tr.UserReward)
                   .HasForeignKey<UserReward>(ur => ur.TripRequestId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Property(r => r.CreatedDate)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            // Seed sample user rewards
            builder.HasData(
                new
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"), 
                    RewardId = Guid.Parse("11111111-1111-1111-1111-111111111111"), 
                    RedeemedAt = DateTime.UtcNow,
                    IsUsed = false,
                    IsDeleted = false
                },
                new
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    UserId = Guid.Parse("44444444-4444-4444-4444-444444444444"), 
                    RewardId = Guid.Parse("22222222-2222-2222-2222-222222222222"), 
                    RedeemedAt = DateTime.UtcNow,
                    IsUsed = true,
                    UsedAt = DateTime.UtcNow.AddDays(-3),
                    IsDeleted = false
                }
            );
        }
    }
}
