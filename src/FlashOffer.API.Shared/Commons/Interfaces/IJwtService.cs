using System.Security.Claims;

namespace FlashOffer.API.Shared.Common.Interfaces;

public interface IJwtService
{
	string GenerateToken(string userId, string username, IEnumerable<string> roles);
	ClaimsPrincipal? ValidateToken(string token);
	Task<ClaimsPrincipal?> ValidateTokenWithUserAsync(string token);
	void BlacklistToken(string token);
	bool IsTokenBlacklisted(string token);
	Task<string?> RefreshTokenAsync(string token);
}