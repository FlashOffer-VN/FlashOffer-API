namespace FlashOffer.API.Application.Common.Interfaces;

/// <summary>
/// Ghi nhận các thao tác đăng nhập/đăng ký/bảo mật tài khoản vào AuthAuditLogs.
/// </summary>
public interface IAuthAuditService
{
    Task LogAsync(
        Guid? userId,
        string? username,
        string action,
        bool isSuccess,
        string? detail = null,
        CancellationToken cancellationToken = default);
}