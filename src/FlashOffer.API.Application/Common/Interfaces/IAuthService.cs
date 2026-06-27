using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface IAuthService
{
	Task<LoginResponse?> LoginAsync(LoginRequest request);
	Task LogoutAsync(string token);
	Task<LoginResponse?> RefreshTokenAsync(string token);
	Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
	Task ForgotPasswordAsync(ForgotPasswordRequest request);
	Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
	Task<UserInfoResponse?> GetUserByIdAsync(Guid userId);
}