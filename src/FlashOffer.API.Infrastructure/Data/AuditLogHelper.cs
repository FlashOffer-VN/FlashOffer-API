using FlashOffer.API.Domain.Attributes;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Shared.Common.Helpers;
using FlashOffer.API.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using System.Text.Json;

namespace FlashOffer.API.Infrastructure.Data;

/// <summary>
/// Tạo các dòng AuditLog từ ChangeTracker của DbContext.
/// Snapshot + Diff JSON: OldValues, NewValues, ChangedProperties.
/// </summary>
public static class AuditLogHelper
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = false,
        MaxDepth = 16
    };

    /// <summary>
    /// Duyệt ChangeTracker và tạo danh sách AuditLog cho mọi entity thay đổi.
    /// Mỗi entry được bọc try/catch — audit hỏng không làm sập transaction chính.
    /// </summary>
    public static List<AuditLog> CreateAuditLogs(
        ChangeTracker changeTracker,
        string? actorId,
        string? actorName,
        string? ipAddress,
        string? userAgent)
    {
        var auditLogs = new List<AuditLog>();

        // Parse thông tin thiết bị một lần cho toàn bộ request
        var deviceInfo = UserAgentParser.Parse(userAgent);

        var entries = changeTracker.Entries<object>()
            .Where(e => e.State != EntityState.Unchanged
                        && e.State != EntityState.Detached)
            .ToList();

        foreach (var entry in entries)
        {
            try
            {
                var auditLog = CreateAuditLog(entry, actorId, actorName, ipAddress, deviceInfo);
                if (auditLog != null)
                    auditLogs.Add(auditLog);
            }
            catch
            {
                // Audit lỗi ở 1 entity thì bỏ qua entity đó, không ảnh hưởng transaction.
            }
        }

        return auditLogs;
    }

    private static AuditLog? CreateAuditLog(
        EntityEntry entry,
        string? actorId,
        string? actorName,
        string? ipAddress,
        UserAgentInfo deviceInfo)
    {
        var entityType = entry.Metadata.ClrType;

        // Không audit chính bảng audit (tránh vòng lặp)
        if (entityType == typeof(AuditLog) || entityType == typeof(AuthAuditLog))
            return null;

        // Chỉ audit entity trong namespace Entities
        if (!entityType.Namespace?.StartsWith("FlashOffer.API.Domain.Entities", StringComparison.Ordinal) ?? true)
            return null;

        // Lấy Id của entity (entity đều có property Id)
        var entityIdProperty = entry.Property("Id");
        if (entityIdProperty == null || entityIdProperty.CurrentValue is not Guid entityId)
            return null;

        // Nhận diện soft-delete: GenericRepository.DeleteAsync set IsDeleted=true rồi Update
        // -> entry ở trạng thái Modified, nhưng thực chất là xóa. Coi như action Delete.
        var isSoftDelete = false;
        if (entry.State == EntityState.Modified)
        {
            var isDeletedProp = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "IsDeleted");
            isSoftDelete = isDeletedProp != null
                && isDeletedProp.OriginalValue is bool o && o == false
                && isDeletedProp.CurrentValue is bool c && c == true;
        }

        var action = entry.State switch
        {
            EntityState.Added => AuditAction.Create,
            EntityState.Deleted => AuditAction.Delete,
            EntityState.Modified when isSoftDelete => AuditAction.Delete,
            EntityState.Modified => AuditAction.Update,
            _ => string.Empty
        };

        if (string.IsNullOrEmpty(action))
            return null;

        var isDelete = action == AuditAction.Delete;

        var oldValues = new Dictionary<string, object?>();
        var newValues = new Dictionary<string, object?>();
        var changedProperties = new List<Dictionary<string, object?>>();

        foreach (var property in entry.Properties)
        {
            if (property.Metadata.IsShadowProperty())
                continue;

            var propertyInfo = property.Metadata.PropertyInfo;
            if (propertyInfo == null)
                continue;

            // Bỏ qua property được đánh dấu [AuditIgnore]
            if (propertyInfo.GetCustomAttribute<AuditIgnoreAttribute>() != null)
                continue;

            var displayName = property.Metadata.Name;
            var currentValue = property.CurrentValue;
            var originalValue = property.OriginalValue;

            if (isDelete)
            {
                // Với soft-delete (Modified + IsDeleted=true), IsDeleted nên phản ánh trạng thái
                // TRƯỚC khi xóa (false). Các property khác lấy giá trị hiện tại.
                oldValues[displayName] =
                    (isSoftDelete && displayName == "IsDeleted") ? originalValue : currentValue;
                continue;
            }

            switch (entry.State)
            {
                case EntityState.Added:
                    newValues[displayName] = currentValue;
                    break;

                case EntityState.Modified:
                    oldValues[displayName] = originalValue;
                    newValues[displayName] = currentValue;

                    // Chỉ đưa vào diff property có GIÁ TRỊ thực sự khác nhau.
                    // Lưu ý: entity được Update() toàn bộ sẽ có IsModified=true cho mọi property,
                    // nên không tin vào IsModified — phải so sánh old/new thực tế.
                    if (!ValuesEqual(originalValue, currentValue))
                    {
                        changedProperties.Add(new Dictionary<string, object?>
                        {
                            ["property"] = displayName,
                            ["oldValue"] = originalValue,
                            ["newValue"] = currentValue
                        });
                    }
                    break;

                case EntityState.Deleted:
                    oldValues[displayName] = currentValue;
                    break;
            }
        }

        var log = new AuditLog
        {
            Id = Guid.NewGuid(),
            EntityName = entityType.Name,
            EntityId = entityId,
            Action = action,
            ActorId = actorId,
            ActorName = actorName,
            IpAddress = ipAddress,
            OperatingSystem = deviceInfo.OperatingSystem,
            BrowserName = deviceInfo.BrowserName,
            DeviceType = deviceInfo.DeviceType,
            Timestamp = DateTime.UtcNow,
            NewValues = Serialize(newValues),
            OldValues = Serialize(oldValues)
        };

        if (changedProperties.Count > 0)
            log.ChangedProperties = Serialize(changedProperties);

        return log;
    }

    private static string? Serialize(Dictionary<string, object?> values)
    {
        if (values.Count == 0)
            return null;

        return JsonSerializer.Serialize(values, JsonOptions);
    }

    private static string? Serialize(List<Dictionary<string, object?>> values)
    {
        if (values.Count == 0)
            return null;

        return JsonSerializer.Serialize(values, JsonOptions);
    }

    /// <summary>
    /// So sánh giá trị old/new dùng JSON để tránh lệch kiểu (enum vs int, decimal vs double,...).
    /// </summary>
    private static bool ValuesEqual(object? a, object? b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;

        return JsonSerializer.Serialize(a, JsonOptions) == JsonSerializer.Serialize(b, JsonOptions);
    }
}