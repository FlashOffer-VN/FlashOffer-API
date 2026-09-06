// src/FlashOffer.API.Application/DTOs/requests/PurchaseRequestQueryDto.cs
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.DTOs.requests;

public class PurchaseRequestQueryDto
{
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 20;
	public PurchaseRequestStatus? Status { get; set; }
	public string? Search { get; set; }
	public string? SortBy { get; set; }    // VD: "CreatedAt"
	public string? SortOrder { get; set; } // "asc" | "desc"
}