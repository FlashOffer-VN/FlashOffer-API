using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Common.Interfaces;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepo;
    private readonly ICurrentUserService _currentUserService;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public UserService(
        IRepository<User> userRepo,
        ICurrentUserService currentUserService,
        IStringLocalizer<SharedResource> localizer)
    {
        _userRepo = userRepo;
        _currentUserService = currentUserService;
        _localizer = localizer;
    }

    public async Task<Guid> GetOrCreateUserAsync(string fullName, string phone, string? email = null)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException(_localizer["User_PhoneRequired"]);

        var existing = await _userRepo.GetFirstAsync(u =>
            u.Phone == phone ||
            (!string.IsNullOrEmpty(email) && u.Email == email)
        );

        if (existing != null)
        {
            if (existing.IsDeleted)
            {
                throw new InvalidOperationException(_localizer["User_PhoneAlreadyExists"]);
            }

            existing.FullName = fullName;
            if (!string.IsNullOrEmpty(email))
                existing.Email = email;

            _userRepo.Update(existing);
            await _userRepo.SaveChangesAsync();
            return existing.Id;
        }

        var user = new User
        {
            FullName = fullName,
            Phone = phone,
            Email = string.IsNullOrEmpty(email) ? $"{phone}@temp.com" : email,
            Username = GenerateUniqueUsername(),
            PasswordHash = HashPassword(GenerateRandomPassword()),
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

    private string GenerateUniqueUsername()
    {
        return $"user_{Guid.NewGuid():N}".Substring(0, 12);
    }

    private string GenerateRandomPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}