using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Application.DTOs.responses;

public class AuthorDto : IMapFrom<User>
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Role { get; set; }
    public bool IsVerified { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, AuthorDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.Avatar, opt => opt.Ignore()); // Sẽ xử lý sau
    }
}