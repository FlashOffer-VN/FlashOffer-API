using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.WebApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CtvRegistrationsController : ApiControllerBase
{
    private readonly ICtvRegistrationService _ctvRegistrationService;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public CtvRegistrationsController(ICtvRegistrationService ctvRegistrationService, IStringLocalizer<SharedResource> localizer)
    {
        _ctvRegistrationService = ctvRegistrationService;
        _localizer = localizer;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCtvRegistrationDto request)
    {
        var result = await _ctvRegistrationService.CreateAsync(request);
        return Ok(result, _localizer["CtvRegistration_CreateSuccess"]);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] CtvRegistrationQueryDto query)
    {
        var result = await _ctvRegistrationService.GetPagedAsync(query);
        return OkPaged(result, _localizer["CtvRegistration_ListRetrievedSuccess"]);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}/approve")] 
    public async Task<IActionResult> Approve(Guid id)
    {
        var result = await _ctvRegistrationService.ApproveAsync(id);
        return Ok(result, _localizer["CtvRegistration_ApprovedSuccess"]);
    }
}