using Lines.Domain.Models.Chats;
using Lines.Domain.Models.TermsAndConditions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Infrastructure.Configurations
{
    internal class TermsAndConditionsConfig : IEntityTypeConfiguration<TermsAndConditions>
    {
        public void Configure(EntityTypeBuilder<TermsAndConditions> builder)
        {

            builder.ToTable("TermsAndConditions", "Config");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Header)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(t => t.Content)
                .IsRequired();

            builder.Property(t => t.TermsType)
                .IsRequired();

            builder.Property(t => t.Order)
                .IsRequired();

            builder.Property(a => a.CreatedDate).IsRequired()
              .HasDefaultValueSql("GETDATE()");


            // Data seeding  
            builder.HasData(
                // PrivacyPolicy
                new TermsAndConditions
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Header = "Privacy Policy 1",
                    Content = "This is the privacy policy content 1.",
                    TermsType = Domain.Enums.TermsType.PrivacyPolicy,
                    Order = 1
                },
                new TermsAndConditions
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    Header = "Privacy Policy 2",
                    Content = "This is the privacy policy content 2.",
                    TermsType = Domain.Enums.TermsType.PrivacyPolicy,
                    Order = 2
                },
                new TermsAndConditions
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000007"),
                    Header = "Privacy Policy 3",
                    Content = "This is the privacy policy content 3.",
                    TermsType = Domain.Enums.TermsType.PrivacyPolicy,
                    Order = 3
                },

                // TermsOfUs
                new TermsAndConditions
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Header = "Terms of Use 1",
                    Content = "This is the terms of use content 1.",
                    TermsType = Domain.Enums.TermsType.TermsOfUs,
                    Order = 1
                },
                new TermsAndConditions
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000005"),
                    Header = "Terms of Use 2",
                    Content = "This is the terms of use content 2.",
                    TermsType = Domain.Enums.TermsType.TermsOfUs,
                    Order = 2
                },
                new TermsAndConditions
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000008"),
                    Header = "Terms of Use 3",
                    Content = "This is the terms of use content 3.",
                    TermsType = Domain.Enums.TermsType.TermsOfUs,
                    Order = 3
                },

                // ServiceRules
                new TermsAndConditions
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Header = "Service Rules 1",
                    Content = "These are the service rules content 1.",
                    TermsType = Domain.Enums.TermsType.ServiceRules,
                    Order = 1
                },
                new TermsAndConditions
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000006"),
                    Header = "Service Rules 2",
                    Content = "These are the service rules content 2.",
                    TermsType = Domain.Enums.TermsType.ServiceRules,
                    Order = 2
                },
                new TermsAndConditions
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000009"),
                    Header = "Service Rules 3",
                    Content = "These are the service rules content 3.",
                    TermsType = Domain.Enums.TermsType.ServiceRules,
                    Order = 3
                }
            );
        }

    }
}
