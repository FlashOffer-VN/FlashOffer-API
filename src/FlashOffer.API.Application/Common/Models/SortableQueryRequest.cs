namespace FlashOffer.API.Application.Common.Models;

/// <summary>
/// Base request chuẩn cho query phân trang + sort động theo chuỗi.
/// DTO query mới có thể kế thừa lớp này.
/// </summary>
public class SortableQueryRequest : PagedRequest
{
    public SortableQueryRequest()
    {
        // Giữ nguyên default cũ (20) — không đổi hành vi của các DTO đang kế thừa lớp này.
        // Base PagedRequest mặc định 10, nên phải set lại tường minh.
        PageSize = 20;
    }

    public string? SortBy { get; set; }     // VD: "CreatedAt"
    public string? SortOrder { get; set; }  // "asc" | "desc"
}
