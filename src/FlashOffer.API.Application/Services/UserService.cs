using FlashOffer.API.Application.Common.Helpers;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Common.Interfaces;
using FlashOffer.API.Shared.Constants;
using FlashOffer.API.Shared.Exceptions;
using FlashOffer.API.Shared.Resources;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepo;
    private readonly ICurrentUserService _currentUserService;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly IStringLocalizer<ExceptionMessages> _exceptionLocalizer;
    private readonly IAuthAuditService _authAuditService;

    public UserService(
        IRepository<User> userRepo,
        ICurrentUserService currentUserService,
        IStringLocalizer<SharedResource> localizer,
        IStringLocalizer<ExceptionMessages> exceptionLocalizer,
        IAuthAuditService authAuditService)
    {
        _userRepo = userRepo;
        _currentUserService = currentUserService;
        _localizer = localizer;
        _exceptionLocalizer = exceptionLocalizer;
        _authAuditService = authAuditService;
    }

    public async Task<Guid> GetOrCreateUserAsync(string fullName, string phone, string? email = null)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw UserException.PhoneRequired(_exceptionLocalizer);

        var existing = await _userRepo.GetFirstAsync(u =>
            u.Phone == phone ||
            (!string.IsNullOrEmpty(email) && u.Email == email)
        );

        if (existing != null)
        {
            if (existing.IsDeleted)
                throw UserException.PhoneAlreadyExists(_exceptionLocalizer, phone);

            existing.FullName = fullName;
            if (!string.IsNullOrEmpty(email))
                existing.Email = email;

            _userRepo.Update(existing);
            await _userRepo.SaveChangesAsync();
            return existing.Id;
        }

        var user = new User
        {
            UserCode = CodeGenerator.Generate("USR"),
            FullName = fullName,
            Phone = phone,
            Email = string.IsNullOrEmpty(email) ? $"{phone}@temp.com" : email,
            Username = GenerateUniqueUsername(phone),
            PasswordHash = HashPassword(GenerateRandomPassword()),
            Role = UserRole.Customer,
            IsActive = true
        };

        await _userRepo.AddAsync(user);
        await _userRepo.SaveChangesAsync();
        await _authAuditService.LogAsync(user.Id, user.Username, AuditAction.Register, true,
            $"Tạo tài khoản mới (SĐT: {phone})");
        return user.Id;
    }

    public async Task<Guid> GetOrCreateUserWithPhonePasswordAsync(string fullName, string phone, string? email = null)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw UserException.PhoneRequired(_exceptionLocalizer);

        var existing = await _userRepo.GetFirstAsync(u =>
            u.Phone == phone ||
            (!string.IsNullOrEmpty(email) && u.Email == email)
        );

        if (existing != null)
        {
            if (existing.IsDeleted)
                throw UserException.PhoneAlreadyExists(_exceptionLocalizer, phone);

            existing.FullName = fullName;
            if (!string.IsNullOrEmpty(email))
                existing.Email = email;

            _userRepo.Update(existing);
            await _userRepo.SaveChangesAsync();
            return existing.Id;
        }

        var user = new User
        {
            UserCode = CodeGenerator.Generate("USR"),
            FullName = fullName,
            Phone = phone,
            Email = string.IsNullOrEmpty(email) ? $"{phone}@temp.com" : email,
            Username = GenerateUniqueUsername(phone),
            PasswordHash = HashPassword(phone), // Password = số điện thoại
            Role = UserRole.Customer,
            IsActive = true
        };

        await _userRepo.AddAsync(user);
        await _userRepo.SaveChangesAsync();
        await _authAuditService.LogAsync(user.Id, user.Username, AuditAction.Register, true,
            $"Tạo tài khoản mới (SĐT: {phone})");
        return user.Id;
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return null;
        return await _userRepo.GetByIdAsync(Guid.Parse(userId));
    }

    private string GenerateUniqueUsername(string phone)
    {
        // Username gắn với SĐT cho dễ đọc (vd: user0912345678); fallback random khi không có SĐT.
        var normalized = string.Concat((phone ?? string.Empty).Where(char.IsDigit));
        var baseUsername = string.IsNullOrEmpty(normalized)
            ? $"user{Guid.NewGuid():N}"[..50]
            : $"user{normalized}";

        // Đảm bảo không trùng username đã có.
        var exists = _userRepo.GetQueryable().Any(u => u.Username == baseUsername);
        if (!exists) return baseUsername;

        return $"{baseUsername}_{Guid.NewGuid():N}"[..50];
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