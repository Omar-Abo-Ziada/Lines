using Lines.Domain.Models.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Infrastructure.Configurations.Notifications
{
    internal class FCMUserTokenConfig : IEntityTypeConfiguration<FCMUserToken>
    {

        public void Configure(EntityTypeBuilder<FCMUserToken> b)


        {
            b.ToTable("FCMUserTokens","Notify");

            b.HasKey(x => x.Id);

            b.Property(x => x.UserId)
             .IsRequired()
             .HasMaxLength(128); 

            b.Property(x => x.Token)
             .IsRequired()
             .HasMaxLength(2048);

            b.Property(x => x.DeviceId)
             .HasMaxLength(256);


            b.Property(x => x.Locale).HasMaxLength(20);

            b.HasIndex(x => x.Token).IsUnique();
            b.HasIndex(x => new
            {
                x.UserId,
                x.Platform
            });
            //b.HasIndex(x => new
            //{
            //    x.UserId,
            //    x.IsActive
            //});

            // (اختياري) لو عندك علاقة User
            // b.HasOne<ApplicationUser>()
            //  .WithMany()
            //  .HasForeignKey(x => x.UserId)
            //  .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
