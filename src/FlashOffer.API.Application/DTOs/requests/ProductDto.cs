// ProductDto.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Domain.Enums;

public class ProductDto : IMapFrom<PartnerProduct>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ProductCategory Category { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal WholesalePrice { get; set; }
    public int MinOrderQuantity { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProductDto, PartnerProduct>();
    }
}