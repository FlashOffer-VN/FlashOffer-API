// Partner.cs
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Domain.Entities;

public class Partner : BaseEntity
{
    public Guid UserId { get; set; }
    public string PartnerCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyTax { get; set; } = string.Empty;
    public string CompanyAddress { get; set; } = string.Empty;
    public BusinessType BusinessType { get; set; }
    public CompanySize CompanySize { get; set; }
    public string? CompanyWebsite { get; set; }
    public string? ReferralCode { get; set; }
    public string? Note { get; set; }
    public PartnerStatus Status { get; set; } = PartnerStatus.Pending;
    public DateTime? ApprovedAt { get; set; }

    // Navigation
    public virtual User User { get; set; } = null!;
    public virtual ICollection<PartnerProduct> Products { get; set; } = new List<PartnerProduct>();
    public virtual PartnerCommission? Commission { get; set; }
}