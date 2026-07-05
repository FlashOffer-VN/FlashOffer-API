// PartnerConfiguration.cs
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class PartnerConfiguration : IEntityTypeConfiguration<Partner>
{
    public void Configure(EntityTypeBuilder<Partner> builder)
    {
        builder.ToTable("Partners");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.PartnerCode).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.PartnerCode).IsUnique();
        builder.HasIndex(x => x.Phone);
        builder.HasIndex(x => x.Email);

        builder.Property(x => x.FullName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(15).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Position).HasMaxLength(100).IsRequired();
        builder.Property(x => x.CompanyName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.CompanyTax).HasMaxLength(20).IsRequired();
        builder.Property(x => x.CompanyAddress).HasMaxLength(500).IsRequired();
        builder.Property(x => x.CompanyWebsite).HasMaxLength(200);
        builder.Property(x => x.ReferralCode).HasMaxLength(50);
        builder.Property(x => x.Note).HasMaxLength(500);

        builder.Property(x => x.BusinessType).HasConversion<int>();
        builder.Property(x => x.CompanySize).HasConversion<int>();
        builder.Property(x => x.Status).HasConversion<int>().HasDefaultValue(PartnerStatus.Pending);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}