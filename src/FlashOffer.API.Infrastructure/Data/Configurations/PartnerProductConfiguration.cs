// PartnerProductConfiguration.cs
using FlashOffer.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class PartnerProductConfiguration : IEntityTypeConfiguration<PartnerProduct>
{
    public void Configure(EntityTypeBuilder<PartnerProduct> builder)
    {
        builder.ToTable("PartnerProducts");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.RetailPrice).HasPrecision(18, 2);
        builder.Property(x => x.WholesalePrice).HasPrecision(18, 2);
        builder.Property(x => x.Category).HasConversion<int>();

        builder.Property(x => x.PartnerProductCode).HasMaxLength(30);
        builder.HasIndex(x => x.PartnerProductCode).IsUnique().HasFilter("[PartnerProductCode] IS NOT NULL");

        builder.HasOne(x => x.Partner)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.PartnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}