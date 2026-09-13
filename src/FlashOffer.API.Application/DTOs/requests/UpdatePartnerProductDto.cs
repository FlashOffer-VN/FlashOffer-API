// UpdatePartnerProductDto.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.Requests;

/// <summary>
/// Cập nhật sản phẩm/dịch vụ của đối tác — partial update: field null giữ nguyên.
/// Không cho đổi PartnerProductCode (mã hệ thống sinh) hay PartnerId (đổi chủ sở hữu
/// phải qua API của partner kia).
/// </summary>
public class UpdatePartnerProductDto : IMapFrom<PartnerProduct>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ProductCategory? Category { get; set; }
    public decimal? RetailPrice { get; set; }
    public decimal? WholesalePrice { get; set; }
    public int? MinOrderQuantity { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdatePartnerProductDto, PartnerProduct>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
}
