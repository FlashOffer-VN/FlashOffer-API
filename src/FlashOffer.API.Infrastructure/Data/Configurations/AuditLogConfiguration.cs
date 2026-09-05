using FlashOffer.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EntityName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Action)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.ActorId)
            .HasMaxLength(100);

        builder.Property(x => x.ActorName)
            .HasMaxLength(200);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(50);

        builder.Property(x => x.OperatingSystem)
            .HasMaxLength(100);

        builder.Property(x => x.BrowserName)
            .HasMaxLength(100);

        builder.Property(x => x.DeviceType)
            .HasMaxLength(50);

        builder.Property(x => x.OldValues)
            .HasColumnType("text");

        builder.Property(x => x.NewValues)
            .HasColumnType("text");

        builder.Property(x => x.ChangedProperties)
            .HasColumnType("text");

        // Index cho tối ưu query admin
        builder.HasIndex(x => x.EntityName);
        builder.HasIndex(x => x.EntityId);
        builder.HasIndex(x => x.Action);
        builder.HasIndex(x => x.Timestamp);
        builder.HasIndex(x => x.ActorId);
    }
}