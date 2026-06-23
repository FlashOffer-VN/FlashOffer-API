using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Features.PurchaseRequests.Commands;
using FlashOffer.API.Application.Features.PurchaseRequests.Queries;
using FlashOffer.API.Application.Resources;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.WebApi.Controllers;

[Route("api/leads")]
//[Authorize]
public class PurchaseRequestsController : ApiControllerBase
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;
	private readonly IPurchaseRequestService _service;
	private readonly IStringLocalizer<SharedResource> _localizer;

	public PurchaseRequestsController(IPurchaseRequestService service, IMediator mediator, IMapper mapper, IStringLocalizer<SharedResource> localizer)
	{
		_mediator = mediator;
		_mapper = mapper;
		_localizer = localizer;
		_service = service;
	}

	[HttpPost("purchase-requests")]
	public async Task<IActionResult> CreateAsync([FromBody] CreatePurchaseRequestDto request)
	{
		var command = _mapper.Map<CreatePurchaseRequestCommand>(request);
		var response = await _mediator.Send(command);
		return Ok(response, _localizer["CreateSuccess"]);
	}

	[HttpGet("purchase-requests/{id}")]
	public async Task<IActionResult> GetByIdAsync(Guid id)
	{
		var response = await _mediator.Send(new GetPurchaseRequestByIdQuery { Id = id });
		if (response == null)
			return NotFound(_localizer["NotFound"]);
		return Ok(response);
	}

	[HttpGet("purchase-requests")]
	public async Task<IActionResult> GetListAsync([FromQuery] PurchaseRequestQueryDto query)
	{
		var result = await _service.GetPagedAsync(query);
		return OkPaged(result, _localizer["PurchaseRequestsRetrievedSuccess"]);
	}
}