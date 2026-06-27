using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace FlashOffer.API.WebApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ApiControllerBase
{
	private readonly IAuthService _authService;
	private readonly IStringLocalizer<SharedResource> _localizer;
	private readonly LoginRequestValidator _validator;

	public AuthController(
		IAuthService authService,
		IStringLocalizer<SharedResource> localizer,
		LoginRequestValidator validator)
	{
		_authService = authService;
		_localizer = localizer;
		_validator = validator;
	}

	/// <summary>
	/// Đăng nhập
	/// </summary>
	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<IActionResult> Login([FromBody] LoginRequest request)
	{
		var validationResult = await _validator.ValidateAsync(request);
		if (!validationResult.IsValid)
		{
			return BadRequest(_localizer["ValidationFailed"],
				validationResult.Errors.Select(e => e.ErrorMessage).ToList());
		}

		var result = await _authService.LoginAsync(request);
		if (result == null)
		{
			return Unauthorized(_localizer["LoginFailed"],
				new List<string> { _localizer["InvalidCredentials"] });
		}

		return Ok(result, _localizer["LoginSuccess"]);
	}

	/// <summary>
	/// Đăng xuất
	/// </summary>
	[HttpPost("logout")]
	[Authorize]
	public async Task<IActionResult> Logout()
	{
		var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
		await _authService.LogoutAsync(token);
		return Ok(_localizer["LogoutSuccess"]);
	}

	/// <summary>
	/// Làm mới token
	/// </summary>
	[HttpPost("refresh")]
	[Authorize]
	public async Task<IActionResult> RefreshToken()
	{
		var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
		var result = await _authService.RefreshTokenAsync(token);
		if (result == null)
		{
			return Unauthorized(_localizer["InvalidToken"],
				new List<string> { _localizer["TokenExpiredOrInvalid"] });
		}
		return Ok(result, _localizer["TokenRefreshed"]);
	}

	/// <summary>
	/// Đổi mật khẩu
	/// </summary>
	[HttpPost("change-password")]
	[Authorize]
	public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (string.IsNullOrEmpty(userId))
		{
			return Unauthorized(_localizer["InvalidUser"],
				new List<string> { _localizer["UserNotAuthenticated"] });
		}

		var result = await _authService.ChangePasswordAsync(Guid.Parse(userId), request);
		if (!result)
		{
			return BadRequest(_localizer["ChangePasswordFailed"],
				new List<string> { _localizer["ChangePasswordErrorMessage"] });
		}
		return Ok(_localizer["ChangePasswordSuccess"]);
	}

	/// <summary>
	/// Quên mật khẩu - gửi email reset
	/// </summary>
	[HttpPost("forgot-password")]
	[AllowAnonymous]
	public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
	{
		await _authService.ForgotPasswordAsync(request);
		return Ok(_localizer["ResetLinkSent"]);
	}

	/// <summary>
	/// Đặt lại mật khẩu với token
	/// </summary>
	[HttpPost("reset-password")]
	[AllowAnonymous]
	public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
	{
		var result = await _authService.ResetPasswordAsync(request);
		if (!result)
		{
			return BadRequest(_localizer["ResetPasswordFailed"]);
		}
		return Ok(_localizer["ResetPasswordSuccess"]);
	}

	/// <summary>
	/// Lấy thông tin user hiện tại
	/// </summary>
	[HttpGet("me")]
	[Authorize]
	public async Task<IActionResult> GetMe()
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (string.IsNullOrEmpty(userId))
		{
			return Unauthorized(_localizer["InvalidUser"]);
		}

		var user = await _authService.GetUserByIdAsync(Guid.Parse(userId));
		if (user == null)
		{
			return NotFound(_localizer["UserNotFound"]);
		}
		return Ok(user, _localizer["UserInfoRetrieved"]);
	}
}