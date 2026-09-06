// src/FlashOffer.API.Application/DTOs/responses/OfferRequestStatusResponseDto.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.responses;

public class OfferRequestStatusResponseDto : IMapFrom<OfferRequest>
{
	public Guid Id { get; set; }
	public string? OfferRequestCode { get; set; }
	public OfferStatus Status { get; set; }
	public DateTime UpdatedAt { get; set; }

	public void Mapping(Profile profile)
		=> profile.CreateMap<OfferRequest, OfferRequestStatusResponseDto>();
}