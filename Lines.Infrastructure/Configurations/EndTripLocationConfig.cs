using Lines.Domain.Models;
using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class EndTripLocationConfig : IEntityTypeConfiguration<EndTripLocation>
    {
        public void Configure(EntityTypeBuilder<EndTripLocation> builder)
        {
            builder.ToTable("EndTripLocations", "Trip");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.TripRequestId)
                .IsRequired();

            builder.OwnsOne(t => t.Location, loc =>
            {
                loc.Property(l => l.Latitude).HasColumnName("Start_Latitude");
                loc.Property(l => l.Longitude).HasColumnName("Start_Longitude");
                loc.Property(l => l.Address).HasColumnName("Start_Address");
            });


            builder.Property(e => e.Order)
                .IsRequired();

            builder.Property(a => a.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");

            builder.HasOne(e => e.Trip)
                .WithMany(t => t.EndLocations)
                .HasForeignKey(e => e.TripId)
                .OnDelete(DeleteBehavior.NoAction);

            // // Data seeding
            //builder.HasData(
            //    new EndTripLocation
            //    {
            //        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            //        TripId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            //        TripRequestId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            //        Order = 1
            //    },
            //    new EndTripLocation
            //    {
            //        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            //        TripId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            //        TripRequestId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            //        Order = 2
            //    }
            //);
        }
    }
}