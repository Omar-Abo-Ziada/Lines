using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

public class DriverRegistrationConfiguration : IEntityTypeConfiguration<DriverRegistration>
{
    public void Configure(EntityTypeBuilder<DriverRegistration> builder)
    {
        builder.ToTable("DriverRegistrations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RegistrationToken)
            .IsRequired()
            .HasMaxLength(36); // GUID length

        builder.Property(x => x.Email)
            .IsRequired(false)  // Make nullable until Step 2
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>();

        // Step 1: Personal Info
        builder.Property(x => x.PersonalPictureUrl)
            .HasMaxLength(500);

        builder.Property(x => x.FirstName)
            .HasMaxLength(50);

        builder.Property(x => x.LastName)
            .HasMaxLength(50);

        builder.Property(x => x.CompanyName)
            .HasMaxLength(100);

        builder.Property(x => x.CommercialRegistration)
            .HasMaxLength(20);

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(255);

        builder.Property(x => x.IdentityType)
            .HasConversion<string>();

        // Step 2: Contact Info
        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20);


        // Step 3: Address
        builder.Property(x => x.SectorId);

        builder.Property(x => x.Address)
            .HasMaxLength(500);

        builder.Property(x => x.PostalCode)
            .HasMaxLength(10);

        builder.Property(x => x.LimousineBadgeUrl)
            .HasMaxLength(500);

        // JSON columns for complex data
        builder.Property(x => x.LicenseData)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.VehicleData)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.BankAccountData)
            .HasColumnType("nvarchar(max)");

        // Indexes
        builder.HasIndex(x => x.RegistrationToken)
            .IsUnique()
            .HasDatabaseName("IX_DriverRegistrations_RegistrationToken");

        builder.HasIndex(x => x.Email)
            .HasDatabaseName("IX_DriverRegistrations_Email");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_DriverRegistrations_Status");

        builder.HasIndex(x => x.CreatedDate)
            .HasDatabaseName("IX_DriverRegistrations_CreatedDate");
    }
}
