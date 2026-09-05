using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class SocialPostConfiguration : IEntityTypeConfiguration<SocialPost>
{
    public void Configure(EntityTypeBuilder<SocialPost> builder)
    {
        builder.ToTable("SocialPosts");

        builder.HasKey(x => x.Id);

        // Content - bắt buộc
        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(5000);

        // Title - không bắt buộc
        builder.Property(x => x.Title)
            .HasMaxLength(200);

        // Enum -> int
        builder.Property(x => x.Type)
            .HasConversion<int>()
            .HasDefaultValue(PostType.Post);

        builder.Property(x => x.Privacy)
            .HasConversion<int>()
            .HasDefaultValue(PrivacyType.Public);

        builder.Property(x => x.Priority)
            .HasConversion<int>()
            .HasDefaultValue(PriorityType.Normal);

        builder.Property(x => x.Images)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>()
            )
            .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            ));

        // Relationship với User
        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship với Tag
        builder.HasMany(p => p.PostTags)
        .WithOne(pt => pt.Post)
        .HasForeignKey(pt => pt.PostId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.SocialPostCode)
            .HasMaxLength(30);

        builder.HasIndex(x => x.SocialPostCode)
            .IsUnique()
            .HasFilter("[SocialPostCode] IS NOT NULL");

        // Index cho tối ưu query
        builder.HasIndex(x => x.AuthorId);
        builder.HasIndex(x => x.Type);
        builder.HasIndex(x => x.Privacy);
        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.IsDeleted);
    }
}