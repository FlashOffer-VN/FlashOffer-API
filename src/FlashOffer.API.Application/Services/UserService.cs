using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Common.Interfaces;

namespace FlashOffer.API.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepo;
    private readonly ICurrentUserService _currentUserService;

    public UserService(IRepository<User> userRepo, ICurrentUserService currentUserService)
    {
        _userRepo = userRepo;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> GetOrCreateUserAsync(string fullName, string phone, string? email = null)
    {
        // Tìm user theo phone
        var existing = await _userRepo.GetFirstAsync(u => u.Phone == phone);
        if (existing != null)
            return existing.Id;

        // Tạo user mới
        var user = new User
        {
            FullName = fullName,
            Phone = phone,
            Email = email ?? $"{phone}@temp.com",
            Username = $"user_{Guid.NewGuid():N}".Substring(0, 12),
            Role = UserRole.Customer,
            IsActive = true
        };

        await _userRepo.AddAsync(user);
        await _userRepo.SaveChangesAsync();
        return user.Id;
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return null;
        return await _userRepo.GetByIdAsync(Guid.Parse(userId));
    }
}