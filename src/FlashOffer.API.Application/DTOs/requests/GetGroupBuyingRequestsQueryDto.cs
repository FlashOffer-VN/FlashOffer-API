namespace FlashOffer.API.Application.DTOs.requests;

public class GetGroupBuyingRequestsQueryDto
{
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 20;
	public string? Status { get; set; }
	public string? Search { get; set; }
	public string? SortBy { get; set; }    // VD: "CreatedAt"
	public string? SortOrder { get; set; } // "asc" | "desc"
}