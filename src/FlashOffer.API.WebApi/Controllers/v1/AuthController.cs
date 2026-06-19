using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Application.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

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

	[HttpPost("login")]
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
			return Unauthorized(_localizer["LoginFailed"]);
		}

		return Ok(result, _localizer["LoginSuccess"]);
	}
}