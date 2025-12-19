// using Lines.Domain.Models;
// using Lines.Domain.Models.Users;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace Lines.Infrastructure.Configurations
// {
//     internal class UserPaymentMethodConfig : IEntityTypeConfiguration<UserPaymentMethod>  
//     {
//         public void Configure(EntityTypeBuilder<UserPaymentMethod> builder)
//         {
//             builder.ToTable("UserPaymentMethods" , "User");
//
//             builder.HasKey(upm => upm.Id);
//
//             builder.Property(upm => upm.UserId)
//                 .IsRequired();
//
//             builder.Property(upm => upm.PaymentMethodId)
//                 .IsRequired();
//
//             builder.Property(upm => upm.IsDefault)
//                 .IsRequired();
//
//             builder.Property(upm => upm.CreatedDate)
//                 .HasDefaultValueSql("GETDATE()")
//                 .IsRequired();
//
//             builder.Property(upm => upm.CardHolderName).HasMaxLength(100);
//             builder.Property(upm => upm.CardNumberMasked).HasMaxLength(30);
//             builder.Property(upm => upm.CardBrand).HasMaxLength(30);
//             builder.Property(upm => upm.CardExpiryMonth).HasMaxLength(2);
//             builder.Property(upm => upm.CardExpiryYear).HasMaxLength(4);
//             builder.Property(upm => upm.CardLast4).HasMaxLength(4);
//             builder.Property(upm => upm.PayPalEmail).HasMaxLength(100);
//             builder.Property(upm => upm.TokenReference).HasMaxLength(200);
//
//             builder.Property(upm => upm.IsCash)
//                 .IsRequired();
//
//             builder.HasOne(upm => upm.User)
//                 .WithMany(u => u.UserPaymentMethods)
//                 .HasForeignKey(upm => upm.UserId)
//                 .OnDelete(DeleteBehavior.NoAction);
//
//             builder.HasOne(upm => upm.PaymentMethod)
//                 .WithMany(pm => pm.UserPaymentMethods)
//                 .HasForeignKey(upm => upm.PaymentMethodId)
//                 .OnDelete(DeleteBehavior.NoAction);
//
//             // Data seeding (example)
//             builder.HasData(
//                 new UserPaymentMethod
//                 {
//                     Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
//                     UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
//                     PaymentMethodId = 1,
//                     IsDefault = true,
//                     CardHolderName = null,
//                     CardNumberMasked = null,
//                     CardBrand = null,
//                     CardExpiryMonth = null,
//                     CardExpiryYear = null,
//                     CardLast4 = null,
//                     PayPalEmail = null,
//                     TokenReference = null,
//                     IsCash = true
//                 },
//                 new UserPaymentMethod
//                 {
//                     Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
//                     UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
//                     PaymentMethodId = 2,
//                     IsDefault = true,
//                     CardHolderName = "John Doe",
//                     CardNumberMasked = "**** **** **** 1234",
//                     CardBrand = "Visa",
//                     CardExpiryMonth = "12",
//                     CardExpiryYear = "2027",
//                     CardLast4 = "1234",
//                     PayPalEmail = null,
//                     TokenReference = null,
//                     IsCash = false
//                 }
//             );
//         }
//     }
// }