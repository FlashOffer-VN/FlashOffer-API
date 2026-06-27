using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class CtvRegistrationConfiguration : IEntityTypeConfiguration<CtvRegistration>
{
	public void Configure(EntityTypeBuilder<CtvRegistration> builder)
	{
		builder.ToTable("CtvRegistrations");

		builder.Property(x => x.FullName)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Phone)
			.IsRequired()
			.HasMaxLength(11);

		builder.Property(x => x.Zalo)
			.HasMaxLength(50);

		builder.Property(x => x.Email)
			.HasMaxLength(100);

		builder.Property(x => x.SalesChannel)
			.HasMaxLength(100);

		builder.Property(x => x.Experience)
			.HasMaxLength(1000);

		builder.Property(x => x.IsApproved)
			.HasDefaultValue(false);

		builder.Property(x => x.CreatedAt)
			.IsRequired();

		// Thêm vào Configure method
		builder.HasOne(x => x.User)
			.WithMany()
			.HasForeignKey(x => x.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.Property(x => x.Status)
			.HasConversion<int>()
			.HasDefaultValue(CTVRegistrationStatus.Pending);

		builder.HasIndex(x => x.UserId);
		builder.HasIndex(x => x.Status);
		builder.HasIndex(x => x.Phone);
		builder.HasIndex(x => x.Email);
	}
}