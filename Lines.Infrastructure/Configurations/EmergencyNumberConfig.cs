using Lines.Domain.Enums;
using Lines.Domain.Models;
using Lines.Domain.Models.Users;
using Lines.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lines.Infrastructure.Configurations
{
    internal class EmergencyNumberConfig : IEntityTypeConfiguration<EmergencyNumber>
    {
        public void Configure(EntityTypeBuilder<EmergencyNumber> builder)
        {
            builder.ToTable("EmergencyNumbers", "User");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.EmergencyNumberType)
                .HasDefaultValue(EmergencyNumberType.PersonalEmergencyNumber)
                .IsRequired();

            builder.Property(e => e.UserId);

            builder.Property(a => a.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");

            builder.HasOne<ApplicationUser>()
                .WithMany(u => u.EmergencyNumbers)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);     // del his emergency numbers when the user itself is deleted

            // Data seeding
            builder.HasData(
                new EmergencyNumber
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Police",
                    PhoneNumber = "117",
                    EmergencyNumberType = EmergencyNumberType.SharedEmergencyNumber,
                    UserId = null
                },
                new EmergencyNumber
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Ambulance",
                    PhoneNumber = "144",
                    EmergencyNumberType = EmergencyNumberType.SharedEmergencyNumber,
                    UserId = null
                },
                 new EmergencyNumber
                 {
                     Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                     Name = "Maged",
                     PhoneNumber = "01127378619",
                     EmergencyNumberType = EmergencyNumberType.PersonalEmergencyNumber,
                     UserId = Guid.Parse("33333333-3333-3333-3333-333333333333")
                 }
            );
        }
    }
}