using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using Microsoft.AspNetCore.Authorization;
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
	public async Task<IActionResult> CreateAsync(
		[FromBody] CreateCtvRegistrationDto request)
	{
		var result = await _ctvRegistrationService.CreateAsync(request);

		return Ok(result, _localizer["CreateCtvRegistrationSuccess"]);
	}

	[Authorize(Roles = "Admin")]
	[HttpGet("ctv-registrations")]
	public async Task<IActionResult> GetList([FromQuery] CtvRegistrationQueryDto query)
	{
		var result = await _ctvRegistrationService.GetPagedAsync(query);
		return OkPaged(result, _localizer["CtvRegistrationListRetrievedSuccess"]);
	}

	[Authorize(Roles = "Admin")]
	[HttpPatch("ctv-registrations/{id}/approve")]
	public async Task<IActionResult> Approve(Guid id)
	{
		var result = await _ctvRegistrationService.ApproveAsync(id);
		return Ok(result, _localizer["CtvRegistrationApprovedSuccess"]);
	}
}