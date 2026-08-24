using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Infrastructure.Services;
using FlashOffer.API.Shared.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.WebApi.Controllers;

[ApiController]
[Route("api/social/[controller]")]
[Authorize]
public class SocialInteractionController : ApiControllerBase
{
    private readonly ISocialInteractionService _interactionService;
    private readonly IRepository<SocialPost> _postRepo;
    private readonly IRepository<User> _userRepo;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly ICurrentUserService _currentUserService;

    public SocialInteractionController(
        ISocialInteractionService interactionService,
        IRepository<SocialPost> postRepo,
        IRepository<User> userRepo,
        IStringLocalizer<SharedResource> localizer,
        ICurrentUserService currentUserService)
    {
        _interactionService = interactionService;
        _postRepo = postRepo;
        _userRepo = userRepo;
        _localizer = localizer;
        _currentUserService = currentUserService;
    }

    // ===== LIKE =====
    [HttpPost("posts/{postId}/like")]
    public async Task<IActionResult> ToggleLike(Guid postId)
    {
        var userId = Guid.Parse(_currentUserService.UserId!);
        await _interactionService.ToggleLikeAsync(postId, userId);
        return Ok(new { message = _localizer["Social_LikeSuccess"] });
    }

    [HttpGet("posts/{postId}/like-status")]
    public async Task<IActionResult> GetLikeStatus(Guid postId)
    {
        var userId = Guid.Parse(_currentUserService.UserId!);
        var hasLiked = await _interactionService.HasLikedAsync(postId, userId);
        var count = await _interactionService.GetLikeCountAsync(postId);
        return Ok(new { hasLiked, count });
    }

    // ===== COMMENT =====
    [HttpPost("posts/{postId}/comments")]
    public async Task<IActionResult> AddComment(Guid postId, [FromBody] CreateCommentDto dto)
    {
        var userId = Guid.Parse(_currentUserService.UserId!);
        var comment = await _interactionService.AddCommentAsync(
            postId, userId, dto.Content, dto.ParentCommentId);

        return Ok(comment);
    }

    [HttpDelete("comments/{commentId}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        var userId = Guid.Parse(_currentUserService.UserId!);
        await _interactionService.DeleteCommentAsync(commentId, userId);
        return Ok(new { message = _localizer["Social_CommentDeleted"] });
    }

    [HttpGet("posts/{postId}/comments")]
    public async Task<IActionResult> GetComments(Guid postId)
    {
        var comments = await _interactionService.GetCommentsAsync(postId);
        return Ok(comments);
    }

    // ===== SHARE =====
    [HttpPost("posts/{postId}/share")]
    public async Task<IActionResult> SharePost(Guid postId, [FromBody] SharePostDto? dto)
    {
        var userId = Guid.Parse(_currentUserService.UserId!);
        var share = await _interactionService.SharePostAsync(
            postId, userId, dto?.Note);
        return Ok(new { message = _localizer["Social_ShareSuccess"], data = share });
    }

    [HttpGet("posts/{postId}/share-count")]
    public async Task<IActionResult> GetShareCount(Guid postId)
    {
        var count = await _interactionService.GetShareCountAsync(postId);
        return Ok(new { count });
    }
}