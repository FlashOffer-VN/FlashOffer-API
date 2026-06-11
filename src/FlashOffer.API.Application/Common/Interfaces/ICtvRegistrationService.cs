using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface ICtvRegistrationService
{
	Task<CtvRegistrationResponseDto> CreateAsync(CreateCtvRegistrationDto dto);
}