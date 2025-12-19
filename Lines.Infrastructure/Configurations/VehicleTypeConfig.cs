using Lines.Domain.Models;
using Lines.Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class VehicleTypeConfig : IEntityTypeConfiguration<VehicleType>
    {
        public void Configure(EntityTypeBuilder<VehicleType> builder)
        {
            builder.ToTable("VehicleTypes" , "Vehicle");

            builder.HasKey(vt => vt.Id);

            builder.Property(vt => vt.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");

            builder.Property(vt => vt.Description)
                .HasMaxLength(200);

            builder.Property(vt => vt.Capacity)
                .IsRequired();

            builder.Property(vt => vt.PerKmCharge)
                .IsRequired();

            builder.Property(vt => vt.PerMinuteDelayCharge)
                .IsRequired();

            builder.HasMany(vt => vt.Vehicles)
                .WithOne(v => v.VehicleType)
                .HasForeignKey(v => v.VehicleTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Data seeding (example)
            builder.HasData(
                new VehicleType
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Classic",
                    Description = "Standard Car",
                    Capacity = 6,
                    PerKmCharge = 1.50m,
                    PerMinuteDelayCharge = 0.50m
                },
                new VehicleType
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Sport Vehicle",
                    Description = "Sport Car",
                    Capacity = 4,
                    PerKmCharge = 2.00m,
                    PerMinuteDelayCharge = 0.75m
                }
            );
        }
    }
}