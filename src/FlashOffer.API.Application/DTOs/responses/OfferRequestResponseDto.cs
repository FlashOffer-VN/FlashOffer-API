using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Application.DTOs.responses;

public class OfferRequestResponseDto : IMapFrom<OfferRequest>
{
	public Guid Id { get; set; }
	public string? OfferRequestCode { get; set; }
	public string SelectedOffer { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string Zalo { get; set; } = string.Empty;
	public string? Email { get; set; }
	public bool IsOfferSent { get; set; }
	public DateTime CreatedAt { get; set; }

	public void Mapping(Profile profile)
	{
		profile.CreateMap<OfferRequest, OfferRequestResponseDto>();
	}
}