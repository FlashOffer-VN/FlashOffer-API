using FlashOffer.API.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashOffer.API.Domain.Entities;

[Table("Companies")]
public class Company : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? TaxCode { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public BusinessType? BusinessType { get; set; }
    public CompanySize? CompanySize { get; set; }

    public Guid? BusinessFieldId { get; set; }

    // Navigation
    public virtual BusinessField? BusinessField { get; set; }
}
