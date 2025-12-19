using Lines.Domain.Models;
using Lines.Domain.Models.Sites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class FavoriteLocationConfig : IEntityTypeConfiguration<FavoriteLocation>
    {
        public void Configure(EntityTypeBuilder<FavoriteLocation> builder)
        {
            builder.ToTable("FavoriteLocations", "Sites");

            builder.HasKey(l => l.Id);
            
            builder.Property(l => l.Name)
                .IsRequired();

            builder.Property(l => l.Latitude)
                .IsRequired();

            builder.Property(l => l.Longitude)
                .IsRequired();

            builder.Property(l => l.CityId)
                .IsRequired();

            builder.Property(l => l.SectorId)
                .IsRequired();

            builder.Property(a => a.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");

            // Data seeding (example)
            builder.HasData(
                new FavoriteLocation
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "dsds",
                    Latitude = 47.3769,
                    Longitude = 8.5417,
                    PassengerId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    CityId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    SectorId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },
                new FavoriteLocation
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "sqqq",
                    Latitude = 46.2044,
                    Longitude = 6.1432,
                    PassengerId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    CityId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    SectorId = Guid.Parse("22222222-2222-2222-2222-222222222222")
                }
            );
        }
    }
}