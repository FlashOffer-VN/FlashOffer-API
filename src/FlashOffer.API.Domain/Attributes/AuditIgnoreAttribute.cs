namespace FlashOffer.API.Domain.Attributes;

/// <summary>
/// Đánh dấu property KHÔNG được ghi vào audit log (vd mật khẩu, dữ liệu nhạy cảm).
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class AuditIgnoreAttribute : Attribute
{
}
