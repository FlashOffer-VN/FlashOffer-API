// src/FlashOffer.API.Application/DTOs/responses/PurchaseRequestResponseDto.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Application.DTOs.responses;

public class PurchaseRequestResponseDto : IMapFrom<PurchaseRequest>
{
	public Guid Id { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public string Status { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }

	public void Mapping(Profile profile)
	{
		profile.CreateMap<PurchaseRequest, PurchaseRequestResponseDto>();
	}
}