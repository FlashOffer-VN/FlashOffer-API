// Application/DTOs/responses/PurchaseRequestStatusResponseDto.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.responses;

public class PurchaseRequestStatusResponseDto : IMapFrom<PurchaseRequest>
{
	public Guid Id { get; set; }
	public PurchaseRequestStatus Status { get; set; }
	public DateTime UpdatedAt { get; set; }

	public void Mapping(Profile profile)
		=> profile.CreateMap<PurchaseRequest, PurchaseRequestStatusResponseDto>();
}