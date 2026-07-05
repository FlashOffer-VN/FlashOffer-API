// WebApi/Controllers/PartnerController.cs
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.WebApi;
using FlashOffer.API.WebApi.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartnerController : ApiControllerBase
{
    private readonly IPartnerService _partnerService;
    private readonly IValidator<PartnerRegisterRequest> _validator;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public PartnerController(
        IPartnerService partnerService,
        IValidator<PartnerRegisterRequest> validator,
        IStringLocalizer<SharedResource> localizer)
    {
        _partnerService = partnerService;
        _validator = validator;
        _localizer = localizer;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] PartnerRegisterRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = _localizer["PartnerValidationFailed"],
                Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            });
        }

        var result = await _partnerService.RegisterAsync(request);
        return Ok(result, _localizer["CreatePartnerSuccess"]);
    }
}