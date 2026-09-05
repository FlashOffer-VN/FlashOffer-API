using FlashOffer.API.Application.Common.Models;

namespace FlashOffer.API.Application.DTOs.requests;

public class AuthAuditLogQueryDto : SortableQueryRequest
{
    public string? Username { get; set; }
    public string? Action { get; set; }
    public bool? IsSuccess { get; set; }
    public string? OperatingSystem { get; set; }
    public string? DeviceType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}