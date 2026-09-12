using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using AutoMapper;

namespace FlashOffer.API.Application.DTOs.Responses;

public class PartnerResponseDto : IMapFrom<Partner>
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
    /// <summary>Id lĩnh vực kinh doanh (BusinessField).</summary>
    public Guid? BusinessFieldId { get; set; }
    /// <summary>Tên lĩnh vực kinh doanh — lấy từ bảng BusinessFields.</summary>
    public string? BusinessFieldName { get; set; }
    public string? ReferralCode { get; set; }
    public string? Note { get; set; }
    public PartnerStatus Status { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Partner, PartnerResponseDto>()
            // Partner không có cột tên lĩnh vực denormalized → bắt buộc Include nav khi query.
            .ForMember(dest => dest.BusinessFieldName,
                opt => opt.MapFrom(src => src.BusinessField != null ? src.BusinessField.Name : null));
    }
}