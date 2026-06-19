using System.Security.Claims;

namespace FlashOffer.API.Shared.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(string userId, string username, IEnumerable<string> roles);
    ClaimsPrincipal? ValidateToken(string token);
}
