using Lines.Domain.Models.Trips;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Infrastructure.Configurations
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.Property(t => t.TripCode)
                .HasMaxLength(16);

            builder.HasIndex(t => t.TripCode)
                .IsUnique()
                .HasFilter("[TripCode] IS NOT NULL");
        }
    }

}
