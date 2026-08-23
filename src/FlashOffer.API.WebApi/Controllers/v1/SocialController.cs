using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.Services;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Models;
using FlashOffer.API.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlashOffer.API.WebApi.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class SocialController : ApiControllerBase
{
    private readonly ISocialService _socialService;

    public SocialController(ISocialService socialService)
    {
        _socialService = socialService;
    }

    /// <summary>
    /// Lấy danh sách bài viết (phân trang + filter)
    /// </summary>
    [HttpGet("posts")]
    public async Task<IActionResult> GetPosts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? type = null,
        [FromQuery] string? privacy = null,
        [FromQuery] string? tag = null)
    {
        var query = new GetPostsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Type = !string.IsNullOrEmpty(type) ? Enum.Parse<PostType>(type, true) : null,
            Privacy = !string.IsNullOrEmpty(privacy) ? Enum.Parse<PrivacyType>(privacy, true) : null,
            Tag = tag
        };

        var result = await _socialService.GetPostsAsync(query);
        return OkPaged(result, "Social_GetPostsSuccess");
    }

    /// <summary>
    /// Lấy chi tiết bài viết theo ID
    /// </summary>
    [HttpGet("posts/{id}")]
    public async Task<IActionResult> GetPostById(Guid id)
    {
        var result = await _socialService.GetPostByIdAsync(id);
        return Ok(result, "Social_GetPostSuccess");
    }

    /// <summary>
    /// Tạo bài viết mới
    /// </summary>
    [HttpPost("posts")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {
        var result = await _socialService.CreatePostAsync(request);
        return Created(nameof(GetPostById), result, "Social_CreateSuccess");
    }

    /// <summary>
    /// Cập nhật bài viết
    /// </summary>
    [HttpPut("posts/{id}")]
    public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostRequest request)
    {
        var result = await _socialService.UpdatePostAsync(id, request);
        return Ok(result, "Social_UpdateSuccess");
    }

    /// <summary>
    /// Xóa bài viết
    /// </summary>
    [HttpDelete("posts/{id}")]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        await _socialService.DeletePostAsync(id);
        return Ok("Social_DeleteSuccess");
    }
}