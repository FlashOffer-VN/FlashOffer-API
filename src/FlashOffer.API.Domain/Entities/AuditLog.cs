namespace FlashOffer.API.Domain.Entities;

/// <summary>
/// Audit log cho các thay đổi trên entity (Create/Update/Delete).
/// Bảng append-only, không kế thừa BaseEntity để không bị soft-delete / global query filter.
/// </summary>
public class AuditLog
{
    public Guid Id { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string? ActorId { get; set; }
    public string? ActorName { get; set; }
    public string? IpAddress { get; set; }
    public DateTime Timestamp { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? ChangedProperties { get; set; }
}
