using Lines.Domain.Models.Drivers.StripeAccounts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class DriverStripeAccountConfig
    : IEntityTypeConfiguration<DriverStripeAccount>
{
    public void Configure(EntityTypeBuilder<DriverStripeAccount> builder)
    {
        builder.ToTable("DriverStripeAccounts", "Payments");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.DriverId).IsUnique();

        builder.Property(x => x.StripeAccountId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.ChargesEnabled).IsRequired();
        builder.Property(x => x.PayoutsEnabled).IsRequired();
        builder.Property(x => x.DetailsSubmitted).IsRequired();
    }
}
