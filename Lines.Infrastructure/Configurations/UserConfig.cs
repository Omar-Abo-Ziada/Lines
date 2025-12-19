using Lines.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.UseTpcMappingStrategy();

        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Rating)
          .IsRequired();

        builder.Property(u => u.TotalTrips)
            .IsRequired();

        builder.Property(u => u.RatedTripsCount)
            .IsRequired();


        builder.Property(u => u.Points)
            .IsRequired();
    }
}