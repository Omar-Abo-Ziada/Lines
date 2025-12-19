using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

internal class DriverEmergencyContactConfig : IEntityTypeConfiguration<DriverEmergencyContact>
{
    public void Configure(EntityTypeBuilder<DriverEmergencyContact> builder)
    {
        builder.ToTable("DriverEmergencyContacts", "Driver");

        builder.HasKey(dec => dec.Id);

        builder.Property(dec => dec.DriverId)
            .IsRequired();

        builder.Property(dec => dec.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(dec => dec.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(dec => dec.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        // Relationships
        builder.HasOne(dec => dec.Driver)
            .WithMany()
            .HasForeignKey(dec => dec.DriverId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

