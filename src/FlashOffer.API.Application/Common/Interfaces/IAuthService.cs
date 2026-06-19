using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface IAuthService
{
	Task<LoginResponse?> LoginAsync(LoginRequest request);
}