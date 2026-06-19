using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.WebApi.Controllers;

[Route("api/leads")]
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

	[HttpPost("group-buying-requests")]
	public async Task<IActionResult> Create([FromBody] CreateGroupBuyingRequestDto request)
	{
		var response = await _service.CreateAsync(request);
		return Ok(response, _localizer["CreateGroupBuyingRequestSuccess"]);
	}

	[HttpGet("group-buying-requests")]
	public async Task<IActionResult> GetList([FromQuery] GetGroupBuyingRequestsQueryDto query)
	{
		if (!string.IsNullOrEmpty(query.Status))
		{
			if (!Enum.TryParse<GroupBuyingStatus>(query.Status, true, out _))
			{
				return BadRequest(_localizer["InvalidStatus"]);
			}
		}

		var result = await _service.GetPagedAsync(query);
		return OkPaged(result, _localizer["GroupBuyingRequestsRetrievedSuccess"]);
	}
}