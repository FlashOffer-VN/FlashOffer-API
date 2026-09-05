using AutoMapper;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Application.DTOs.responses;

public class AuthAuditLogDto : IMapFrom<AuthAuditLog>
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string? Username { get; set; }
    public string Action { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? OperatingSystem { get; set; }
    public string? BrowserName { get; set; }
    public string? DeviceType { get; set; }
    public string? Detail { get; set; }
    public DateTime Timestamp { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<AuthAuditLog, AuthAuditLogDto>();
}