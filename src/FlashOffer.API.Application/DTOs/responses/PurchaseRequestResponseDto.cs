// src/FlashOffer.API.Application/DTOs/responses/PurchaseRequestResponseDto.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.responses;

public class PurchaseRequestResponseDto : IMapFrom<PurchaseRequest>
{
	public Guid Id { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public decimal? ExpectedPrice { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Email { get; set; }
	public string? Note { get; set; }
	public PurchaseRequestStatus Status { get; set; }
	public string? AdminNote { get; set; }
	public string? Source { get; set; }
	public DateTime CreatedAt { get; set; }

	public void Mapping(Profile profile)
	{
		profile.CreateMap<PurchaseRequest, PurchaseRequestResponseDto>();
	}
}