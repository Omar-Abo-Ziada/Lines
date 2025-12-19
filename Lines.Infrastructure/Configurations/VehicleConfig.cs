using Lines.Domain.Enums;
using Lines.Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

internal class VehicleConfig : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles", "Vehicle");

        builder.HasKey(v => v.Id);

        //builder.Property(v => v.DriverId)
        //    .IsRequired();

        builder.Property(v => v.VehicleTypeId)
            .IsRequired();

        builder.Property(v => v.KmPrice)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(v => v.RegistrationDocumentUrls)
            .HasColumnType("nvarchar(max)");

        builder.Property(v => v.IsPrimary)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(v => v.IsActive)
            .IsRequired()
            .HasDefaultValue(true);



        builder.Property(v => v.Model)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Year)
            .IsRequired();

        builder.Property(v => v.LicensePlate)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(v => v.IsVerified)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(v => v.LicenseId)
            .IsRequired();

        builder.Property(a => a.CreatedDate).IsRequired()
          .HasDefaultValueSql("GETDATE()");

        builder.Property(v => v.Status)
            .HasDefaultValue(VehicleRequestStatus.PendingVerification)
            .IsRequired();

        builder.HasOne(v => v.Driver)
            .WithMany(d => d.Vehicles)
            .HasForeignKey(v => v.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(v => v.VehicleType)
            .WithMany(vt => vt.Vehicles)
            .HasForeignKey(v => v.VehicleTypeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(v => v.License)
            .WithOne(l => l.Vehicle)
            .HasForeignKey<Vehicle>(v => v.LicenseId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(v => v.Photos)
            .WithOne(p => p.Vehicle)
            .HasForeignKey(p => p.VehicleId)
            .OnDelete(DeleteBehavior.NoAction);

        // Data seeding (example)
        builder.HasData(
            new Vehicle
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                //DriverId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                VehicleTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Model = "Toyota Prius",
                Year = 2020,
                LicensePlate = "ABC123",
                IsVerified = true,
                LicenseId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Status = VehicleRequestStatus.Approved,
                KmPrice = 0.5m,
                IsPrimary = true,
                IsActive = true
            },
            new Vehicle
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                //DriverId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                VehicleTypeId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Model = "Honda Civic",
                Year = 2019,
                LicensePlate = "XYZ789",
                IsVerified = false,
                LicenseId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Status = VehicleRequestStatus.PendingVerification,
                KmPrice = 0.8m,
                IsPrimary = false,
                IsActive = true
            }
        );
    }
}