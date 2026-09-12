using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.Requests;

/// <summary>
/// Cập nhật CTV — partial update: field nào null thì giữ nguyên giá trị cũ.
/// Không cho đổi CollaboratorCode, Status (có endpoint riêng), UserId, Level,
/// ParentCollaboratorId (thuộc cấu trúc cây, đổi sẽ phá quan hệ đa cấp).
/// </summary>
public class UpdateCollaboratorDto : IMapFrom<Collaborator>
{
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Zalo { get; set; }
    public string? Email { get; set; }
    public string? Position { get; set; }
    public string? Skills { get; set; }
    public string? Interests { get; set; }
    public string? Goals { get; set; }
    public SalesChannel? SalesChannel { get; set; }
    public string? Experience { get; set; }

    // Thông tin doanh nghiệp
    public string? Address { get; set; }
    public string? BusinessName { get; set; }
    public int? BusinessSize { get; set; }
    public string? Website { get; set; }

    /// <summary>Id lĩnh vực — ưu tiên hơn BusinessFieldName.</summary>
    public Guid? BusinessFieldId { get; set; }
    /// <summary>Tên lĩnh vực — fallback find-or-create cho client chưa gửi Id.</summary>
    public string? BusinessFieldName { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdateCollaboratorDto, Collaborator>()
            // BusinessFieldId/BusinessFieldName được xử lý riêng trong service để
            // ghi lại đúng cột denormalized BusinessFieldName.
            .ForMember(dest => dest.BusinessFieldId, opt => opt.Ignore())
            .ForMember(dest => dest.BusinessFieldName, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
}
