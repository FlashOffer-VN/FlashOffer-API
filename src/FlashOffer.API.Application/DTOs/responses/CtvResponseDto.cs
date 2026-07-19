using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.Responses;

public class CtvResponseDto : IMapFrom<CtvRegistration>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Zalo { get; set; }
    public string? Email { get; set; }
    public SalesChannel? SalesChannel { get; set; }
    public string? Experience { get; set; }
    public CTVRegistrationStatus Status { get; set; }
    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CtvRegistration, CtvResponseDto>();
    }
}

public class CtvDetailResponseDto : CtvResponseDto
{
    public UserInfoDto? User { get; set; }

    public new void Mapping(Profile profile)
    {
        profile.CreateMap<CtvRegistration, CtvDetailResponseDto>()
            .IncludeBase<CtvRegistration, CtvResponseDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
    }
}

public class UserInfoDto : IMapFrom<User>
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserInfoDto>();
    }
}