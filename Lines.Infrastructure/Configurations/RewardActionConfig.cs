using Lines.Domain.Enums;
using Lines.Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class RewardActionConfig : IEntityTypeConfiguration<RewardAction>
    {
        public void Configure(EntityTypeBuilder<RewardAction> builder)
        {
            builder.ToTable("RewardActions", "User");

            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(r => r.Points)
                   .IsRequired();

            builder.Property(r => r.Type)
                   .IsRequired();

            builder.Property(r => r.CreatedDate)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            // Seed some example data
            builder.HasData(
                new
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Complete 5 Trips",
                    Points = 50,
                    IsDeleted = false,
                    Type = RewardActionType.FiveRidesBonus
                },
                new
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Invite a Friend",
                    Points = 100,
                    IsDeleted = false,
                    Type = RewardActionType.ReferFriend
                },
                new
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Complete Profile",
                    Points = 15,
                    IsDeleted = false,
                    Type = RewardActionType.CompleteProfile
                },
                new
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Visit app daily for one week",
                    Points = 40,
                    IsDeleted = false,
                    Type = RewardActionType.DailyVisitWeek
                },
                 new
                 {
                     Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                     Name = "Leave Tips for the driver",
                     Points = 5,
                     IsDeleted = false,
                     Type = RewardActionType.LeaveTip
                 }
            );
        }
    }
}
