// CreatePartnerProductDto.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.Requests;

/// <summary>
/// Thêm sản phẩm/dịch vụ cho đối tác.
/// </summary>
public class CreatePartnerProductDto : IMapFrom<PartnerProduct>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ProductCategory? Category { get; set; }
    public decimal? RetailPrice { get; set; }
    public decimal? WholesalePrice { get; set; }
    public int? MinOrderQuantity { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreatePartnerProductDto, PartnerProduct>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category ?? ProductCategory.Other))
            .ForMember(dest => dest.RetailPrice, opt => opt.MapFrom(src => src.RetailPrice ?? 0m))
            .ForMember(dest => dest.WholesalePrice, opt => opt.MapFrom(src => src.WholesalePrice ?? 0m))
            .ForMember(dest => dest.MinOrderQuantity, opt => opt.MapFrom(src => src.MinOrderQuantity ?? 1));
    }
}
