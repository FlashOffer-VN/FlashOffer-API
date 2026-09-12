using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.Responses;

public class CollaboratorResponseDto : IMapFrom<Collaborator>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Zalo { get; set; }
    public string? Email { get; set; }
    public string? Position { get; set; }
    public string? Skills { get; set; }
    public string? Interests { get; set; }
    public string? Goals { get; set; }
    public SalesChannel? SalesChannel { get; set; }
    public string? Experience { get; set; }
    public string? Address { get; set; }
    /// <summary>Id lĩnh vực kinh doanh (BusinessField).</summary>
    public Guid? BusinessFieldId { get; set; }
    /// <summary>Tên lĩnh vực kinh doanh.</summary>
    public string? BusinessFieldName { get; set; }
    public int Level { get; set; }
    public string? CollaboratorCode { get; set; }
    public CollaboratorStatus Status { get; set; }
    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<Collaborator, CollaboratorResponseDto>()
            // Ưu tiên tên từ bảng BusinessFields (đổi tên vẫn đúng), fallback cột denormalized
            // để các bản ghi cũ / query không Include nav vẫn có dữ liệu.
            .ForMember(dest => dest.BusinessFieldName,
                opt => opt.MapFrom(src => src.BusinessField != null ? src.BusinessField.Name : src.BusinessFieldName));
}