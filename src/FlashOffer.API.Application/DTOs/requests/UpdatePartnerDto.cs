// UpdatePartnerDto.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.Requests;

/// <summary>
/// Cập nhật đối tác — partial update: field nào null thì giữ nguyên giá trị cũ.
/// Không cho đổi PartnerCode, Status (có endpoint riêng), UserId, ReferralCode.
/// </summary>
public class UpdatePartnerDto : IMapFrom<Partner>
{
    // Thông tin cá nhân
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Position { get; set; }

    // Thông tin doanh nghiệp
    public string? CompanyName { get; set; }
    public string? CompanyTax { get; set; }
    public string? CompanyAddress { get; set; }
    public string? CompanyWebsite { get; set; }
    public BusinessType? BusinessType { get; set; }
    public CompanySize? CompanySize { get; set; }
    public string? Note { get; set; }

    /// <summary>Lĩnh vực kinh doanh. Chỉ cập nhật khi có giá trị.</summary>
    public Guid? BusinessFieldId { get; set; }

    /// <summary>
    /// Danh sách sản phẩm/dịch vụ. null = giữ nguyên danh sách cũ.
    /// Có giá trị = thay thế toàn bộ (sản phẩm cũ bị xóa mềm).
    /// </summary>
    public List<ProductDto>? Products { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdatePartnerDto, Partner>()
            .ForMember(dest => dest.Products, opt => opt.Ignore()) // Xử lý riêng trong service
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
}
