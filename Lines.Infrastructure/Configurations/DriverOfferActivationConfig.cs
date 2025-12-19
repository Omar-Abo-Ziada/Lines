using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

internal class DriverOfferActivationConfig : IEntityTypeConfiguration<DriverOfferActivation>
{
    public void Configure(EntityTypeBuilder<DriverOfferActivation> builder)
    {
        builder.ToTable("DriverOfferActivations", "Driver");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.DriverId)
            .IsRequired();

        builder.Property(a => a.OfferId)
            .IsRequired();

        builder.Property(a => a.ActivationDate)
            .IsRequired();

        builder.Property(a => a.ExpirationDate)
            .IsRequired();

        builder.Property(a => a.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(a => a.PaymentReference)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.CreatedDate)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        // Relationships
        builder.HasOne(a => a.Driver)
            .WithMany(d => d.OfferActivations)
            .HasForeignKey(a => a.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Offer)
            .WithMany(o => o.Activations)
            .HasForeignKey(a => a.OfferId)
            .OnDelete(DeleteBehavior.NoAction);

        // Index for efficient queries
        builder.HasIndex(a => new { a.DriverId, a.IsActive, a.ExpirationDate });
    }
}

