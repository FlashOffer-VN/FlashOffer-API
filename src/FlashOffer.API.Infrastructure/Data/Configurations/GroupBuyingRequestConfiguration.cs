using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class GroupBuyingRequestConfiguration : IEntityTypeConfiguration<GroupBuyingRequest>
{
	public void Configure(EntityTypeBuilder<GroupBuyingRequest> builder)
	{
		builder.ToTable("GroupBuyingRequests");

		builder.Property(x => x.ProductName)
			.IsRequired()
			.HasMaxLength(500);

		builder.Property(x => x.FullName)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Phone)
			.IsRequired()
			.HasMaxLength(11);

		builder.Property(x => x.Note)
			.HasMaxLength(1000);

		builder.Property(x => x.Status)
			.HasConversion<int>()
			.HasDefaultValue(GroupBuyingStatus.Pending);

		builder.HasIndex(x => x.Phone);
	}
}