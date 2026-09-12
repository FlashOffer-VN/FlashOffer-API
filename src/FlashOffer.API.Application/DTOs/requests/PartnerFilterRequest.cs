using FlashOffer.API.Application.Common.Models;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.Requests;

public class PartnerFilterRequest : PagedRequest
{
    public string? Search { get; set; }
    public PartnerStatus? Status { get; set; }

    /// <summary>
    /// true  = chỉ lấy đối tác đã xóa mềm (bỏ qua global soft-delete filter).
    /// false/null = danh sách đang hoạt động như bình thường.
    /// </summary>
    public bool? IsDeleted { get; set; }

    public string? SortBy { get; set; }    // VD: "CreatedAt"
    public string? SortOrder { get; set; } // "asc" | "desc"
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
