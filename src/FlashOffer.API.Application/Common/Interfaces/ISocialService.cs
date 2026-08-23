using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Models;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface ISocialService
{
    Task<PagedList<PostResponse>> GetPostsAsync(GetPostsQuery query);
    Task<PostResponse> GetPostByIdAsync(Guid id);
    Task<PostResponse> CreatePostAsync(CreatePostRequest request);
    Task<PostResponse> UpdatePostAsync(Guid id, UpdatePostRequest request);
    Task<bool> DeletePostAsync(Guid id);
}