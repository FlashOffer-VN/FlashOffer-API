namespace FlashOffer.API.Domain.Entities;

/// <summary>
/// Audit log cho các thao tác đăng nhập / đăng ký / bảo mật tài khoản.
/// Bảng append-only, không kế thừa BaseEntity để không bị soft-delete / global query filter.
/// </summary>
public class AuthAuditLog
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
}
