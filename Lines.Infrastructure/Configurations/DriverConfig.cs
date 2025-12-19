using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

internal class DriverConfig : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("Drivers", "Driver");

        builder.Property(d => d.IsAvailable)
            .IsRequired();

        builder.Property(d => d.IsNotifiedForOnlyTripsAboveMyPrice)
               .IsRequired();


        builder.Property(d => d.Rating)
            .IsRequired();

        builder.Property(d => d.TotalTrips)
            .IsRequired();

        builder.OwnsOne(d => d.Email, UserConfigurationHelper.ConfigureEmail);
        builder.OwnsOne(d => d.PhoneNumber, UserConfigurationHelper.ConfigurePhoneNumber);


        builder.Property(a => a.CreatedDate).IsRequired()
          .HasDefaultValueSql("GETDATE()");

        // Relationships
        // All licenses including history (one-to-many)
        // Current license is determined by IsCurrent flag on DriverLicense
        builder.HasMany(d => d.Licenses)
            .WithOne(l => l.Driver)
            .HasForeignKey(l => l.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Offers)
            .WithOne(o => o.Driver)
            .HasForeignKey(o => o.DriverId);

        builder.HasMany(d => d.Earnings)
       .WithOne(e => e.Driver)
       .HasForeignKey(e => e.DriverId);

        builder.OwnsOne(t => t.Location, loc =>
        {
            loc.Property(l => l.Latitude).HasColumnName("Start_Latitude");
            loc.Property(l => l.Longitude).HasColumnName("Start_Longitude");
            loc.Property(l => l.Address).HasColumnName("Start_Address");

            // Seed data for owned entity - use the correct naming convention
            loc.HasData(
                 new
                 {
                     DriverId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                     Latitude = 1.1,
                     Longitude = 1.1,
                     Address = "Start Location 1"
                 },
                 new
                 {
                     DriverId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                     Latitude = 2.2,
                     Longitude = 2.2,
                     Address = "Start Location 2"
                 }
             );

        });

        // Data seeding
        //builder.HasData(
        //    new
        //    {
        //        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        //        IsAvailable = true,
        //        Rating = 4.8,
        //        TotalTrips = 120,
        //        DriverLicenseId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        //        FirstName = "Mohamed",
        //        LastName = "Driver",
        //        IsDeleted = false,
        //        IsNotifiedForOnlyTripsAboveMyPrice = true,
        //        VehicleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        //        Points = 50,
        //        RatedTripsCount = 2
        //    },
        //    new
        //    {
        //        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        //        IsAvailable = false,
        //        Rating = 4.5,
        //        TotalTrips = 75,
        //        DriverLicenseId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        //        FirstName = "Khaled",
        //        LastName = "Driver",
        //        IsDeleted = false,
        //        IsNotifiedForOnlyTripsAboveMyPrice = false,
        //        VehicleId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        //        Points = 60,
        //        RatedTripsCount = 3
        //    }
        //);



        builder.OwnsOne(p => p.Email)
            .HasData(new
            {
                DriverId = Guid.Parse("11111111-1111-1111-1111-111111111111"),  // FK to driver entity
                Value = "mohamed.driver@example.com"
            });

        builder.OwnsOne(p => p.PhoneNumber)

            .HasData(new
            {
                DriverId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Value = "01234567890"
            });


        builder.OwnsOne(p => p.Email)

            .HasData(new
            {
                DriverId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Value = "khaled.driver@example.com"
            });

        builder.OwnsOne(p => p.PhoneNumber)

            .HasData(new
            {
                DriverId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Value = "01234567891"
            });
    }
}