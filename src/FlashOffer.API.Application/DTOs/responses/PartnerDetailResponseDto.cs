using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using AutoMapper;

namespace FlashOffer.API.Application.DTOs.Responses;

public class PartnerDetailResponseDto : IMapFrom<Partner>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string PartnerCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyTax { get; set; } = string.Empty;
    public string CompanyAddress { get; set; } = string.Empty;
    public BusinessType BusinessType { get; set; }
    public CompanySize CompanySize { get; set; }
    public string? CompanyWebsite { get; set; }
    public string? ReferralCode { get; set; }
    public string? Note { get; set; }
    public PartnerStatus Status { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public UserBriefDto? User { get; set; }
    public PartnerCommissionDto? Commission { get; set; }
    public List<PartnerProductDto> Products { get; set; } = new();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Partner, PartnerDetailResponseDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Commission, opt => opt.MapFrom(src => src.Commission))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        // ✅ Mapping cho các DTO con
        profile.CreateMap<User, UserBriefDto>();
        profile.CreateMap<PartnerCommission, PartnerCommissionDto>();
        profile.CreateMap<PartnerProduct, PartnerProductDto>();
    }
}

public class UserBriefDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
}

public class PartnerCommissionDto
{
    public Guid Id { get; set; }
    public CommissionType Type { get; set; }
    public decimal Rate { get; set; }
    public decimal? MinOrderValue { get; set; }
    public decimal? MaxCommission { get; set; }
    public string? SpecialConditions { get; set; }
}

public class PartnerProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ProductCategory Category { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal WholesalePrice { get; set; }
    public int MinOrderQuantity { get; set; }
}