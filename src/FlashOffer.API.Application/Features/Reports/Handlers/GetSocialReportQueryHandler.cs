using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Application.Features.Reports.Queries;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FlashOffer.API.Application.Features.Reports.Handlers;

public class GetSocialReportQueryHandler : IRequestHandler<GetSocialReportQuery, SocialReportDto>
{
    private readonly IRepository<SocialPost> _postRepo;
    private readonly IRepository<User> _userRepo;
    private readonly ILogger<GetSocialReportQueryHandler> _logger;

    public GetSocialReportQueryHandler(
        IRepository<SocialPost> postRepo,
        IRepository<User> userRepo,
        ILogger<GetSocialReportQueryHandler> logger)
    {
        _postRepo = postRepo;
        _userRepo = userRepo;
        _logger = logger;
    }

    public async Task<SocialReportDto> Handle(GetSocialReportQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Admin report social requested [TopPostCount: {Count}]", request.TopPostCount);

        var posts = (await _postRepo.FindAsync(x => !x.IsDeleted, cancellationToken)).ToList();
        var topCount = Math.Clamp(request.TopPostCount, 1, 50);

        // Thống kê theo loại bài viết
        var postsByType = Enum.GetValues<PostType>()
            .Select(t => new PostsByTypeDto
            {
                Type = t.ToString(),
                Count = posts.Count(p => p.Type == t)
            })
            .Where(x => x.Count > 0 || x.Type == nameof(PostType.Post))
            .ToList();

        // Top bài viết nổi bật theo tổng tương tác
        var users = await _userRepo.FindAsync(x => !x.IsDeleted, cancellationToken);
        var userDict = users.ToDictionary(u => u.Id, u => u.FullName);

        var topPosts = posts
            .Select(p => new TopPostDto
            {
                Id = p.Id,
                Title = p.Title,
                AuthorName = p.AuthorId != Guid.Empty
                    ? userDict.GetValueOrDefault(p.AuthorId, "Unknown")
                    : "Unknown",
                LikesCount = p.LikesCount,
                CommentsCount = p.CommentsCount,
                SharesCount = p.SharesCount,
                CreatedAt = p.CreatedAt
            })
            .OrderByDescending(x => x.LikesCount + x.CommentsCount + x.SharesCount)
            .ThenByDescending(x => x.CreatedAt)
            .Take(topCount)
            .ToList();

        return new SocialReportDto
        {
            TotalPosts = posts.Count,
            TotalLikes = posts.Sum(p => p.LikesCount),
            TotalComments = posts.Sum(p => p.CommentsCount),
            TotalShares = posts.Sum(p => p.SharesCount),
            PostsByType = postsByType,
            TopPosts = topPosts
        };
    }
}