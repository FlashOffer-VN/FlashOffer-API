namespace FlashOffer.API.Domain.Entities;

public class Admin : BaseEntity
{
	public string Username { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
	public string? FullName { get; set; }
	public string? Email { get; set; }
	public bool IsActive { get; set; } = true;
	public DateTime? LastLoginAt { get; set; }
}