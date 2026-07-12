using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.WebApi.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class GroupBuyingRequestsController : ApiControllerBase
{
    private readonly IGroupBuyingRequestService _service;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public GroupBuyingRequestsController(
        IGroupBuyingRequestService service,
        IStringLocalizer<SharedResource> localizer)
    {
        _service = service;
        _localizer = localizer;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateGroupBuyingRequestDto request)
    {
        var response = await _service.CreateAsync(request);
        return Ok(response, _localizer["GroupBuyingRequest_CreateSuccess"]);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet] 
    public async Task<IActionResult> GetList([FromQuery] GetGroupBuyingRequestsQueryDto query)
    {
        if (!string.IsNullOrEmpty(query.Status) && !Enum.TryParse<GroupBuyingStatus>(query.Status, true, out _))
        {
            return BadRequest(_localizer["GroupBuyingRequest_InvalidStatus"],
                new List<string> { _localizer["GroupBuyingRequest_InvalidStatusMessage"] });
        }

        var result = await _service.GetPagedAsync(query);
        return OkPaged(result, _localizer["GroupBuyingRequest_ListRetrievedSuccess"]);
    }
}