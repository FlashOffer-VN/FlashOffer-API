using FlashOffer.API.Application.DTOs.Responses;
using MediatR;

namespace FlashOffer.API.Application.Features.Reports.Queries;

/// <summary>
/// Truy vấn báo cáo tổng quan hệ thống theo khoảng thời gian
/// </summary>
public class GetReportOverviewQuery : IRequest<ReportOverviewDto>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}