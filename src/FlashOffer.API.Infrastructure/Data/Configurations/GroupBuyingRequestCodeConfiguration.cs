using FlashOffer.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlashOffer.API.Infrastructure.Data.Configurations;

/// <summary>
/// Config riêng cho cột GroupBuyingRequestCode.
/// NOTE: GroupBuyingRequestConfiguration.cs bị exclude khỏi compile (xem csproj),
/// nên config cột code được khai báo ở file riêng để đảm bảo được áp dụng.
/// </summary>
public class GroupBuyingRequestCodeConfiguration : IEntityTypeConfiguration<GroupBuyingRequest>
{
    public void Configure(EntityTypeBuilder<GroupBuyingRequest> builder)
    {
        builder.ToTable("GroupBuyingRequests");

        builder.Property(x => x.GroupBuyingRequestCode)
            .HasMaxLength(30);

        builder.HasIndex(x => x.GroupBuyingRequestCode)
            .IsUnique()
            .HasFilter("[GroupBuyingRequestCode] IS NOT NULL");
    }
}