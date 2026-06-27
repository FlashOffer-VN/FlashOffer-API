using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Application.DTOs.Responses;

public class DashboardStatsDto
{
	public int TotalPurchaseRequests { get; set; }
	public int PendingPurchaseRequests { get; set; }
	public int TotalGroupBuyingRequests { get; set; }
	public int PendingGroupBuyingRequests { get; set; }
	public int TotalOfferRequests { get; set; }
	public int PendingOfferRequests { get; set; }
	public int TotalCTVRegistrations { get; set; }
	public int PendingCTVRegistrations { get; set; }
	public List<RecentActivityDto> RecentActivities { get; set; } = new();
}