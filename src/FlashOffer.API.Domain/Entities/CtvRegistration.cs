using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Domain.Entities;

public class CtvRegistration : BaseEntity
{
	public Guid UserId { get; set; } // Thêm FK
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Zalo { get; set; }
	public string? Email { get; set; }
	public SalesChannel? SalesChannel { get; set; }
	public string? Experience { get; set; }
	public CTVRegistrationStatus Status { get; set; } = CTVRegistrationStatus.Pending; // Thay IsApproved
	public bool IsApproved { get; set; } // Giữ lại để tương thích
	public DateTime? ApprovedAt { get; set; }

	public virtual User User { get; set; } = null!;
}