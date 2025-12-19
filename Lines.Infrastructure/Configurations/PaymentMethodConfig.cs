using Lines.Domain.Enums;
using Lines.Domain.Models.PaymentMethods;
using Lines.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{  
    internal class PaymentMethodConfig : IEntityTypeConfiguration<PaymentMethod> 
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable("PaymentMethods" , "PaymentMethod");

            builder.HasKey(pm => pm.Id);

            builder.Property(pm => pm.IsDefault)
                .IsRequired();

            builder.Property(pm => pm.CreatedDate)
                    .HasDefaultValueSql("GETDATE()")
                    .IsRequired();
            
            builder.HasOne<ApplicationUser>()
                .WithMany(u => u.UserPaymentMethods)
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(pm => pm.Payments)
              .WithOne(p => p.PaymentMethod)
              .HasForeignKey(upm => upm.PaymentMethodId)
              .OnDelete(DeleteBehavior.NoAction);

            //Data seeding (example)
            builder.HasData(
                new PaymentMethod
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Type = PaymentMethodType.Cash,
                    IsDefault = true,
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // لازم تكون User موجود بالفعل
                    CreatedDate = new DateTime(2024, 8, 9, 0, 0, 0),
                    CustomerId = "cus_test_12345",
                    PaymentGatewayPaymentMethodId = "pm_test_67890"
                },
                new PaymentMethod
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Type = PaymentMethodType.ApplePay,
                    IsDefault = false,
                    UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // نفس اليوزر أو يوزر تاني
                    CreatedDate = new DateTime(2024, 10, 6, 0, 0, 0),
                    CustomerId = "cus_test_67895",
                    PaymentGatewayPaymentMethodId = "pm_test_12345"
                }
            );

        }
    }
}