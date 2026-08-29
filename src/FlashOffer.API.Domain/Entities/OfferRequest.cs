// src/FlashOffer.API.Domain/Entities/OfferRequest.cs
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Domain.Entities;

public class OfferRequest : BaseEntity
{
    public Guid UserId { get; set; }

    // Product information
    public string ProductName { get; set; } = string.Empty;
    public string? ProductLink { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal? ExpectedPrice { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;

    // User information
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Zalo { get; set; }
    public string? Email { get; set; }
    public string? Note { get; set; }

    // Status
    public OfferStatus Status { get; set; } = OfferStatus.Pending;
    public bool IsOfferSent { get; set; } // Keep for backward compatibility

    // Navigation
    public virtual User User { get; set; } = null!;
}