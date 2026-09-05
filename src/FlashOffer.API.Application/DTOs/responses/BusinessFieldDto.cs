using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using AutoMapper;

namespace FlashOffer.API.Application.DTOs.Responses;

public class BusinessFieldDto : IMapFrom<BusinessField>
{
    public Guid Id { get; set; }
    public string? BusinessFieldCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Aliases { get; set; }
    public bool IsActive { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<BusinessField, BusinessFieldDto>();
    }
}