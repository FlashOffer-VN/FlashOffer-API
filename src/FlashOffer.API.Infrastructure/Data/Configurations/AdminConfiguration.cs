using FlashOffer.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
	public void Configure(EntityTypeBuilder<Admin> builder)
	{
		builder.ToTable("Admins");
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Username)
			.IsRequired()
			.HasMaxLength(50);

		builder.HasIndex(x => x.Username)
			.IsUnique();

		builder.Property(x => x.PasswordHash)
			.IsRequired()
			.HasMaxLength(255);

		builder.Property(x => x.FullName)
			.HasMaxLength(200);

		builder.Property(x => x.Email)
			.HasMaxLength(100);
	}
}