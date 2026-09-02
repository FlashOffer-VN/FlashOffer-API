using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Models;
using FlashOffer.API.Shared.Common.Interfaces;
using FlashOffer.API.Shared.Exceptions;
using FlashOffer.API.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using FlashOffer.API.Shared.Extensions;

namespace FlashOffer.API.Application.Services;

public class SocialService : ISocialService
{
    private readonly IRepository<SocialPost> _postRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Tag> _tagRepository;
    private readonly IRepository<PostTag> _postTagRepository;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly ICurrentUserService _currentUserService;
    private readonly ISocialInteractionService _interactionService;

    public SocialService(
        IRepository<SocialPost> postRepository,
        IRepository<User> userRepository,
        IRepository<Tag> tagRepository,
        IRepository<PostTag> postTagRepository,
        IMapper mapper,
        IStringLocalizer<SharedResource> localizer,
        ICurrentUserService currentUserService,
        ISocialInteractionService interactionService)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _tagRepository = tagRepository;
        _postTagRepository = postTagRepository;
        _mapper = mapper;
        _localizer = localizer;
        _currentUserService = currentUserService;
        _interactionService = interactionService;
    }

    public async Task<PagedList<PostResponse>> GetPostsAsync(GetPostsQuery query)
    {
        var userId = _currentUserService.UserId;
        var isAdmin = _currentUserService.IsInRole("Admin");

        Expression<Func<SocialPost, bool>>? predicate = null;

        if (query.Type.HasValue)
            predicate = predicate.And(p => p.Type == query.Type.Value);

        if (!string.IsNullOrEmpty(query.Tag))
            predicate = predicate.And(p => p.PostTags.Any(pt => pt.Tag.Name == query.Tag));

        // XÂY DỰNG PREDICATE KHÔNG CÓ AWAIT
        if (!isAdmin)
        {
            // Chỉ lấy bài Public đã duyệt + bài của chính user
            // Phần Friends sẽ xử lý sau khi lấy dữ liệu
            if (!string.IsNullOrEmpty(userId))
            {
                var userGuid = Guid.Parse(userId);
                predicate = predicate.And(p =>
                    (p.Privacy == PrivacyType.Public && p.IsApproved == true) ||
                    p.AuthorId == userGuid
                // Friends sẽ xử lý sau
                );
            }
            else
            {
                // Chưa login: chỉ lấy Public đã duyệt
                predicate = predicate.And(p =>
                    p.Privacy == PrivacyType.Public && p.IsApproved == true
                );
            }
        }

        predicate ??= p => true;

        // Lấy dữ liệu
        var posts = await _postRepository.GetPagedWithIncludesAsync(
            query.PageNumber,
            query.PageSize,
            includes: q => q
                .Include(p => p.Author)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .Include(p => p.Shares)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag),
            predicate: predicate,
            orderBy: p => p.CreatedAt,
            isDescending: true
        );

        // XỬ LÝ FRIENDS SAU KHI LẤY DỮ LIỆU
        if (!string.IsNullOrEmpty(userId) && !isAdmin)
        {
            var userGuid = Guid.Parse(userId);
            var friendIds = await GetFriendIdsAsync(userGuid); // Lấy danh sách bạn bè

            // Lọc bỏ bài Friends của người không phải bạn bè
            var filteredItems = posts.Items.Where(p =>
                p.Privacy != PrivacyType.Friends ||
                p.AuthorId == userGuid ||
                friendIds.Contains(p.AuthorId)
            ).ToList();

            // Cập nhật lại posts
            posts = new PagedList<SocialPost>(
                filteredItems,
                filteredItems.Count,
                query.PageNumber,
                query.PageSize
            );
        }

        // Map sang response
        var postResponses = _mapper.Map<List<PostResponse>>(posts.Items);

        // Thêm logic lấy Like status cho current user
        if (!string.IsNullOrEmpty(userId))
        {
            var userGuid = Guid.Parse(userId);
            foreach (var response in postResponses)
            {
                var post = posts.Items.First(p => p.Id == response.Id);
                response.IsLiked = await _interactionService.HasLikedAsync(post.Id, userGuid);
            }
        }

        // Gán Author
        foreach (var response in postResponses)
        {
            var post = posts.Items.First(p => p.Id == response.Id);
            response.Author = _mapper.Map<AuthorDto>(post.Author);
        }

        return new PagedList<PostResponse>(
            postResponses,
            posts.TotalCount,
            query.PageNumber,
            query.PageSize
        );
    }

    public async Task<PostResponse> GetPostByIdAsync(Guid id)
    {
        // Thêm Includes cho Likes, Comments, Shares và Comments.User
        var post = await _postRepository.GetFirstWithIncludesAsync(
            p => p.Id == id,
            includes: q => q
                .Include(p => p.Author)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Replies)
                        .ThenInclude(r => r.User)
                .Include(p => p.Shares)
                    .ThenInclude(s => s.User)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
        );

        if (post == null)
        {
            throw new NotFoundException(_localizer["Social_NotFound"]);
        }

        var response = _mapper.Map<PostResponse>(post);
        response.Author = _mapper.Map<AuthorDto>(post.Author);

        // Kiểm tra current user đã Like chưa
        var userId = _currentUserService.UserId;
        if (!string.IsNullOrEmpty(userId))
        {
            var userGuid = Guid.Parse(userId);
            response.IsLiked = await _interactionService.HasLikedAsync(post.Id, userGuid);
        }

        return response;
    }

    public async Task<PostResponse> CreatePostAsync(CreatePostRequest request)
    {
        var userId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedException(_localizer["Social_NotAuthorized"]);
        }

        var user = await _userRepository.GetByIdAsync(Guid.Parse(userId));
        if (user == null)
        {
            throw new NotFoundException(_localizer["User_NotFound"]);
        }

        var post = _mapper.Map<SocialPost>(request);
        post.AuthorId = user.Id;
        post.IsApproved = false; // Chờ duyệt

        // Xử lý Tags
        if (request.Tags != null && request.Tags.Any())
        {
            foreach (var tagName in request.Tags.Distinct())
            {
                var tag = await _tagRepository.GetFirstAsync(t => t.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName, UsageCount = 0 };
                    await _tagRepository.AddAsync(tag);
                    await _tagRepository.SaveChangesAsync();
                }
                tag.UsageCount++;

                post.PostTags.Add(new PostTag
                {
                    PostId = post.Id,
                    TagId = tag.Id,
                    Post = post,
                    Tag = tag
                });
            }
        }

        await _postRepository.AddAsync(post);
        await _postRepository.SaveChangesAsync();

        // Log để Admin biết
        _logger.LogInformation("📝 New post waiting for approval: PostId={PostId}, AuthorId={AuthorId}, Title={Title}",
            post.Id, post.AuthorId, post.Title ?? "Untitled");

        var createdPost = await _postRepository.GetFirstWithIncludesAsync(
            p => p.Id == post.Id,
            includes: q => q
                .Include(p => p.Author)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
        );

        var response = _mapper.Map<PostResponse>(createdPost);
        response.Author = _mapper.Map<AuthorDto>(createdPost.Author);

        // ✅ Thêm message chờ duyệt
        response.Message = _localizer["Social_PendingApproval"];

        return response;
    }

    public async Task<PostResponse> UpdatePostAsync(Guid id, UpdatePostRequest request)
    {
        var post = await _postRepository.GetFirstWithIncludesAsync(
        p => p.Id == id,
        includes: q => q
            .Include(p => p.Author)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
        );

        if (post == null)
            throw new NotFoundException(_localizer["Social_NotFound"]);

        var userId = _currentUserService.UserId;
        var isAdmin = _currentUserService.IsInRole("Admin");

        if (post.AuthorId.ToString() != userId && !isAdmin)
            throw new ForbiddenException(_localizer["Social_NotAuthorized"]);

        // Nếu chuyển từ private sang public -> cần duyệt lại
        var wasPrivate = post.Privacy != PrivacyType.Public;
        var isNowPublic = request.Privacy == PrivacyType.Public;

        if (wasPrivate && isNowPublic && !isAdmin)
        {
            post.IsApproved = false;
        }

        // Cập nhật Tags
        if (request.Tags != null)
        {
            foreach (var oldPostTag in post.PostTags.ToList())
            {
                var tag = await _tagRepository.GetByIdAsync(oldPostTag.TagId);
                if (tag != null)
                {
                    tag.UsageCount--;
                    _tagRepository.Update(tag);
                }
                _postTagRepository.Delete(oldPostTag);
            }
            post.PostTags.Clear();

            foreach (var tagName in request.Tags.Distinct())
            {
                var tag = await _tagRepository.GetFirstAsync(t => t.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName, UsageCount = 0 };
                    await _tagRepository.AddAsync(tag);
                    await _tagRepository.SaveChangesAsync();
                }
                tag.UsageCount++;
                _tagRepository.Update(tag);

                post.PostTags.Add(new PostTag
                {
                    PostId = post.Id,
                    TagId = tag.Id,
                    Post = post,
                    Tag = tag
                });
            }
        }

        _mapper.Map(request, post);
        _postRepository.Update(post);
        await _postRepository.SaveChangesAsync();

        var updatedPost = await _postRepository.GetFirstWithIncludesAsync(
            p => p.Id == id,
            includes: q => q
                .Include(p => p.Author)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
        );

        var response = _mapper.Map<PostResponse>(updatedPost);
        response.Author = _mapper.Map<AuthorDto>(updatedPost.Author);
        return response;
    }

    public async Task<bool> DeletePostAsync(Guid id)
    {
        var post = await _postRepository.GetFirstWithIncludesAsync(
            p => p.Id == id,
            includes: q => q.Include(p => p.PostTags)
        );

        if (post == null)
        {
            throw new NotFoundException(_localizer["Social_NotFound"]);
        }

        var userId = _currentUserService.UserId;
        var isAdmin = _currentUserService.IsInRole("Admin");

        if (post.AuthorId.ToString() != userId && !isAdmin)
        {
            throw new ForbiddenException(_localizer["Social_NotAuthorized"]);
        }

        foreach (var postTag in post.PostTags)
        {
            var tag = await _tagRepository.GetByIdAsync(postTag.TagId);
            if (tag != null)
            {
                tag.UsageCount--;
                _tagRepository.Update(tag);
            }
        }

        _postRepository.Delete(post);
        await _postRepository.SaveChangesAsync();
        return true;
    }

    public async Task<PagedList<PostResponse>> GetPendingPostsAsync(int pageNumber, int pageSize)
    {
        var isAdmin = _currentUserService.IsInRole("Admin");
        if (!isAdmin)
            throw new ForbiddenException(_localizer["Social_NotAuthorized"]);

        Expression<Func<SocialPost, bool>>? predicate = null;
        predicate = predicate.And(p => p.IsApproved == false);
        predicate = predicate.And(p => p.Privacy == PrivacyType.Public);

        var posts = await _postRepository.GetPagedWithIncludesAsync(
            pageNumber,
            pageSize,
            includes: q => q
                .Include(p => p.Author)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag),
            predicate: predicate ?? (p => true),
            orderBy: p => p.CreatedAt,
            isDescending: true
        );

        var postResponses = _mapper.Map<List<PostResponse>>(posts.Items);
        foreach (var response in postResponses)
        {
            var post = posts.Items.First(p => p.Id == response.Id);
            response.Author = _mapper.Map<AuthorDto>(post.Author);
        }

        return new PagedList<PostResponse>(
            postResponses,
            posts.TotalCount,
            pageNumber,
            pageSize
        );
    }

    public async Task<PostResponse> ApprovePostAsync(Guid id)
    {
        var isAdmin = _currentUserService.IsInRole("Admin");
        if (!isAdmin)
            throw new ForbiddenException(_localizer["Social_NotAuthorized"]);

        var post = await _postRepository.GetFirstWithIncludesAsync(
            p => p.Id == id,
            includes: q => q.Include(p => p.Author)
        );

        if (post == null)
            throw new NotFoundException(_localizer["Social_NotFound"]);

        if (post.IsApproved)
            throw new InvalidOperationException(_localizer["Social_AlreadyApproved"]);

        post.IsApproved = true;

        _postRepository.Update(post);
        await _postRepository.SaveChangesAsync();

        var response = _mapper.Map<PostResponse>(post);
        response.Author = _mapper.Map<AuthorDto>(post.Author);
        return response;
    }

    public async Task<PostResponse> RejectPostAsync(Guid id, string? reason = null)
    {
        var isAdmin = _currentUserService.IsInRole("Admin");
        if (!isAdmin)
            throw new ForbiddenException(_localizer["Social_NotAuthorized"]);

        var post = await _postRepository.GetFirstWithIncludesAsync(
            p => p.Id == id,
            includes: q => q.Include(p => p.Author)
        );

        if (post == null)
            throw new NotFoundException(_localizer["Social_NotFound"]);

        if (post.IsApproved)
            throw new InvalidOperationException(_localizer["Social_AlreadyApproved"]);

        post.IsApproved = false;
        // Có thể thêm field RejectionReason nếu muốn

        _postRepository.Update(post);
        await _postRepository.SaveChangesAsync();

        var response = _mapper.Map<PostResponse>(post);
        response.Author = _mapper.Map<AuthorDto>(post.Author);
        return response;
    }

    private async Task<List<Guid>> GetFriendIdsAsync(Guid userId)
    {
        //var friends = await _friendRepository.FindAsync(f =>
        //    (f.UserId == userId || f.FriendId == userId) &&
        //    f.Status == FriendStatus.Accepted
        //);

        //return friends.Select(f => f.UserId == userId ? f.FriendId : f.UserId).ToList();
        return new List<Guid>(); // Trả về danh sách rỗng nếu không có bạn bè
    }
}