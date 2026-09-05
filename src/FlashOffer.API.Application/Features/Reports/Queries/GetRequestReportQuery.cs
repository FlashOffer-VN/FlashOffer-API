using FlashOffer.API.Application.DTOs.Responses;
using MediatR;

namespace FlashOffer.API.Application.Features.Reports.Queries;

/// <summary>
/// Truy vấn báo cáo yêu cầu (mua hàng / mua nhóm / báo giá) phân theo trạng thái
/// </summary>
public class GetRequestReportQuery : IRequest<RequestReportDto>
{
    /// <summary>Loại yêu cầu: purchase | groupbuying | offer (rỗng = tất cả)</summary>
    public string? Type { get; set; }
}