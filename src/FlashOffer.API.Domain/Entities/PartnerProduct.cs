// PartnerProduct.cs
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Domain.Entities;

public class PartnerProduct : BaseEntity
{
    public Guid PartnerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ProductCategory Category { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal WholesalePrice { get; set; }
    public int MinOrderQuantity { get; set; }

    // Navigation
    public virtual Partner Partner { get; set; } = null!;
}