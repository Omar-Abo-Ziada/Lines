using Lines.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Lines.Infrastructure.Configurations
{
    internal class RoleConfig : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            // Data seeding
            builder.HasData(
                    new IdentityRole<Guid>
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Driver",
                        NormalizedName = "DRIVER"
                    },
                    new IdentityRole<Guid>
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Name = "Passenger",
                        NormalizedName = "PASSENGER"
                    }
                    );
        }
    }
}