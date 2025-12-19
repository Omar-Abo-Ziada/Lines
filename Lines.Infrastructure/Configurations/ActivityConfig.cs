// ... existing code ...
using Lines.Domain.Models.Users;
using Lines.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class ActivityConfig : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.ToTable("Activities", "User");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedDate).IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(a => a.Day).IsRequired()
                   .HasDefaultValueSql("CAST(GETDATE() AS DATE)");

            builder.HasOne<ApplicationUser>()
                   .WithMany(u => u.Activities)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(a => new { a.UserId, a.Day })
                   .IsUnique()
                   .HasDatabaseName("UX_Activities_UserId_Day");

            // Data seeding
            builder.HasData(
                   new Activity
                   {
                       Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                       UserId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                   },
                   new Activity
                   {
                       Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                       UserId = Guid.Parse("22222222-2222-2222-2222-222222222222")
                   }
               );
        }
    }
}
