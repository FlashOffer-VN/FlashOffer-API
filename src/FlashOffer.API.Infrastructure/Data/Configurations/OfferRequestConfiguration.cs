using FlashOffer.API.Domain.Entities;
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

		builder.HasIndex(x => x.Phone);
		builder.HasIndex(x => x.Zalo);
	}
}