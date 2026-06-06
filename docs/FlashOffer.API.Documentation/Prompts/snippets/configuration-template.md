using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class {EntityName}Configuration : IEntityTypeConfiguration<{EntityName}>
{
    public void Configure(EntityTypeBuilder<{EntityName}> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        // ThÃªm cÃ¡c cáº¥u hÃ¬nh khÃ¡c táº¡i Ä‘Ã¢y
    }
}