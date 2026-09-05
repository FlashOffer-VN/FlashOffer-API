namespace FlashOffer.API.Application.Common.Models;

/// <summary>
/// Base request chuẩn cho query phân trang + sort động theo chuỗi.
/// DTO query mới có thể kế thừa lớp này.
/// </summary>
public class SortableQueryRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; }     // VD: "CreatedAt"
    public string? SortOrder { get; set; }  // "asc" | "desc"
}