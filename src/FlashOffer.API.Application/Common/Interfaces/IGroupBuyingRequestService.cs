using FlashOffer.API.Application.Common.Models;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface IGroupBuyingRequestService
{
	Task<GroupBuyingRequestResponseDto> CreateAsync(CreateGroupBuyingRequestDto request);
	Task<PagedList<GroupBuyingRequestResponseDto>> GetPagedAsync(GetGroupBuyingRequestsQueryDto query);
}