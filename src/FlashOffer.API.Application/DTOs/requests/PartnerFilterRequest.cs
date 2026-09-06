using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.Requests;

public class PartnerFilterRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public PartnerStatus? Status { get; set; }
    public string? SortBy { get; set; }    // VD: "CreatedAt"
    public string? SortOrder { get; set; } // "asc" | "desc"
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}