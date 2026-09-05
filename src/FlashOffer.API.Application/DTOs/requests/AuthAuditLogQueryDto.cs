namespace FlashOffer.API.Application.DTOs.requests;

public class AuthAuditLogQueryDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Username { get; set; }
    public string? Action { get; set; }
    public bool? IsSuccess { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}