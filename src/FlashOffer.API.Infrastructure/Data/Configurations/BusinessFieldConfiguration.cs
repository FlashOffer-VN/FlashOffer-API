using FlashOffer.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class BusinessFieldConfiguration : IEntityTypeConfiguration<BusinessField>
{
    public void Configure(EntityTypeBuilder<BusinessField> builder)
    {
        builder.ToTable("BusinessFields");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.NormalizedName)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(x => x.NormalizedName)
            .IsUnique()
            .HasDatabaseName("IX_BusinessFields_NormalizedName");

        builder.Property(x => x.Aliases)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.BusinessFieldCode)
            .HasMaxLength(30);

        builder.HasIndex(x => x.BusinessFieldCode)
            .IsUnique()
            .HasFilter("[BusinessFieldCode] IS NOT NULL");
    }
}