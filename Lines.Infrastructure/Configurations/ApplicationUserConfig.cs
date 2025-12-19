using Lines.Domain.Models.Drivers;
using Lines.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

internal class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {

        //builder.ToTable("ApplicationUsers", "User");
        builder
       .ToTable("ApplicationUsers", "User", t =>
       {
        // Check Constraint - to prevent a user from being both a driver and a passenger at the same time
           t.HasCheckConstraint(
               "CK_ApplicationUser_DriverOrPassenger",
               @"(
                    (""DriverId"" IS NOT NULL AND ""PassengerId"" IS NULL) OR
                    (""DriverId"" IS NULL AND ""PassengerId"" IS NOT NULL) OR
                    (""DriverId"" IS NULL AND ""PassengerId"" IS NULL)
                )"
           );
       });


        builder.HasKey(u => u.Id);

        builder.Property(p => p.StripeCustomerId)
         .IsRequired(false);

        // relations
        builder.HasOne(au => au.Passenger)
            .WithOne()
            .HasForeignKey<ApplicationUser>(au => au.PassengerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(au => au.Driver)
        .WithOne()
        .HasForeignKey<ApplicationUser>(au => au.DriverId)
        .OnDelete(DeleteBehavior.NoAction);


        // Add Wallet Relation
        builder.HasOne(u => u.Wallet)
            .WithOne()
            .HasForeignKey<Wallet>(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    


        builder.HasMany(u => u.Otps)
           .WithOne()
           .HasForeignKey(o => o.UserId)
           .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(au => au.EmergencyNumbers)
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.HasMany(u => u.Notifications)
        .WithOne()
        .HasForeignKey(n => n.UserId)
        .OnDelete(DeleteBehavior.NoAction);


        builder.HasMany(u => u.Activities)
            .WithOne()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.UserRewards)
        .WithOne()
        .HasForeignKey(r => r.UserId)
        .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.MessagesSent)
         .WithOne()
         .HasForeignKey(m => m.SenderId)
         .OnDelete(DeleteBehavior.NoAction);


        builder.HasMany(u => u.MessagesRecieved)
         .WithOne()
         .HasForeignKey(m => m.RecipientId)
         .OnDelete(DeleteBehavior.NoAction);


        builder.HasMany(u => u.UserPaymentMethods)  
          .WithOne()
          .HasForeignKey(upm => upm.UserId)
          .OnDelete(DeleteBehavior.NoAction);



        // Data seeding
        builder.HasData(
                new ApplicationUser
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    UserName = "Mohamed.driver",
                    NormalizedUserName = "MOHAMED.DRIVER",
                    Email = "mohamed.driver@example.com",
                    NormalizedEmail = "MOHAMED.DRIVER@EXAMPLE.COM",
                    EmailConfirmed = true,
                    IsActive = true,
                    SecurityStamp = "STATIC-SECURITY-STAMP-VALUE",
                    ConcurrencyStamp = "Static-Concurrency-Stamp-Value",
                    PasswordHash = "AQAAAAEAACcQAAAAE...", // Use a real hash in production
                    DriverId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    PassengerId = null,
                    PhoneNumber = "01234567890"
                },
                new ApplicationUser
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    UserName = "khaled.driver",
                    NormalizedUserName = "KHALED.DRIVER",
                    Email = "khaled.driver@example.com",
                    NormalizedEmail = "KHALED.DRIVER@EXAMPLE.COM",
                    EmailConfirmed = true,
                    IsActive = false,
                    SecurityStamp = "STATIC-SECURITY-STAMP-VALUE",
                    ConcurrencyStamp = "Static-Concurrency-Stamp-Value",
                    PasswordHash = "AQAAAAEAACcQAAAAE...", // Use a real hash in production
                    DriverId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    PassengerId = null,
                    PhoneNumber = "01234567891"
                },
                new ApplicationUser
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    UserName = "ahmed.passenger",
                    NormalizedUserName = "AHMED.PASSENGER",
                    Email = "ahmed.passenger@example.com",
                    NormalizedEmail = "AHMED.PASSENGER@EXAMPLE.COM",
                    EmailConfirmed = true,
                    IsActive = true,
                    SecurityStamp = "STATIC-SECURITY-STAMP-VALUE",
                    ConcurrencyStamp = "Static-Concurrency-Stamp-Value",
                    PasswordHash = "AQAAAAEAACcQAAAAE...", // Use a real hash in production
                    DriverId = null,
                    PassengerId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    PhoneNumber = "01234567890"
                },
                new ApplicationUser
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    UserName = "Mostafa.passenger",
                    NormalizedUserName = "MOSTAFA.PASSENGER",
                    Email = "mostafa.passenger@example.com",
                    NormalizedEmail = "MOSTAFA.PASSENGER@EXAMPLE.COM",
                    EmailConfirmed = true,
                    IsActive = true,
                    SecurityStamp = "STATIC-SECURITY-STAMP-VALUE",
                    ConcurrencyStamp = "Static-Concurrency-Stamp-Value",
                    PasswordHash = "AQAAAAEAACcQAAAAE...", // Use a real hash in production
                    DriverId = null,
                    PassengerId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    PhoneNumber = "01534567890"
                },
                 new ApplicationUser
                 {
                     Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),  // consider it as the ai support agent
                     UserName = "Support.user",
                     NormalizedUserName = "SUPPORT.USER",
                     Email = "support.user@example.com",
                     NormalizedEmail = "SUPPORT.USER@EXAMPLE.COM",
                     EmailConfirmed = true,
                     IsActive = true,
                     SecurityStamp = "STATIC-SECURITY-STAMP-VALUE",
                     ConcurrencyStamp = "Static-Concurrency-Stamp-Value",
                     PasswordHash = "AQAAAAEAACcQAAAAE...", // Use a real hash in production
                     DriverId = null,
                     PassengerId = null,
                     PhoneNumber = "01234567892" // Example phone number for support user
                 }
            );
    }
}