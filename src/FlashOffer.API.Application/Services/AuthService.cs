using FlashOffer.API.Application.Common.Configurations;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Common.Helpers;
using FlashOffer.API.Shared.Common.Interfaces;
using FlashOffer.API.Shared.Constants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace FlashOffer.API.Application.Services;

public class AuthService : IAuthService
{
	private readonly IRepository<User> _userRepository;
	private readonly IJwtService _jwtService;
	private readonly JwtSettings _jwtSettings;
	private readonly ILogger<AuthService> _logger;
	private readonly IAuthAuditService _authAuditService;

	public AuthService(
		IRepository<User> userRepository,
		IJwtService jwtService,
		IOptions<JwtSettings> jwtSettings,
		ILogger<AuthService> logger,
		IAuthAuditService authAuditService)
	{
		_userRepository = userRepository;
		_jwtService = jwtService;
		_jwtSettings = jwtSettings.Value;
		_logger = logger;
		_authAuditService = authAuditService;
	}

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        // Tìm user theo username, email, hoặc phone
        var users = await _userRepository.FindAsync(u =>
            !u.IsDeleted && (
                u.Username == request.Username ||
                u.Email == request.Username ||
                u.Phone == request.Username
            )
        );
        var user = users.FirstOrDefault();

        if (user == null || !PasswordHasher.Verify(request.Password, user.PasswordHash ?? string.Empty))
        {
            _logger.LogWarning($"Login failed for user: {request.Username}");
            await _authAuditService.LogAsync(null, request.Username, AuditAction.Login, false,
                "Sai tên đăng nhập hoặc mật khẩu");
            return null;
        }

        if (!user.IsActive)
        {
            _logger.LogWarning($"Inactive user attempted login: {request.Username}");
            await _authAuditService.LogAsync(user.Id, user.Username, AuditAction.Login, false,
                "Tài khoản đã bị vô hiệu hóa");
            return null;
        }

        var roles = GetRoles(user.Role);
        var token = _jwtService.GenerateToken(user.Id.ToString(), user.Username, roles);

        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.SaveChangesAsync();

        _logger.LogInformation($"User logged in successfully: {user.Username} (ID: {user.Id})");
        await _authAuditService.LogAsync(user.Id, user.Username, AuditAction.Login, true);

        return new LoginResponse
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role.ToString()
        };
    }

    public async Task LogoutAsync(string token)
	{
		var principal = _jwtService.ValidateToken(token);
		var username = principal?.FindFirst(ClaimTypes.Name)?.Value;
		var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		_jwtService.BlacklistToken(token);
		_logger.LogInformation("User logged out");
		await _authAuditService.LogAsync(
			Guid.TryParse(userId, out var id) ? id : null,
			username,
			AuditAction.Logout,
			true);
	}

	public async Task<LoginResponse?> RefreshTokenAsync(string token)
	{
		// Lấy thông tin user từ token cũ (trước khi refresh) để ghi audit
		var oldPrincipal = _jwtService.ValidateToken(token);
		var oldUsername = oldPrincipal?.FindFirst(ClaimTypes.Name)?.Value;
		var oldUserId = oldPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		var newToken = await _jwtService.RefreshTokenAsync(token);
		if (string.IsNullOrEmpty(newToken))
		{
			_logger.LogWarning("Refresh token failed");
			await _authAuditService.LogAsync(
				Guid.TryParse(oldUserId, out var oldId) ? oldId : null,
				oldUsername,
				AuditAction.RefreshToken,
				false, "Token hết hạn hoặc không hợp lệ");
			return null;
		}

		// Extract user info from new token
		var principal = _jwtService.ValidateToken(newToken);
		var username = principal?.FindFirst(ClaimTypes.Name)?.Value;
		var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userId))
		{
			await _authAuditService.LogAsync(null, username ?? oldUsername, AuditAction.RefreshToken, false);
			return null;
		}

		var users = await _userRepository.FindAsync(u => u.Id == Guid.Parse(userId) && !u.IsDeleted);
		var user = users.FirstOrDefault();

		await _authAuditService.LogAsync(
			Guid.TryParse(userId, out var id) ? id : null,
			username,
			AuditAction.RefreshToken,
			true);

		return new LoginResponse
		{
			Token = newToken,
			ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
			Username = username,
			FullName = user?.FullName ?? string.Empty,
			Role = user?.Role.ToString() ?? string.Empty
		};
	}

	public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
	{
		if (request.NewPassword != request.ConfirmNewPassword)
		{
			_logger.LogWarning("Password confirmation mismatch");
			await _authAuditService.LogAsync(userId, null, AuditAction.ChangePassword, false,
				"Mật khẩu xác nhận không khớp");
			return false;
		}

		var users = await _userRepository.FindAsync(u => u.Id == userId && !u.IsDeleted);
		var user = users.FirstOrDefault();

		if (user == null)
		{
			_logger.LogWarning($"User not found: {userId}");
			await _authAuditService.LogAsync(userId, null, AuditAction.ChangePassword, false,
				"Không tìm thấy người dùng");
			return false;
		}

		if (!PasswordHasher.Verify(request.CurrentPassword, user.PasswordHash ?? string.Empty))
		{
			_logger.LogWarning($"Invalid current password for user: {userId}");
			await _authAuditService.LogAsync(userId, user.Username, AuditAction.ChangePassword, false,
				"Mật khẩu hiện tại không đúng");
			return false;
		}

		user.PasswordHash = PasswordHasher.Hash(request.NewPassword);
		await _userRepository.SaveChangesAsync();

		_logger.LogInformation($"Password changed for user: {userId}");
		await _authAuditService.LogAsync(userId, user.Username, AuditAction.ChangePassword, true);
		return true;
	}

	public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
	{
		// TODO: Send reset password email with token
		_logger.LogInformation($"Password reset requested for email: {request.Email}");
		await _authAuditService.LogAsync(null, request.Email, AuditAction.ResetPassword, true,
			"Yêu cầu gửi link đặt lại mật khẩu");
	}

	public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
	{
		if (request.NewPassword != request.ConfirmPassword)
		{
			_logger.LogWarning("Password confirmation mismatch");
			await _authAuditService.LogAsync(null, request.Email, AuditAction.ResetPassword, false,
				"Mật khẩu xác nhận không khớp");
			return false;
		}

		// TODO: Validate reset token and update password
		var users = await _userRepository.FindAsync(u => u.Email == request.Email && !u.IsDeleted);
		var user = users.FirstOrDefault();

		if (user == null)
		{
			_logger.LogWarning($"User not found for password reset: {request.Email}");
			await _authAuditService.LogAsync(null, request.Email, AuditAction.ResetPassword, false,
				"Không tìm thấy người dùng");
			return false;
		}

		user.PasswordHash = PasswordHasher.Hash(request.NewPassword);
		await _userRepository.SaveChangesAsync();

		_logger.LogInformation($"Password reset for user: {request.Email}");
		await _authAuditService.LogAsync(user.Id, user.Username, AuditAction.ResetPassword, true);
		return true;
	}

	public async Task<UserInfoResponse?> GetUserByIdAsync(Guid userId)
	{
		var users = await _userRepository.FindAsync(u => u.Id == userId && !u.IsDeleted);
		var user = users.FirstOrDefault();

		if (user == null) return null;

		return new UserInfoResponse
		{
			Id = user.Id,
			Username = user.Username,
			FullName = user.FullName,
			Email = user.Email,
			Phone = user.Phone,
			Role = user.Role.ToString(),
			IsActive = user.IsActive,
			LastLoginAt = user.LastLoginAt
		};
	}

	private static List<string> GetRoles(UserRole role)
	{
		return role switch
		{
			UserRole.Admin => new List<string> { "Admin" },
			UserRole.CTV => new List<string> { "CTV" },
			UserRole.Customer => new List<string> { "Customer" },
			_ => new List<string> { "Customer" }
		};
	}
}