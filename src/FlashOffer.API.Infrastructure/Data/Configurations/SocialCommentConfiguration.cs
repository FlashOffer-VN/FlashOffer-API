using FlashOffer.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class SocialCommentConfiguration : IEntityTypeConfiguration<SocialComment>
{
    public void Configure(EntityTypeBuilder<SocialComment> builder)
    {
        builder.ToTable("SocialComment");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SocialCommentCode)
            .HasMaxLength(30);

        builder.HasIndex(x => x.SocialCommentCode)
            .IsUnique()
            .HasFilter("[SocialCommentCode] IS NOT NULL");
    }
}