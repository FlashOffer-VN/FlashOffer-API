using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using MediatR;

namespace FlashOffer.API.Application.Features.OfferRequests.Commands;

public class CreateOfferRequestCommand : IRequest<OfferRequestResponseDto>, IMapFrom<CreateOfferRequestDto>
{
	public string SelectedOffer { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string Zalo { get; set; } = string.Empty;
	public string? Email { get; set; }

	public void Mapping(Profile profile)
	{
		profile.CreateMap<CreateOfferRequestDto, CreateOfferRequestCommand>();
	}
}