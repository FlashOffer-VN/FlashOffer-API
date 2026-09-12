namespace FlashOffer.API.Application.Common.Models;

/// <summary>
/// Base request chuẩn cho MỌI query có phân trang.
/// Query/DTO mới cần phân trang thì kế thừa lớp này, KHÔNG khai lại PageNumber/PageSize.
///
/// Lưu ý: một số DTO cũ (OfferRequestQueryDto, PurchaseRequestQueryDto,
/// CtvRegistrationQueryDto, GetGroupBuyingRequestsQueryDto) đang dùng tên param `Page`
/// thay vì `PageNumber`. Chúng chưa được chuyển sang base vì đổi tên param sẽ phá client.
/// Chuẩn của base là `pageNumber`/`pageSize` — không copy theo kiểu `Page`.
///
/// Cần thêm sort động thì kế thừa <see cref="SortableQueryRequest"/> (đã bao gồm lớp này).
/// </summary>
public class PagedRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
