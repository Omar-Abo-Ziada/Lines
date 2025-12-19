// ... existing code ...
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Lines.Domain.Models;
using Lines.Domain.Enums;
using Lines.Domain.Models.Drivers;

namespace Lines.Infrastructure.Configurations
{
    internal class DriverLicenseConfig : IEntityTypeConfiguration<DriverLicense>  
    {
        public void Configure(EntityTypeBuilder<DriverLicense> builder)
        {
            builder.Property(l => l.DriverId);

            builder.Property(x => x.IsCurrent)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Many-to-One relationship with Driver (for collection)
            builder.HasOne(x => x.Driver)
                .WithMany(d => d.Licenses)
                .HasForeignKey(x => x.DriverId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Data seeding example
            builder.HasData(
                new DriverLicense
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    LicenseNumber = "DR123456",
                    IssueDate = new DateTime(2022, 1, 1),
                    ExpiryDate = new DateTime(2027, 1, 1),
                    IsValid = true,
                    IsDeleted = false,
                    DriverId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    licenseType = LicenseType.DriverLicense,
                    IsCurrent = true,
                    IsActive = true
                },
                new DriverLicense
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    LicenseNumber = "DR7891012",
                    IssueDate = new DateTime(2024, 1, 1),
                    ExpiryDate = new DateTime(2029, 1, 1),
                    IsValid = true,
                    IsDeleted = false,
                    DriverId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    licenseType = LicenseType.DriverLicense,
                    IsCurrent = true,
                    IsActive = true
                }
            );
        }
    }
}
// ... existing code ...