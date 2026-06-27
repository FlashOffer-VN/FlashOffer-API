using FlashOffer.API.Application.Features.Dashboard.Queries;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.WebApi;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.WebApi.Controllers.Admin;

[Route("api/admin")]
[Authorize(Roles = "Admin")]
[ApiController]
public class DashboardController : ApiControllerBase
{
	private readonly IMediator _mediator;
	private readonly IStringLocalizer<SharedResource> _localizer;

	public DashboardController(IMediator mediator, IStringLocalizer<SharedResource> localizer)
	{
		_mediator = mediator;
		_localizer = localizer;
	}

	[HttpGet("dashboard")]
	public async Task<IActionResult> GetDashboard()
	{
		var stats = await _mediator.Send(new GetDashboardQuery());
		return Ok(stats, _localizer["DashboardSuccess"]);
	}
}