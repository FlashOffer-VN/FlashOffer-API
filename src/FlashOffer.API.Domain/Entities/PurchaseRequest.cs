// src/FlashOffer.API.Domain/Entities/PurchaseRequest.cs
namespace FlashOffer.API.Domain.Entities;

public class PurchaseRequest : BaseEntity
{
	public string ProductName { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public decimal? ExpectedPrice { get; set; }
	public string FullName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string? Email { get; set; }
	public string? Note { get; set; }
	public string Status { get; set; } = "Pending";
}