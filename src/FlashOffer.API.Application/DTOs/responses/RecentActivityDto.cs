namespace FlashOffer.API.Application.DTOs.Responses;

public class RecentActivityDto
{
	public string Type { get; set; } = string.Empty; // purchase_request, group_buying_request, offer_request, ctv_registration
	public string Action { get; set; } = string.Empty; // created, registered, updated
	public string UserName { get; set; } = string.Empty;
	public DateTime Timestamp { get; set; }
}