// src/FlashOffer.API.Application/DTOs/requests/CreatePurchaseRequestDto.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Application.DTOs.requests;

public class CreatePurchaseRequestDto : IMapFrom<PurchaseRequest>
{
	public string ProductName { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public decimal? ExpectedPrice { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Email { get; set; }
	public string? Note { get; set; }

	public void Mapping(Profile profile)
	{
		profile.CreateMap<CreatePurchaseRequestDto, PurchaseRequest>();
	}
}