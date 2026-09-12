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

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class PartnersController : ApiControllerBase
{
    private readonly IPartnerService _partnerService;
    private readonly IValidator<PartnerRegisterRequest> _validator;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public PartnersController(
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
        return Ok(result, _localizer["PartnerRegisterSuccess"]);
    }

    [HttpGet("check-referral/{code}")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckReferralCode([FromRoute] string code)
    {
        var isValid = await _partnerService.IsReferralCodeValidAsync(code);
        if (!isValid)
        {
            return NotFound(_localizer["Partner_ReferralCodeNotFound"]);
        }

        return Ok(isValid,_localizer["Partner_ReferralCodeValid"]);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetList([FromQuery] PartnerFilterRequest filter)
    {
        var result = await _partnerService.GetPagedAsync(filter);
        return OkPaged(result, _localizer["Success"]);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var result = await _partnerService.GetDetailAsync(id);
        if (result == null)
            return NotFound(_localizer["Partner_NotFound"]);

        return Ok(result, _localizer["Success"]);
    }

    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var result = await _partnerService.ApproveAsync(id);
        return Ok(result, _localizer["Partner_ApproveSuccess"]);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(Guid id)
    {
        var result = await _partnerService.RejectAsync(id);
        return Ok(result, _localizer["Partner_RejectSuccess"]);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var result = await _partnerService.ActivateAsync(id);
        return Ok(result, _localizer["Partner_ActivateSuccess"]);
    }

    /// <summary>
    /// Danh sách đối tác đã xóa mềm
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpGet("deleted")]
    public async Task<IActionResult> GetDeleted(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var result = await _partnerService.GetPagedDeletedAsync(pageNumber, pageSize, search);
        return OkPaged(result, _localizer["Success"]);
    }

    /// <summary>
    /// Xóa mềm đối tác
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _partnerService.DeleteAsync(id);
        return Ok(new { message = _localizer["Partner_DeleteSuccess"] });
    }

    /// <summary>
    /// Khôi phục đối tác đã xóa
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _partnerService.RestoreAsync(id);
        return Ok(result, _localizer["Partner_RestoreSuccess"]);
    }
}