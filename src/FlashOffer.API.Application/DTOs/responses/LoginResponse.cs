
// LoginResponse.cs
namespace FlashOffer.API.Application.DTOs.responses;

public class LoginResponse
{
	public string Token { get; set; } = string.Empty;
	public DateTime ExpiresAt { get; set; }
	public string Username { get; set; } = string.Empty;
}