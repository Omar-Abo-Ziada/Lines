using Lines.Domain.Models.License;
using Lines.Domain.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Infrastructure.Configurations
{
    internal class LicensePhotoConfig : IEntityTypeConfiguration<LicensePhoto>
    {
        public void Configure(EntityTypeBuilder<LicensePhoto> builder)
        {
            builder.ToTable("LicensePhotos", "License");

            builder.HasKey(lp => lp.Id);

            builder.Property(lp => lp.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");

            builder.Property(lp => lp.LicenseId)
                .IsRequired();

            builder.Property(lp => lp.PhotoUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(lp => lp.License)
                .WithMany(l => l.Photos)
                .HasForeignKey(lp => lp.LicenseId) 
                .OnDelete(DeleteBehavior.NoAction);

            // Data seeding (example)
            builder.HasData(
                new LicensePhoto
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    PhotoUrl = "https://example.com/photos/vehicle1_main.jpg",
                    LicenseId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },
                new LicensePhoto
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    PhotoUrl = "https://example.com/photos/vehicle1_side.jpg",
                    LicenseId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },
                new LicensePhoto
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    PhotoUrl = "https://example.com/photos/vehicle2_main.jpg",
                    LicenseId = Guid.Parse("22222222-2222-2222-2222-222222222222")

                }
            );
        }
    }
}