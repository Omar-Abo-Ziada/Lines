using Lines.Domain.Enums;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

internal class TripRequestConfig : IEntityTypeConfiguration<TripRequest>
{
    public void Configure(EntityTypeBuilder<TripRequest> builder)
    {
        builder.ToTable("TripRequests", "Trip");

        builder.HasKey(tr => tr.Id);

        builder.Property(a => a.CreatedDate).IsRequired()
                  .HasDefaultValueSql("GETDATE()");

        builder.Property(tr => tr.IsScheduled)
          .HasDefaultValue(false)
          .IsRequired();

        builder.Property(tr => tr.ScheduledAt);

        builder.Property(tr => tr.Distance)
            .IsRequired();


        builder.Property(tr => tr.RequestedAt)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");


        builder.Property(tr => tr.Status)
            .HasDefaultValue(TripRequestStatus.Pending)
            .IsRequired();

        builder.Property(tr => tr.EstimatedPrice)
            .IsRequired();

        builder.Property(tr => tr.CancellationReason)
               .HasMaxLength(500);

        builder.Property(tr => tr.PassengerId)
            .IsRequired();

        builder.Property(tr => tr.DriverId);

        builder.Property(tr => tr.VehicleTypeId)
               .IsRequired();

        builder.Property(tr => tr.UserRewardId);

        //builder.Ignore(t => t.Distance);  // mark it as not mapped as it is "computed prop" calculated in run time >> no need to be stored in db


        builder.OwnsOne(t => t.StartLocation, loc =>
        {
            loc.Property(l => l.Latitude).HasColumnName("Start_Latitude");
            loc.Property(l => l.Longitude).HasColumnName("Start_Longitude");
            loc.Property(l => l.Address).HasColumnName("Start_Address");

            // Seed data for owned entity - use the correct naming convention
            loc.HasData(
                new
                {
                    // The property name should match the owner's primary key property name + "Id"
                    TripRequestId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Latitude = 1.1,  // Remove the prefix - EF will map it correctly
                    Longitude = 1.1,
                    Address = "Start Location 1"
                },
                new
                {
                    TripRequestId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Latitude = 2.2,
                    Longitude = 2.2,
                    Address = "Start Location 2"
                }
            );
        });

        builder.HasOne(tr => tr.Passenger)
            .WithMany(p => p.TripRequests)
            .HasForeignKey(tr => tr.PassengerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(tr => tr.Driver)
            .WithMany(d => d.TripRequests)
            .HasForeignKey(tr => tr.DriverId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(tr => tr.EndLocations)
            .WithOne(e => e.TripRequest)
            .HasForeignKey(e => e.TripRequestId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(tr => tr.Offers)
            .WithOne(o => o.TripRequest)
            .HasForeignKey(o => o.TripRequestId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(tr => tr.VehicleType)
               .WithMany(vt => vt.TripRequests)
               .HasForeignKey(tr => tr.VehicleTypeId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(tr => tr.UserReward)
               .WithOne(r => r.TripRequest)
               .HasForeignKey<TripRequest>(tr => tr.UserRewardId)
               .OnDelete(DeleteBehavior.NoAction);

        //Data seeding (example)
        builder.HasData(
            new TripRequest
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                PassengerId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Status = TripRequestStatus.Pending,
                EstimatedPrice = 30.00m,
                DriverId = null,
                PaymentMethodId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                VehicleTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            },
            new TripRequest
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                PassengerId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Status = TripRequestStatus.Accepted,
                EstimatedPrice = 45.50m,
                DriverId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                PaymentMethodId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                VehicleTypeId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            }
        );
    }
}