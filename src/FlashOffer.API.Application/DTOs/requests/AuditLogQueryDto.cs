using FlashOffer.API.Application.Common.Models;

namespace FlashOffer.API.Application.DTOs.requests;

public class AuditLogQueryDto : SortableQueryRequest
{
    public string? EntityName { get; set; }
    public string? Action { get; set; }
    public string? ActorId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}