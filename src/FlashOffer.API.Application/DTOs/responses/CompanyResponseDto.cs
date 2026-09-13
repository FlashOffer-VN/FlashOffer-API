using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Application.DTOs.Responses;

public class CompanyResponseDto : IMapFrom<Company>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? TaxCode { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public string? BusinessFieldName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Company, CompanyResponseDto>()
            .ForMember(dest => dest.BusinessFieldName,
                opt => opt.MapFrom(src => src.BusinessField != null ? src.BusinessField.Name : null));
    }
}
