using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Common.Interfaces;
using Microsoft.Extensions.Options;
using FlashOffer.API.Application.Common.Configurations;
using FlashOffer.API.Shared.Common.Helpers;

namespace FlashOffer.API.Application.Services;

public class AuthService : IAuthService
{
	private readonly IRepository<Admin> _adminRepository;
	private readonly IJwtService _jwtService;
	private readonly JwtSettings _jwtSettings;

	public AuthService(
		IRepository<Admin> adminRepository,
		IJwtService jwtService,
		IOptions<JwtSettings> jwtSettings)
	{
		_adminRepository = adminRepository;
		_jwtService = jwtService;
		_jwtSettings = jwtSettings.Value;
	}

	public async Task<LoginResponse?> LoginAsync(LoginRequest request)
	{
		// Tìm admin theo username
		var admins = await _adminRepository.FindAsync(a => a.Username == request.Username);
		var admin = admins.FirstOrDefault(); // Lấy phần tử đầu tiên


		if (admin == null || !PasswordHasher.Verify(request.Password, admin.PasswordHash))
		{
			return null;
		}

		var roles = new List<string> { "Admin" };
		var token = _jwtService.GenerateToken(admin.Id.ToString(), admin.Username, roles);

		admin.LastLoginAt = DateTime.UtcNow;
		await _adminRepository.SaveChangesAsync();

		return new LoginResponse
		{
			Token = token,
			ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
			Username = admin.Username
		};
	}
}