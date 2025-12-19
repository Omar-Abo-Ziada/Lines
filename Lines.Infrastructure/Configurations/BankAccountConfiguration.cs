using Lines.Domain.Models.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.BankName)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(b => b.IBAN)
            .IsRequired()
            .HasMaxLength(34);
        
        // Make IBAN unique
        builder.HasIndex(b => b.IBAN)
            .IsUnique();
            
        builder.Property(b => b.SWIFT)
            .IsRequired()
            .HasMaxLength(11);
            
        builder.Property(b => b.AccountHolderName)
            .IsRequired()
            .HasMaxLength(100);
        
        // New optional fields
        builder.Property(b => b.AccountNumber)
            .HasMaxLength(50);
            
        builder.Property(b => b.BranchName)
            .HasMaxLength(100);
        
        builder.Property(b => b.IsPrimary)
            .IsRequired()
            .HasDefaultValue(false);
            
        builder.Property(b => b.CardNumber)
            .HasMaxLength(19); // Max length for masked card numbers (e.g., 1234 5678 9012 3456)
            
        builder.Property(b => b.ExpiryDate)
            .HasMaxLength(5); // MM/YY format
            
        builder.Property(b => b.CVV)
            .HasMaxLength(4); // 3 or 4 digits

        builder.HasOne(b => b.Driver)
            .WithMany(d => d.BankAccounts)
            .HasForeignKey(b => b.DriverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("BankAccounts");
    }
}
