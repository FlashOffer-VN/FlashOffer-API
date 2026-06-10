using FlashOffer.API.Domain;

namespace FlashOffer.API.Domain.Entities;

public class OfferRequest : BaseEntity
{
	public string SelectedOffer { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string Zalo { get; set; } = string.Empty;
	public string? Email { get; set; }
	public bool IsOfferSent { get; set; }
}