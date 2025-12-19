using Lines.Domain.Enums; // Adjust namespace as needed
using Lines.Domain.Models;
using Lines.Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    public class VehicleLicenseConfig : IEntityTypeConfiguration<VehicleLicense>
    {
        public void Configure(EntityTypeBuilder<VehicleLicense> builder)
        {
           
            builder.Property(vl => vl.VehicleId)
           .IsRequired();


            builder.HasOne(vl => vl.Vehicle)
             .WithOne(v => v.License)
             .HasForeignKey<VehicleLicense>(vl => vl.VehicleId)
             .OnDelete(DeleteBehavior.NoAction);

            // Add more property configurations as needed

            // Seeding initial data
            builder.HasData(
                new VehicleLicense {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    LicenseNumber = "ABC123",
                    IssueDate = new DateTime(2024, 1, 1, 8, 0, 0), 
                    ExpiryDate = new DateTime(2025, 12, 31),
                    IsDeleted = false,
                    IsValid = false,
                    VehicleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    licenseType = LicenseType.VehicleLicense
                },
                new VehicleLicense { 
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    LicenseNumber = "XYZ789", 
                    IssueDate = new DateTime(2024, 1, 1, 8, 0, 0),
                    ExpiryDate = new DateTime(2026, 6, 30),
                    IsDeleted = false,
                    IsValid = true,
                    VehicleId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    licenseType = LicenseType.VehicleLicense
                }
            );
        }
    }
}