using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Models;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface IPurchaseRequestService
{
	Task<PurchaseRequestResponseDto> CreateAsync(CreatePurchaseRequestDto request);
	Task<PagedList<PurchaseRequestResponseDto>> GetPagedAsync(PurchaseRequestQueryDto query);
}