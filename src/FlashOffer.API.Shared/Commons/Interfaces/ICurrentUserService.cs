// src/FlashOffer.API.Shared/Common/Interfaces/ICurrentUserService.cs
namespace FlashOffer.API.Shared.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
    string? IpAddress { get; }
    string? UserAgent { get; }
}