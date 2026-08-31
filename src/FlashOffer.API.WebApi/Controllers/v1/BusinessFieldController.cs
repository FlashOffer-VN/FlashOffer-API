using FlashOffer.API.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlashOffer.API.WebApi.Controllers.v1;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BusinessFieldController : ApiControllerBase
{
    private readonly IBusinessFieldService _businessFieldService;

    public BusinessFieldController(IBusinessFieldService businessFieldService)
    {
        _businessFieldService = businessFieldService;
    }

    /// <summary>
    /// Lấy danh sách lĩnh vực hoạt động đang hoạt động
    /// </summary>
    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActiveFields()
    {
        var fields = await _businessFieldService.GetActiveFieldsAsync();
        return Ok(fields);
    }
}
