using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

internal class DriverBlockedPassengerConfig : IEntityTypeConfiguration<DriverBlockedPassenger>
{
    public void Configure(EntityTypeBuilder<DriverBlockedPassenger> builder)
    {
        builder.ToTable("DriverBlockedPassengers", "Driver");

        builder.HasKey(dbp => dbp.Id);

        builder.Property(dbp => dbp.DriverId)
            .IsRequired();

        builder.Property(dbp => dbp.PassengerId)
            .IsRequired();

        builder.Property(dbp => dbp.Reason)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(dbp => dbp.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        // Relationships
        builder.HasOne(dbp => dbp.Driver)
            .WithMany()
            .HasForeignKey(dbp => dbp.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(dbp => dbp.Passenger)
            .WithMany()
            .HasForeignKey(dbp => dbp.PassengerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Create unique index to prevent duplicate blocking entries
        builder.HasIndex(dbp => new { dbp.DriverId, dbp.PassengerId })
            .IsUnique()
            .HasDatabaseName("IX_DriverBlockedPassengers_DriverId_PassengerId");
    }
}

