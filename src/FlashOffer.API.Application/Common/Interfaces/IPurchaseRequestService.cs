using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface IPurchaseRequestService
{
	Task<PurchaseRequestResponseDto> CreateAsync(CreatePurchaseRequestDto request);
}