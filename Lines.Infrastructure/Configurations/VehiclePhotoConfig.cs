using Lines.Domain.Models;
using Lines.Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class VehiclePhotoConfig : IEntityTypeConfiguration<VehiclePhoto>
    {
        public void Configure(EntityTypeBuilder<VehiclePhoto> builder)
        {
            builder.ToTable("VehiclePhotos", "Vehicle");

            builder.HasKey(vp => vp.Id);

            builder.Property(a => a.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");

            builder.Property(vp => vp.VehicleId)
                .IsRequired();

            builder.Property(vp => vp.PhotoUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(vp => vp.Description)
                .HasMaxLength(200);

            builder.Property(vp => vp.IsPrimary)
                .IsRequired();

            builder.HasOne(vp => vp.Vehicle)
                .WithMany(v => v.Photos)
                .HasForeignKey(vp => vp.VehicleId)
                .OnDelete(DeleteBehavior.NoAction);

            // Data seeding (example)
            builder.HasData(
                new VehiclePhoto
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    VehicleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    PhotoUrl = "https://example.com/photos/vehicle1_main.jpg",
                    Description = "Main photo of Toyota Prius",
                    IsPrimary = true
                },
                new VehiclePhoto
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    VehicleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    PhotoUrl = "https://example.com/photos/vehicle1_side.jpg",
                    Description = "Side view",
                    IsPrimary = false
                },
                new VehiclePhoto
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    VehicleId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    PhotoUrl = "https://example.com/photos/vehicle2_main.jpg",
                    Description = "Main photo of Honda Civic",
                    IsPrimary = true
                }
            );
        }
    }
}