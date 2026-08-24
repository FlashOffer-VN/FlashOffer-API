using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface ISocialInteractionService
{
    // Like
    Task ToggleLikeAsync(Guid postId, Guid userId);
    Task<bool> HasLikedAsync(Guid postId, Guid userId);
    Task<int> GetLikeCountAsync(Guid postId);

    // Comment
    Task<SocialComment> AddCommentAsync(Guid postId, Guid userId, string content, Guid? parentId = null);
    Task DeleteCommentAsync(Guid commentId, Guid userId);
    Task<List<SocialComment>> GetCommentsAsync(Guid postId);

    // Share
    Task<SocialShare> SharePostAsync(Guid postId, Guid userId, string? note = null);
    Task<int> GetShareCountAsync(Guid postId);
}