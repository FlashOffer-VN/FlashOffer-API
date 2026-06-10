using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Application.DTOs.responses;

public class OfferRequestResponseDto : IMapFrom<OfferRequest>
{
	public Guid Id { get; set; }
	public string SelectedOffer { get; set; } = string.Empty;
	public bool IsOfferSent { get; set; }
	public DateTime CreatedAt { get; set; }

	public void Mapping(Profile profile)
	{
		profile.CreateMap<OfferRequest, OfferRequestResponseDto>();
	}
}