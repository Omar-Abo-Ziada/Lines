using Lines.Domain.Models.User;
using Lines.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class OtpConfig : IEntityTypeConfiguration<Otp>
    {
        public void Configure(EntityTypeBuilder<Otp> builder)
        {
            builder.ToTable("Otp", "User");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Code)
                .IsRequired()
                .HasMaxLength(6);

            builder.Property(o => o.OTPGenerationTime)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");  

            builder.HasOne<ApplicationUser>()
                .WithMany(u => u.Otps)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            // Data seeding
            builder.HasData(
                   new Otp {
                       Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                       Code = "123456",
                       UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                       OTPGenerationTime = new DateTime(2024, 1, 1, 9, 0, 0),
                   },
                   new Otp
                   {
                       Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                       Code = "654321",
                       UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                       OTPGenerationTime = new DateTime(2024, 1, 1, 9, 0, 0)
                   }
                );
        }
    }
}
