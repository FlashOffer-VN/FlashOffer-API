using AutoMapper;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Features.PurchaseRequests.Commands;
using FlashOffer.API.Application.Features.PurchaseRequests.Queries;
using FlashOffer.API.Application.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.WebApi.Controllers;

[Route("api/leads")]
public class PurchaseRequestsController : ApiControllerBase
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;
	private readonly IStringLocalizer<SharedResource> _localizer;

	public PurchaseRequestsController(IMediator mediator, IMapper mapper, IStringLocalizer<SharedResource> localizer)
	{
		_mediator = mediator;
		_mapper = mapper;
		_localizer = localizer;
	}

	[HttpPost("purchase-requests")]
	public async Task<IActionResult> Create([FromBody] CreatePurchaseRequestDto request)
	{
		var command = _mapper.Map<CreatePurchaseRequestCommand>(request);
		var response = await _mediator.Send(command);
		return Ok(response, _localizer["CreateSuccess"]);
	}

	[HttpGet("purchase-requests/{id}")]
	public async Task<IActionResult> GetById(Guid id)
	{
		var response = await _mediator.Send(new GetPurchaseRequestByIdQuery { Id = id });
		if (response == null)
			return NotFound(_localizer["NotFound"]);
		return Ok(response);
	}
}