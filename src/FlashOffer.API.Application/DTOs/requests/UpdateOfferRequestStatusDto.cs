// src/FlashOffer.API.Application/DTOs/requests/UpdateOfferRequestStatusDto.cs
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.requests;

public class UpdateOfferRequestStatusDto
{
	public OfferStatus Status { get; set; }
}