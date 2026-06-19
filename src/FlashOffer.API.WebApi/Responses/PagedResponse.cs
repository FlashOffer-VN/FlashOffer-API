using FlashOffer.API.Domain.Models;

namespace FlashOffer.API.WebApi.Responses;

public class PagedResponse<T>
{
	public bool Success { get; set; }
	public string Message { get; set; } = string.Empty;
	public List<T> Data { get; set; } = new();
	public int PageNumber { get; set; }
	public int TotalPages { get; set; }
	public int TotalCount { get; set; }
	public bool HasPreviousPage { get; set; }
	public bool HasNextPage { get; set; }
	public DateTime Timestamp { get; set; }

	public PagedResponse()
	{
		Timestamp = DateTime.UtcNow;
	}

	public static PagedResponse<T> Ok(PagedList<T> pagedData, string message = "Success")
	{
		return new PagedResponse<T>
		{
			Success = true,
			Message = message,
			Data = pagedData.Items,
			PageNumber = pagedData.PageNumber,
			TotalPages = pagedData.TotalPages,
			TotalCount = pagedData.TotalCount,
			HasPreviousPage = pagedData.HasPreviousPage,
			HasNextPage = pagedData.HasNextPage,
			Timestamp = DateTime.UtcNow
		};
	}
}