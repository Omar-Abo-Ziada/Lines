using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class DriverServiceFeeOfferConfig : IEntityTypeConfiguration<DriverServiceFeeOffer>
    {
        public void Configure(EntityTypeBuilder<DriverServiceFeeOffer> builder)
        {
            builder.ToTable("DriverServiceFeeOffers", "Driver");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.DriverId)
                .IsRequired();

            builder.Property(o => o.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(o => o.Description)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(o => o.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(o => o.DurationDays)
                .IsRequired();

            builder.Property(o => o.ServiceFeePercent)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(o => o.ValidFrom)
                .IsRequired();

            builder.Property(o => o.ValidUntil)
                .IsRequired();

            builder.Property(o => o.IsGloballyActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(o => o.CreatedDate)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.HasOne(o => o.Driver)
                .WithMany(d => d.ServiceFeeOffers)
                .HasForeignKey(o => o.DriverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.Activations)
                .WithOne(a => a.Offer)
                .HasForeignKey(a => a.OfferId)
                .OnDelete(DeleteBehavior.NoAction);

            // Seed Data
            var now = DateTime.UtcNow;
            builder.HasData(
                new DriverServiceFeeOffer
                {
                    Id = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    DriverId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Title = "Service Fee Cap: 7%",
                    Description = "Don't miss out — activate now and maximize every trip!",
                    Price = 5.00m,
                    DurationDays = 3,
                    ServiceFeePercent = 7.0m,
                    ValidFrom = now.AddDays(-1),
                    ValidUntil = now.AddDays(30),
                    IsGloballyActive = true
                },
                new DriverServiceFeeOffer
                {
                    Id = Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"),
                    DriverId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Title = "Service Fee Cap: 5%",
                    Description = "Premium offer! Reduce service fee to only 5% for a week.",
                    Price = 10.00m,
                    DurationDays = 7,
                    ServiceFeePercent = 5.0m,
                    ValidFrom = now.AddDays(-1),
                    ValidUntil = now.AddDays(30),
                    IsGloballyActive = true
                },
                new DriverServiceFeeOffer
                {
                    Id = Guid.Parse("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    DriverId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Title = "Service Fee Cap: 3%",
                    Description = "Best offer! Maximum earnings with only 3% service fee for 2 weeks.",
                    Price = 20.00m,
                    DurationDays = 14,
                    ServiceFeePercent = 3.0m,
                    ValidFrom = now.AddDays(-1),
                    ValidUntil = now.AddDays(30),
                    IsGloballyActive = true
                }
            );
        }
    }
}


