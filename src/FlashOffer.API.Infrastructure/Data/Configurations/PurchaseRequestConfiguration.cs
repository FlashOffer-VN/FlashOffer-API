// Bước 2: Tạo Configuration
// src/FlashOffer.API.Infrastructure/Configurations/PurchaseRequestConfiguration.cs

using FlashOffer.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Configurations;

public class PurchaseRequestConfiguration : IEntityTypeConfiguration<PurchaseRequest>
{
	public void Configure(EntityTypeBuilder<PurchaseRequest> builder)
	{
		builder.ToTable("PurchaseRequests");

		builder.Property(x => x.ProductName)
			.HasMaxLength(500)
			.IsRequired();

		builder.Property(x => x.FullName)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(x => x.Phone)
			.HasMaxLength(11)
			.IsRequired();

		builder.Property(x => x.Email)
			.HasMaxLength(200);

		builder.Property(x => x.Note)
			.HasMaxLength(1000);

		builder.Property(x => x.Status)
			.HasMaxLength(50)
			.HasDefaultValue("Pending");

		builder.HasIndex(x => x.Phone);
		builder.HasIndex(x => x.Status);
	}
}