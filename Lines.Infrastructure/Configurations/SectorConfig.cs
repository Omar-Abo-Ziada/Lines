using Lines.Domain.Models;
using Lines.Domain.Models.Sites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class SectorConfig : IEntityTypeConfiguration<Sector>   
    {
        public void Configure(EntityTypeBuilder<Sector> builder)
        {
            builder.ToTable("Sectors", "Sites");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(70);

            builder.Property(s => s.CityId)
                .IsRequired();

            builder.Property(a => a.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");

            builder.HasOne(s => s.City)             
                .WithMany(c => c.Sectors)
                .HasForeignKey(s => s.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            // Data seeding (example)
            builder.HasData(
                new Sector
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "sector 1",
                    CityId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },
                new Sector
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "sector 2",
                    CityId = Guid.Parse("22222222-2222-2222-2222-222222222222")
                }
            );
        }
    }
}