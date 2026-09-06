using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.Responses;

public class CtvResponseDto : IMapFrom<Collaborator>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? CollaboratorCode { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Zalo { get; set; }
    public string? Email { get; set; }
    public SalesChannel? SalesChannel { get; set; }
    public string? Experience { get; set; }
    public CollaboratorStatus Status { get; set; }
    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Collaborator, CtvResponseDto>();
    }
}

public class CtvDetailResponseDto : CtvResponseDto
{
    public UserInfoDto? User { get; set; }
    public BusinessInfoDto? BusinessInfo { get; set; }

    public new void Mapping(Profile profile)
    {
        profile.CreateMap<Collaborator, CtvDetailResponseDto>()
            .IncludeBase<Collaborator, CtvResponseDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.BusinessInfo, opt => opt.MapFrom(src => new BusinessInfoDto
            {
                CompanyName = src.BusinessName,
                CompanyAddress = src.Address,
                CompanyWebsite = src.Website,
                BusinessField = src.BusinessFieldName,
                CompanySize = src.BusinessSize.HasValue ? (CompanySize?)src.BusinessSize.Value : null
            }));
    }
}

public class UserInfoDto : IMapFrom<User>
{
    public Guid Id { get; set; }
    public string? UserCode { get; set; }
    public string? Username { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserInfoDto>();
    }
}