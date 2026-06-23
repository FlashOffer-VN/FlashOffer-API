// Application/DTOs/requests/UpdatePurchaseRequestStatusDto.cs
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Application.DTOs.requests;

public class UpdatePurchaseRequestStatusDto : IMapFrom<PurchaseRequest>
{
	public PurchaseRequestStatus Status { get; set; }
}