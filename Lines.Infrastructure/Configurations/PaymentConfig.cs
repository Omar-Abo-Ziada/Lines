using Lines.Domain.Models;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments", "Trip");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.TripId)
                .IsRequired();

            builder.Property(p => p.PaymentMethodId)
                .IsRequired();

            builder.Property(p => p.Amount)
                .IsRequired();

            builder.Property(p => p.PaidAt)
                    .HasDefaultValueSql("GETDATE()")
                    .IsRequired();

            builder.Property(a => a.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.EarningId);


            builder.Property(p => p.TransactionReference)
                .IsRequired()
                .HasMaxLength(100);


            // relations

            builder.HasOne(p => p.Trip)
                .WithOne(t => t.Payment)
                .HasForeignKey<Payment>(p => p.TripId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.PaymentMethod)
                .WithMany(pm => pm.Payments)
                .HasForeignKey(p => p.PaymentMethodId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Earning)
                .WithOne(e => e.Payment)
                .HasForeignKey<Payment>(p => p.EarningId)
                .OnDelete(DeleteBehavior.NoAction);

            // Data seeding (example)
            // builder.HasData(
            //     new Payment
            //     {
            //         Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            //         TripId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            //         PaymentMethodId = ,
            //         EarningId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            //         Amount = 50.00m,
            //         TransactionReference = "TXN123456"
            //     },
            //     new Payment
            //     {
            //         Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            //         TripId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            //         PaymentMethodId = 2,
            //         EarningId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            //         Amount = 75.50m,
            //         TransactionReference = "TXN654321"
            //     }
        // );
        }
    }
}