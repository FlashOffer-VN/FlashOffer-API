using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Common.Models;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Application.Services;
using FlashOffer.API.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.WebApi.Controllers.v1;

[Route("api/leads")]
public class CtvRegistrationsController : ApiControllerBase
{
	private readonly ICtvRegistrationService _ctvRegistrationService;
	private readonly IStringLocalizer<SharedResource> _localizer;

	public CtvRegistrationsController(ICtvRegistrationService ctvRegistrationService, IStringLocalizer<SharedResource> localizer)
	{
		_ctvRegistrationService = ctvRegistrationService;
		_localizer = localizer;
	}

	[HttpPost("ctv-registrations")]
	public async Task<IActionResult> Create(
		[FromBody] CreateCtvRegistrationDto request)
	{
		var result = await _ctvRegistrationService.CreateAsync(request);

		return Ok(result, _localizer["CreateCtvRegistrationSuccess"]);
	}
}