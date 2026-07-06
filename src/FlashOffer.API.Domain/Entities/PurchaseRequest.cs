using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Domain.Entities;

public class PurchaseRequest : BaseEntity
{
	public Guid UserId { get; set; } // Thêm FK
	public string ProductName { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public decimal? ExpectedPrice { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Email { get; set; }
	public string? Note { get; set; }
	public PurchaseRequestStatus Status { get; set; } = PurchaseRequestStatus.Pending;
	public string? AdminNote { get; set; }
	public Guid? AssignedTo { get; set; }
	public DateTime? ResolvedAt { get; set; }
	public string? Source { get; set; }

	public virtual User User { get; set; } = null!;
}