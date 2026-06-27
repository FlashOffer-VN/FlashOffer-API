using FlashOffer.API.Application.DTOs.Responses;
using MediatR;

namespace FlashOffer.API.Application.Features.Dashboard.Queries;

public class GetDashboardQuery : IRequest<DashboardStatsDto>
{
}