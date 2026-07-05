// PartnerRegisterRequest.cs
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using AutoMapper;

namespace FlashOffer.API.Application.DTOs.Requests;

public class PartnerRegisterRequest : IMapFrom<Partner>
{
    // Step 1: Personal Info
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;

    // Step 2: Business Info
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyTax { get; set; } = string.Empty;
    public string CompanyAddress { get; set; } = string.Empty;
    public BusinessType BusinessType { get; set; }
    public string? CompanyWebsite { get; set; }
    public CompanySize CompanySize { get; set; }
    public string? ReferralCode { get; set; }

    // Step 3: Products & Commission
    public List<ProductDto> Products { get; set; } = new();
    public CommissionType CommissionType { get; set; }
    public decimal CommissionRate { get; set; }
    public decimal? MinOrderValue { get; set; }
    public decimal? MaxCommission { get; set; }
    public string? SpecialConditions { get; set; }

    // Step 4: Confirmation
    public bool AgreeTerms { get; set; }
    public string? Note { get; set; }

    public void Mapping(Profile profile)
    {
        // Map PartnerRegisterRequest -> Partner
        profile.CreateMap<PartnerRegisterRequest, Partner>()
            .ForMember(dest => dest.PartnerCode,
                opt => opt.MapFrom(src => $"KINDI-{Guid.NewGuid():N}".Substring(0, 8).ToUpper()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PartnerStatus.Pending))
            .ForMember(dest => dest.Products, opt => opt.Ignore()) // Xử lý riêng
            .ForMember(dest => dest.Commission, opt => opt.Ignore()); // Xử lý riêng

        // Map ProductDto -> PartnerProduct
        profile.CreateMap<ProductDto, PartnerProduct>();

        // Map PartnerRegisterRequest -> PartnerCommission
        profile.CreateMap<PartnerRegisterRequest, PartnerCommission>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.CommissionType))
            .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.CommissionRate))
            .ForMember(dest => dest.MinOrderValue, opt => opt.MapFrom(src => src.MinOrderValue))
            .ForMember(dest => dest.MaxCommission, opt => opt.MapFrom(src => src.MaxCommission))
            .ForMember(dest => dest.SpecialConditions, opt => opt.MapFrom(src => src.SpecialConditions));
    }
}