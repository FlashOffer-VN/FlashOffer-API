using FlashOffer.API.Application.DTOs.Responses;
using MediatR;

namespace FlashOffer.API.Application.Features.Reports.Queries;

/// <summary>
/// Truy vấn báo cáo xu hướng tăng trưởng theo ngày
/// </summary>
public class GetReportTrendQuery : IRequest<ReportTrendDto>
{
    /// <summary>Số ngày gần đây (mặc định 30 ngày)</summary>
    public int Days { get; set; } = 30;
}