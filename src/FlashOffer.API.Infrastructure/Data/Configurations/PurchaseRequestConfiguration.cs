// src/FlashOffer.API.Infrastructure/Data/Configurations/PurchaseRequestConfiguration.cs
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class PurchaseRequestConfiguration : IEntityTypeConfiguration<PurchaseRequest>
{
	public void Configure(EntityTypeBuilder<PurchaseRequest> builder)
	{
		builder.ToTable("PurchaseRequests");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.ProductName)
			.IsRequired()
			.HasMaxLength(255);

		builder.Property(x => x.Quantity)
			.IsRequired();

		builder.Property(x => x.ExpectedPrice)
			.HasPrecision(18, 2);

		builder.Property(x => x.FullName)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(x => x.Phone)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(x => x.Email)
			.HasMaxLength(255);

		builder.Property(x => x.Note)
			.HasMaxLength(1000);

		builder.Property(x => x.Status)
			.HasConversion<int>()
			.HasDefaultValue(PurchaseRequestStatus.Pending);

		builder.Property(x => x.AdminNote)
			.HasMaxLength(1000);

		builder.Property(x => x.AssignedTo);

		builder.Property(x => x.ResolvedAt);

		builder.Property(x => x.Source)
			.HasMaxLength(50);

		// Indexes
		builder.HasIndex(x => x.Status);
		builder.HasIndex(x => x.CreatedAt);
		builder.HasIndex(x => x.Phone);
	}
}