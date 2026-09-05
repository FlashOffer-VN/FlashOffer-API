// PartnerCommissionConfiguration.cs
using FlashOffer.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class PartnerCommissionConfiguration : IEntityTypeConfiguration<PartnerCommission>
{
    public void Configure(EntityTypeBuilder<PartnerCommission> builder)
    {
        builder.ToTable("PartnerCommissions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Rate).HasPrecision(18, 2);
        builder.Property(x => x.MinOrderValue).HasPrecision(18, 2);
        builder.Property(x => x.MaxCommission).HasPrecision(18, 2);
        builder.Property(x => x.SpecialConditions).HasMaxLength(500);
        builder.Property(x => x.Type).HasConversion<int>();

        builder.Property(x => x.PartnerCommissionCode).HasMaxLength(30);
        builder.HasIndex(x => x.PartnerCommissionCode).IsUnique().HasFilter("[PartnerCommissionCode] IS NOT NULL");

        builder.HasOne(x => x.Partner)
            .WithOne(x => x.Commission)
            .HasForeignKey<PartnerCommission>(x => x.PartnerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}