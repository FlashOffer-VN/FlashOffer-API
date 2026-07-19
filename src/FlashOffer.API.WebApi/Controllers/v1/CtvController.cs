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
}