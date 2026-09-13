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

    // Sản phẩm KHÔNG nằm ở đây — quản lý qua API riêng:
    //   POST   /partners/{id}/products
    //   PUT    /partners/{id}/products/{productId}
    //   DELETE /partners/{id}/products/{productId}

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdatePartnerDto, Partner>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
}
