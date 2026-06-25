using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Models;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface ICtvRegistrationService
{
	Task<CtvRegistrationResponseDto> CreateAsync(CreateCtvRegistrationDto dto);
	Task<PagedList<CtvRegistrationResponseDto>> GetPagedAsync(CtvRegistrationQueryDto query);
	Task<CtvRegistrationResponseDto> ApproveAsync(Guid id);
}