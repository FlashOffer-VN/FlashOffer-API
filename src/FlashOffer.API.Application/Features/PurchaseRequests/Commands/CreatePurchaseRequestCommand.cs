using AutoMapper;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using MediatR;

namespace FlashOffer.API.Application.Features.PurchaseRequests.Commands;

public class CreatePurchaseRequestCommand : IRequest<PurchaseRequestResponseDto>
{
	public string ProductName { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public decimal? ExpectedPrice { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Email { get; set; }
	public string? Note { get; set; }
}

public class CreatePurchaseRequestMapping : Profile
{
	public CreatePurchaseRequestMapping()
	{
		CreateMap<CreatePurchaseRequestCommand, PurchaseRequest>();
	}
}