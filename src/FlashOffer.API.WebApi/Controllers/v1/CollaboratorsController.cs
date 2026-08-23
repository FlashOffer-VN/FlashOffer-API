using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.WebApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class CollaboratorsController : ApiControllerBase
{
    private readonly ICollaboratorService _collaboratorService;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public CollaboratorsController(ICollaboratorService collaboratorService, IStringLocalizer<SharedResource> localizer)
    {
        _collaboratorService = collaboratorService;
        _localizer = localizer;
    }

    /// <summary>
    /// Đăng ký CTV mới
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateCollaboratorDto request)
    {
        var result = await _collaboratorService.CreateAsync(request);
        return Ok(result, _localizer["Collaborator_CreateSuccess"]);
    }

    /// <summary>
    /// Lấy thông tin CTV theo Id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _collaboratorService.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Lấy danh sách CTV phân trang
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string? search = null)
    {
        var result = await _collaboratorService.GetPagedAsync(page, size, search);
        return OkPaged(result);
    }

    /// <summary>
    /// Cập nhật thông tin CTV
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCollaboratorDto request)
    {
        var result = await _collaboratorService.UpdateAsync(id, request);
        return Ok(result, _localizer["Collaborator_UpdateSuccess"]);
    }

    /// <summary>
    /// Duyệt CTV
    /// </summary>
    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _collaboratorService.ApproveAsync(id);
        return Ok(new { message = _localizer["Collaborator_ApproveSuccess"] });
    }

    /// <summary>
    /// Từ chối CTV
    /// </summary>
    [HttpPost("{id}/reject")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Reject(Guid id, [FromBody] string? reason = null)
    {
        await _collaboratorService.RejectAsync(id, reason);
        return Ok(new { message = _localizer["Collaborator_RejectSuccess"] });
    }

    /// <summary>
    /// Xóa mềm CTV
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _collaboratorService.DeleteAsync(id);
        return Ok(new { message = _localizer["Collaborator_DeleteSuccess"] });
    }

    /// <summary>
    /// Khôi phục CTV đã xóa
    /// </summary>
    [HttpPost("{id}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Restore(Guid id)
    {
        await _collaboratorService.RestoreAsync(id);
        return Ok(new { message = _localizer["Collaborator_RestoreSuccess"] });
    }
}