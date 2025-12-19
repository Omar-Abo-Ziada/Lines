using Lines.Domain.Value_Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class UserConfigurationHelper
{
    public static void ConfigureEmail<T>(OwnedNavigationBuilder<T, Email> email) where T : class
    {
        email.Property(e => e.Value)
             .HasColumnName("Email")
             .IsRequired();
    }

    public static void ConfigurePhoneNumber<T>(OwnedNavigationBuilder<T, PhoneNumber> phone) where T : class
    {
        phone.Property(p => p.Value)
             .HasColumnName("PhoneNumber")
             .IsRequired();
    }
}
