using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlashOffer.API.WebApi.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class CtvController : ApiControllerBase
{
    private readonly ICtvService _ctvService;

    public CtvController(ICtvService ctvService)
    {
        _ctvService = ctvService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] CtvFilterRequest request)
    {
        var result = await _ctvService.GetPagedAsync(request);
        return OkPaged(result, "Success");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var result = await _ctvService.GetDetailAsync(id);
        if (result == null)
            return NotFound("CTV_NotFound");
        return Ok(result);
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var result = await _ctvService.ApproveAsync(id);
        return Ok(result, "CTV_ApproveSuccess");
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(Guid id)
    {
        var result = await _ctvService.RejectAsync(id);
        return Ok(result, "CTV_RejectSuccess");
    }

    [HttpGet("deleted")]
    public async Task<IActionResult> GetDeleted(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var result = await _ctvService.GetPagedDeletedAsync(pageNumber, pageSize, search);
        return OkPaged(result, "Success");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _ctvService.DeleteAsync(id);
        return Ok(new { message = "Deleted successfully" });
    }

    [HttpPost("{id}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _ctvService.RestoreAsync(id);
        return Ok(result, "Restored successfully");
    }
}