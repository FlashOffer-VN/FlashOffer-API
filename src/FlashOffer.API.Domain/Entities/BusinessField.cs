namespace FlashOffer.API.Domain.Entities;

public class BusinessField : BaseEntity
{
    public string? BusinessFieldCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public string? Aliases { get; set; } // JSON array: ["CNTT", "IT"]
    public bool IsActive { get; set; } = true;
}