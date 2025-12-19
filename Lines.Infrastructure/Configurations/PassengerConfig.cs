using Lines.Domain.Models.Passengers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Lines.Infrastructure.Configurations
{
    internal class PassengerConfig : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.ToTable("Passengers", "Passenger");

            builder.Property(p => p.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.ReferralCode)
                   .IsRequired();

            builder.HasIndex(p => p.ReferralCode)
                   .IsUnique();

            builder.Property(p => p.isReferralCodeSubmitted)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(p => p.ConsecutiveActiveDays)
                   .IsRequired()
                   .HasDefaultValue(0);


            builder.OwnsOne(p => p.Email, UserConfigurationHelper.ConfigureEmail);
            builder.OwnsOne(p => p.PhoneNumber, UserConfigurationHelper.ConfigurePhoneNumber);

            builder.HasMany(p => p.TripRequests)
                .WithOne(tr => tr.Passenger)
                .HasForeignKey(tr => tr.PassengerId)       
                .OnDelete(DeleteBehavior.NoAction);

            // Seed main entity data
            builder.HasData(
                new
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    FirstName = "Ahmed",
                    LastName = "Passenger",
                    IsDeleted = false,
                    TotalTrips = 3,
                    Rating = 4.8,
                    RatedTripsCount = 0,
                    Points = 100,
                    ReferralCode = "REF12345"
                },
                new
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    FirstName = "Mostafa",
                    LastName = "Passenger",
                    IsDeleted = false,
                    TotalTrips = 5,
                    Rating = 4.8,
                    RatedTripsCount = 1,
                    Points = 60,
                    ReferralCode = "REF67890"
                }
            );

            // Seed owned entity data separately
            builder.OwnsOne(p => p.Email).HasData(
                new
                {
                    PassengerId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Value = "ahmed.passenger@example.com"
                },
                new
                {
                    PassengerId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Value = "mostafa.passenger@example.com"
                }
            );

            builder.OwnsOne(p => p.PhoneNumber).HasData(
                new
                {
                    PassengerId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Value = "01234567890"
                },
                new
                {
                    PassengerId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Value = "01534567890"
                }
            );
        }
    }
}