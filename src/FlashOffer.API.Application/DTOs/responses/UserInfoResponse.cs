// Application/DTOs/responses/UserInfoResponse.cs
namespace FlashOffer.API.Application.DTOs.responses;

public class UserInfoResponse
{
	public Guid Id { get; set; }
	public string Username { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string? Phone { get; set; }
	public string Role { get; set; } = string.Empty;
	public bool IsActive { get; set; }
	public DateTime? LastLoginAt { get; set; }
}