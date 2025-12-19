using Lines.Domain.Models;
using Lines.Domain.Models.Sites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class CityConfig : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities", "Sites");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");

            builder.HasMany(c => c.Sectors)
                .WithOne(s => s.City)
                .HasForeignKey(s => s.CityId);

            // Data seeding
            builder.HasData(
                new City
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Zurich"
                },
                new City
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Geneva"
                }
            );
        }
    }
}