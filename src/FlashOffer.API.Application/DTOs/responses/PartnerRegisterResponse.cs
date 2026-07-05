// PartnerRegisterResponse.cs
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using AutoMapper;

namespace FlashOffer.API.Application.DTOs.Responses;

public class PartnerRegisterResponse : IMapFrom<Partner>
{
    public Guid Id { get; set; }
    public string PartnerCode { get; set; } = string.Empty;
    public PartnerStatus Status { get; set; }
    public DateTime RegisteredAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Partner, PartnerRegisterResponse>()
            .ForMember(dest => dest.RegisteredAt,
                opt => opt.MapFrom(src => src.CreatedAt));
    }
}