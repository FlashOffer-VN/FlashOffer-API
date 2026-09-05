using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Common.Helpers;
using FlashOffer.API.Shared.Common.Interfaces;

namespace FlashOffer.API.Application.Services;

public class AuthAuditService : IAuthAuditService
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AuthAuditService(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task LogAsync(
        Guid? userId,
        string? username,
        string action,
        bool isSuccess,
        string? detail = null,
        CancellationToken cancellationToken = default)
    {
        // Parse User-Agent ra thông tin thiết bị ngay lúc ghi log
        var deviceInfo = UserAgentParser.Parse(_currentUser.UserAgent);

        var entry = new AuthAuditLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Username = username,
            Action = action,
            IsSuccess = isSuccess,
            Detail = detail,
            IpAddress = _currentUser.IpAddress,
            UserAgent = _currentUser.UserAgent,
            OperatingSystem = deviceInfo.OperatingSystem,
            BrowserName = deviceInfo.BrowserName,
            DeviceType = deviceInfo.DeviceType,
            Timestamp = DateTime.UtcNow
        };

        _context.Set<AuthAuditLog>().Add(entry);
        await _context.SaveChangesAsync(cancellationToken);
    }
}