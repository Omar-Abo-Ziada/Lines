using Lines.Domain.Models;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class OfferConfig : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.ToTable("Offers", "Driver");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.TimeToArriveInMinutes)
                .IsRequired();

            builder.Property(o => o.DistanceToArriveInMeters)
                .IsRequired();

            builder.Property(o => o.Price)
                .IsRequired();

            builder.Property(o => o.DriverId)
                .IsRequired();

            builder.Property(o => o.IsAccepted)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(o => o.CreatedDate)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.HasOne(o => o.Driver)
                .WithMany(d => d.Offers)
                .HasForeignKey(o => o.DriverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.TripRequest)
              .WithMany(t => t.Offers)
              .HasForeignKey(o => o.TripRequestId)
              .OnDelete(DeleteBehavior.NoAction);

            // Data seeding (example)
            builder.HasData(
                new Offer

                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    TimeToArriveInMinutes = 5,
                    DistanceToArriveInMeters = 1200.5f,
                    Price = 25.0m,
                    DriverId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    TripRequestId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    IsAccepted = false
                },
                new Offer
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),

                    TimeToArriveInMinutes = 3,
                    DistanceToArriveInMeters = 800.0f,
                    Price = 18.5m,
                    DriverId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    TripRequestId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    IsAccepted = true
                }
            );
        }
    }
}