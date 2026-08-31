using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class CollaboratorConfiguration : IEntityTypeConfiguration<Collaborator>
{
    public void Configure(EntityTypeBuilder<Collaborator> builder)
    {
        builder.ToTable("Collaborators");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Zalo)
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .HasMaxLength(100);

        builder.Property(c => c.Position)
            .HasMaxLength(100);

        builder.Property(c => c.Skills)
            .HasMaxLength(500);

        builder.Property(c => c.Interests)
            .HasMaxLength(500);

        builder.Property(c => c.Goals)
            .HasMaxLength(500);

        builder.Property(c => c.Experience)
            .HasMaxLength(1000);

        builder.Property(c => c.ReferralCode)
            .HasMaxLength(50);

        builder.Property(c => c.RejectionReason)
            .HasMaxLength(500);

        // Cấu hình IsApproved
        builder.Property(c => c.IsApproved)
            .IsRequired()
            .HasDefaultValue(false);

        // Cấu hình Status (dùng field _status)
        builder.Property(c => c.Status)
          .HasField("_status")
          .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction)
          .HasConversion<int>()
          .HasDefaultValue(CollaboratorStatus.Pending)
          .HasColumnName("Status");

        builder.Property(c => c.Level)
            .HasDefaultValue(1);

        builder.Property(c => c.BusinessFieldName)
            .HasMaxLength(200);

        // Indexes
        builder.HasIndex(c => c.UserId)
            .IsUnique();

        builder.HasIndex(c => c.Phone)
            .IsUnique();

        builder.HasIndex(c => c.Email)
            .IsUnique()
            .HasFilter("[Email] IS NOT NULL");

        builder.HasIndex(c => c.ReferralCode)
            .IsUnique()
            .HasFilter("[ReferralCode] IS NOT NULL");

        // Relationships
        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.ParentCollaborator)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentCollaboratorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.BusinessFieldId)
          .HasColumnType("uuid");
    }
}