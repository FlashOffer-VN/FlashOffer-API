using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlashOffer.API.Domain.Entities;

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

		builder.HasIndex(x => x.Phone);
		builder.HasIndex(x => x.Email);
	}
}