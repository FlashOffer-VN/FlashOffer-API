using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class OfferRequestConfiguration : IEntityTypeConfiguration<OfferRequest>
{
	public void Configure(EntityTypeBuilder<OfferRequest> builder)
	{
		builder.ToTable("OfferRequests");

		builder.Property(x => x.SelectedOffer)
			.IsRequired()
			.HasMaxLength(500);

		builder.Property(x => x.FullName)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Phone)
			.IsRequired()
			.HasMaxLength(11);

		builder.Property(x => x.Zalo)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(x => x.Email)
			.HasMaxLength(100);

		builder.Property(x => x.IsOfferSent)
			.HasDefaultValue(false);

		// Thêm vào Configure method
		builder.HasOne(x => x.User)
			.WithMany()
			.HasForeignKey(x => x.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.Property(x => x.Status)
			.HasConversion<int>()
			.HasDefaultValue(OfferStatus.Pending);

		builder.HasIndex(x => x.UserId);
		builder.HasIndex(x => x.Status);

		builder.HasIndex(x => x.Phone);
		builder.HasIndex(x => x.Zalo);
	}
}