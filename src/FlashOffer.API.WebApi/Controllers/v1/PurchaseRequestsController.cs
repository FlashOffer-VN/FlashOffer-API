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

	[HttpPatch("purchase-requests/{id}/status")]
	//[Authorize(Roles = "Admin")]
	public async Task<IActionResult> UpdateStatus(
	Guid id,
	[FromBody] UpdatePurchaseRequestStatusDto dto)
	{
		var result = await _service.UpdateStatusAsync(id, dto);
		return Ok(result, _localizer["UpdateStatusSuccess"]);
	}

	[HttpGet("purchase-requests/export")]
	//[Authorize(Roles = "Admin")]
	public async Task<IActionResult> ExportAsync([FromQuery] ExportPurchaseRequestsQuery query)
	{
		var bytes = await _mediator.Send(query);
		var fileName = $"purchase_requests_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
		return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
	}
}