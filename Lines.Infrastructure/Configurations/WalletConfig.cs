
using Lines.Domain.Models.Drivers;
using Lines.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

internal class WalletConfig : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("Wallets", "Finance"); 

        builder.HasKey(w => w.Id);

        builder.Property(w => w.UserId)
               .IsRequired();

        builder.Property(w => w.Balance)
               .HasPrecision(18, 2)
               .HasDefaultValue(0)
               .IsRequired();

        builder.Property(w => w.CreatedDate)
               .HasDefaultValueSql("GETDATE()")
               .IsRequired();

        //  One-to-one with ApplicationUser
        builder
            .HasOne<ApplicationUser>()
            .WithOne(u => u.Wallet)
            .HasForeignKey<Wallet>(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        //  Wallet - many - WalletTransactions
        builder
            .HasMany(w => w.Transactions)
            .WithOne(t => t.Wallet)
            .HasForeignKey(t => t.WalletId)
            .OnDelete(DeleteBehavior.Cascade);

        //   كل User لا يمكن أن يكون له أكثر من Wallet
        builder.HasIndex(w => w.UserId).IsUnique();
    }
}



//using Lines.Domain.Models.Drivers;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//namespace Lines.Infrastructure.Configurations;
//internal class WalletConfig : IEntityTypeConfiguration<Wallet>
//{
//    public void Configure(EntityTypeBuilder<Wallet> builder)
//    {
//        builder.ToTable("Wallets", "Driver");

//        builder.HasKey(w => w.Id);

//        builder.Property(w => w.DriverId)
//            .IsRequired();

//        builder.Property(w => w.Balance)
//            .HasPrecision(18, 2)
//            .HasDefaultValue(0)
//            .IsRequired();

//        builder.Property(w => w.CreatedDate)
//            .HasDefaultValueSql("GETDATE()")
//            .IsRequired();

//        // One-to-one relationship with Driver
//        builder.HasOne(w => w.Driver)
//            .WithOne(d => d.Wallet)
//            .HasForeignKey<Wallet>(w => w.DriverId)
//            .OnDelete(DeleteBehavior.Cascade);

//        // One-to-many relationship with transactions
//        builder.HasMany(w => w.Transactions)
//            .WithOne(t => t.Wallet)
//            .HasForeignKey(t => t.WalletId)
//            .OnDelete(DeleteBehavior.Cascade);

//        // Index for efficient driver lookup
//        builder.HasIndex(w => w.DriverId)
//            .IsUnique();
//    }
//}

