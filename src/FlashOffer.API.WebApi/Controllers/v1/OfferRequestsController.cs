using AutoMapper;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.Features.OfferRequests.Commands;
using FlashOffer.API.Application.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.WebApi.Controllers;

[Route("api/leads")]
//[Authorize]
public class OfferRequestsController : ApiControllerBase
{
	private readonly IMediator _mediator;
	private readonly IMapper _mapper;
	private readonly IStringLocalizer<SharedResource> _localizer;

	public OfferRequestsController(IMediator mediator, IMapper mapper, IStringLocalizer<SharedResource> localizer)
	{
		_mediator = mediator;
		_mapper = mapper;
		_localizer = localizer;
	}

	[HttpPost("offer-requests")]
	public async Task<IActionResult> CreateAsync([FromBody] CreateOfferRequestDto request)
	{
		var command = _mapper.Map<CreateOfferRequestCommand>(request);
		var response = await _mediator.Send(command);
		return Ok(response, _localizer["CreateOfferRequestSuccess"]);
	}
}