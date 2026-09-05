// PartnerCommission.cs
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Domain.Entities;

public class PartnerCommission : BaseEntity
{
    public string? PartnerCommissionCode { get; set; }
    public Guid PartnerId { get; set; }
    public CommissionType Type { get; set; }
    public decimal Rate { get; set; }
    public decimal? MinOrderValue { get; set; }
    public decimal? MaxCommission { get; set; }
    public string? SpecialConditions { get; set; }

    // Navigation
    public virtual Partner Partner { get; set; } = null!;
}