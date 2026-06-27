using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Domain.Entities;

public class User : BaseEntity
{
	public string Username { get; set; } = string.Empty;
	public string? PasswordHash { get; set; } // Cho phép null
	public string FullName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string? Phone { get; set; }
	public bool IsActive { get; set; } = true;
	public DateTime? LastLoginAt { get; set; }
	public UserRole Role { get; set; } = UserRole.Customer;
}