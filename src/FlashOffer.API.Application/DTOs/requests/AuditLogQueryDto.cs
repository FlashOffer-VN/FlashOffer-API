namespace FlashOffer.API.Application.DTOs.requests;

public class AuditLogQueryDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? EntityName { get; set; }
    public string? Action { get; set; }
    public string? ActorId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}