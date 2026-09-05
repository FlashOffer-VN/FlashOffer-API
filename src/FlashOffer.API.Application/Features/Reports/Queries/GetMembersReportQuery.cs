using FlashOffer.API.Application.DTOs.Responses;
using MediatR;

namespace FlashOffer.API.Application.Features.Reports.Queries;

/// <summary>
/// Truy vấn báo cáo thành viên (đối tác + cộng tác viên)
/// </summary>
public class GetMembersReportQuery : IRequest<MembersReportDto>
{
}