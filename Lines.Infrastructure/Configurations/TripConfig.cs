using Lines.Domain.Enums;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

internal class TripConfig : IEntityTypeConfiguration<Trip>
{
    public void Configure(EntityTypeBuilder<Trip> builder)
    {
        builder.ToTable("Trips", "Trip");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.DriverId).IsRequired();
        builder.Property(t => t.TripRequestId).IsRequired();
        builder.Property(t => t.PassengerId).IsRequired();
        builder.Property(t => t.Distance).IsRequired();
        builder.Property(t => t.Fare).IsRequired();
        builder.Property(t => t.Status).IsRequired().HasDefaultValue(TripStatus.InProgress);
        builder.Property(t => t.IsPaid).IsRequired().HasDefaultValue(false);
        builder.Property(t => t.PaymentMethodId).IsRequired();  // passenger must detect how he will pay first
        builder.Property(t => t.StartedAt);  // update it whenever the passenger enters the vehicle and start the trip
        builder.Property(t => t.IsRewardApplied).IsRequired().HasDefaultValue(false);

        builder.Property(tr => tr.EstimatedPickupTime)  // should be sent from frontend based on when the driver expected to arrive , if not sent set to now.
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

        builder.Property(a => a.CreatedDate).IsRequired()
          .HasDefaultValueSql("GETDATE()");

        builder.Property(t => t.EndedAt);

        builder.Property(t => t.PaymentId);
        //builder.Property(t => t.FeedbackId);

        builder.Ignore(t => t.Distance);  // mark it as not mapped as it is "computed prop" calculated in run time >> no need to be stored in db

        builder.HasOne(t => t.Driver)
            .WithMany(d => d.Trips)
            .HasForeignKey(t => t.DriverId)
            .OnDelete(DeleteBehavior.NoAction);

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
                    TripId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Latitude = 1.1,  // Remove the prefix - EF will map it correctly
                    Longitude = 1.1,
                    Address = "Start Location 1"
                },
                new
                {
                    TripId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Latitude = 2.2,
                    Longitude = 2.2,
                    Address = "Start Location 2"
                }
            );
        });

        builder.HasOne(t => t.Passenger)
            .WithMany(p => p.Trips)
            .HasForeignKey(t => t.PassengerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(t => t.TripRequest)
            .WithOne(tr => tr.Trip)
            .HasForeignKey<Trip>(t => t.TripRequestId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(t => t.Payment)
            .WithOne(p => p.Trip)
            .HasForeignKey<Trip>(t => t.PaymentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(t => t.TripRequest)
          .WithOne(tr => tr.Trip)
          .HasForeignKey<Trip>(t => t.TripRequestId)
          .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(t => t.Feedbacks)
            .WithOne(f => f.Trip)
            .HasForeignKey(f => f.TripId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(t => t.EndLocations)
            .WithOne(e => e.Trip)
            .HasForeignKey(e => e.TripId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(t => t.Messages)
            .WithOne(m => m.Trip)
            .HasForeignKey(m => m.TripId)
            .OnDelete(DeleteBehavior.NoAction);

            // Data seeding (example)
            builder.HasData(
                new Trip
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    TripRequestId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    DriverId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    PassengerId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Fare = 35.00m,
                    Status = TripStatus.Completed,
                    StartedAt = new DateTime(2024, 1, 1, 10, 0, 0),
                    EndedAt = new DateTime(2024, 1, 1, 10, 30, 0),
                    PaymentMethodType = PaymentMethodType.Cash,
                    PaymentMethodId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    IsPaid = false
                },
                new Trip
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    TripRequestId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    DriverId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    PassengerId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Fare = 50.00m,
                    Status = TripStatus.Completed,
                    StartedAt = new DateTime(2024, 1, 1, 10, 0, 0),
                    EndedAt = new DateTime(2024, 1, 1, 10, 30, 0),
                    PaymentMethodType = PaymentMethodType.card,
                    PaymentMethodId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    IsPaid = false
                }
            );
        }
    }

