using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

public class DriverAddressConfiguration : IEntityTypeConfiguration<DriverAddress>
{
    public void Configure(EntityTypeBuilder<DriverAddress> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(500);
            
        builder.Property(x => x.PostalCode)
            .IsRequired()
            .HasMaxLength(10);
            
        builder.Property(x => x.LimousineBadgeUrl)
            .HasMaxLength(500);
            
        builder.Property(x => x.IsPrimary)
            .IsRequired()
            .HasDefaultValue(true);
        
        // Relationships
        builder.HasOne(x => x.Driver)
            .WithMany(d => d.Addresses)
            .HasForeignKey(x => x.DriverId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(x => x.Sector)
            .WithMany()
            .HasForeignKey(x => x.SectorId)
            .OnDelete(DeleteBehavior.SetNull);
            
        // Indexes
        builder.HasIndex(x => x.DriverId);
        builder.HasIndex(x => x.CityId);
        builder.HasIndex(x => x.SectorId);
        builder.HasIndex(x => new { x.DriverId, x.IsPrimary });
    }
}
