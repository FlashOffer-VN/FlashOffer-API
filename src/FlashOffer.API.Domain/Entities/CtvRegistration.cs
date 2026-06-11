namespace FlashOffer.API.Domain.Entities;

public class CtvRegistration : BaseEntity
{
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Zalo { get; set; }
	public string? Email { get; set; }
	public string? SalesChannel { get; set; }
	public string? Experience { get; set; }
	public bool IsApproved { get; set; }
}