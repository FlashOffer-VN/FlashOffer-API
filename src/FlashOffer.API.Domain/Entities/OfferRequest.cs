using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Domain.Entities;

public class OfferRequest : BaseEntity
{
	public Guid UserId { get; set; } // Thêm FK
	public string SelectedOffer { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string Zalo { get; set; } = string.Empty;
	public string? Email { get; set; }
	public OfferStatus Status { get; set; } = OfferStatus.Pending; // Thay IsOfferSent
	public bool IsOfferSent { get; set; } // Giữ lại để tương thích

	public virtual User User { get; set; } = null!;
}