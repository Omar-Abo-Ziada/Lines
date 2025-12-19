using Lines.Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

public class CityVehicleTypeConfig : IEntityTypeConfiguration<CityVehicleType>
{
    public void Configure(EntityTypeBuilder<CityVehicleType> builder)
    {
        builder.ToTable("CityVehicleTypes", "Vehicle");

        builder.HasKey(cvt => cvt.Id);

        builder.HasOne(cvt => cvt.City)
            .WithMany(c => c.CityVehicleTypes)
            .HasForeignKey(cvt => cvt.CityId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(cvt => cvt.VehicleType)
            .WithMany(v => v.CityVehicleTypes)
            .HasForeignKey(cvt => cvt.VehicleTypeId)
            .OnDelete(DeleteBehavior.NoAction);
        
    }
}