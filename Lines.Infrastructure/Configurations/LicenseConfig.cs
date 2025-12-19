using Lines.Domain.Models;
using Lines.Domain.Models.Drivers;
using Lines.Domain.Models.License;
using Lines.Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class LicenseConfig : IEntityTypeConfiguration<License>
    {
        public void Configure(EntityTypeBuilder<License> builder)  
        {
            builder.ToTable("Licenses", "Common").HasDiscriminator(l => l.licenseType)
             .HasValue<DriverLicense>(Domain.Enums.LicenseType.DriverLicense)
             .HasValue<VehicleLicense>(Domain.Enums.LicenseType.VehicleLicense);

            builder.HasKey(l => l.Id);

            builder.Property(l => l.LicenseNumber)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(l => l.IssueDate)
                    .IsRequired();

            builder.Property(l => l.ExpiryDate)
                .IsRequired();

            builder.Property(l => l.IsValid)
                .HasDefaultValue(false)   // till the admin validate it
                .IsRequired();

            builder.Property(l => l.IsDeleted)
                 .HasDefaultValue(false)
                .IsRequired();

            builder.Property(l => l.licenseType)
                .IsRequired();

            builder.Property(a => a.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");


            //builder.HasOne(l => l.Driver)
            //    .WithOne(d => d.DriverLicense)
            //    .HasForeignKey<LicenseEntity>(l => l.DriverId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(l => l.Vehicle)
            //    .WithOne(v => v.License)
            //    .HasForeignKey<LicenseEntity>(l => l.VehicleId)
            //    .OnDelete(DeleteBehavior.NoAction);

            // Data seeding   >> done using the derived classes as this is abstract class 
            //builder.HasData(
            //    new License
            //    {
            //        Id = 1,
            //        LicenseNumber = "DR123456",
            //        IssueDate = new DateTime(2022, 1, 1),
            //        ExpiryDate = new DateTime(2027, 1, 1),
            //        IsValid = true,
            //        IsDeleted = false,
            //        LicenseType = LicenseType.DriverLicense,
            //        DriverId = 1,
            //        VehicleId = null
            //    },
            //    new License
            //    {
            //        Id = 2,
            //        LicenseNumber = "VH654321",
            //        IssueDate = new DateTime(2023, 6, 15),
            //        ExpiryDate = new DateTime(2028, 6, 15),
            //        IsValid = true,
            //        IsDeleted = false,
            //        LicenseType = LicenseType.VehicleLicense,
            //        DriverId = null,
            //        VehicleId = 1
            //    }
            //);
        }
    }
}