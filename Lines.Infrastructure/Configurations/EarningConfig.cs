using Lines.Domain.Models;
using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class EarningConfig : IEntityTypeConfiguration<Earning>
    {
        public void Configure(EntityTypeBuilder<Earning> builder)
        {
            builder.ToTable("Earnings", "Driver");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Amount)
                .IsRequired();

            builder.Property(e => e.IsPaid)
                .IsRequired();

            builder.Property(e => e.PaidAt)
                    .HasDefaultValueSql("GETDATE()");

            builder.Property(a => a.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");

            //builder.Property(e => e.TripId)
            //    .IsRequired();


            builder.Property(e => e.DriverId)
                .IsRequired();

            builder.Property(e => e.PaymentId)
                .IsRequired();

            // Relationships
            builder.HasOne(e => e.Driver)
                .WithMany(d => d.Earnings)
                .HasForeignKey(e => e.DriverId)
                .OnDelete(DeleteBehavior.NoAction);  // prevent deletion of parent if children exists

            builder.HasOne(e => e.Payment)
                .WithOne(p => p.Earning)
                .HasForeignKey<Earning>(e => e.PaymentId)
                .OnDelete(DeleteBehavior.NoAction);

            // Data seeding
            builder.HasData(
                new Earning
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    DriverId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    PaymentId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Amount = 85.50m,
                    IsPaid = true
                },
                new Earning
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    DriverId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    PaymentId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Amount = 120.00m,
                    IsPaid = false
                }
            );
        }
    }
}