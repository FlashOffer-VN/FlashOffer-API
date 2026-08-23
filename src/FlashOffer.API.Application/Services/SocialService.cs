using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.DTOs.Responses;
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

    public SocialService(
        IRepository<SocialPost> postRepository,
        IRepository<User> userRepository,
        IRepository<Tag> tagRepository,
        IRepository<PostTag> postTagRepository,
        IMapper mapper,
        IStringLocalizer<SharedResource> localizer,
        ICurrentUserService currentUserService)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _tagRepository = tagRepository;
        _postTagRepository = postTagRepository;
        _mapper = mapper;
        _localizer = localizer;
        _currentUserService = currentUserService;
    }

    public async Task<PagedList<PostResponse>> GetPostsAsync(GetPostsQuery query)
    {
        var userId = _currentUserService.UserId;
        var isAdmin = _currentUserService.IsInRole("Admin");

        // Xây dựng predicate - Dùng ExpressionExtensions.And()
        Expression<Func<SocialPost, bool>>? predicate = null;

        if (query.Type.HasValue)
            predicate = predicate.And(p => p.Type == query.Type.Value);

        if (!string.IsNullOrEmpty(query.Tag))
            predicate = predicate.And(p => p.PostTags.Any(pt => pt.Tag.Name == query.Tag));

        if (!isAdmin && !string.IsNullOrEmpty(userId))
        {
            var userGuid = Guid.Parse(userId);
            predicate = predicate.And(p => p.Privacy == PrivacyType.Public || p.AuthorId == userGuid);
        }
        else if (!isAdmin)
        {
            predicate = predicate.And(p => p.Privacy == PrivacyType.Public);
        }

        predicate ??= p => true;

        // Lấy dữ liệu - Dùng QueryableExtensions
        var posts = await _postRepository.GetPagedWithIncludesAsync(
            query.PageNumber,
            query.PageSize,
            includes: q => q
                .Include(p => p.Author)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag),
            predicate: predicate,
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
            query.PageNumber,
            query.PageSize
        );
    }

    public async Task<PostResponse> GetPostByIdAsync(Guid id)
    {
        var post = await _postRepository.GetFirstWithIncludesAsync(
            p => p.Id == id,
            includes: q => q
                .Include(p => p.Author)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
        );

        if (post == null)
        {
            throw new NotFoundException(_localizer["Social_NotFound"]);
        }

        var response = _mapper.Map<PostResponse>(post);
        response.Author = _mapper.Map<AuthorDto>(post.Author);
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

        var createdPost = await _postRepository.GetFirstWithIncludesAsync(
            p => p.Id == post.Id,
            includes: q => q
                .Include(p => p.Author)
                .Include(p => p.PostTags)
                    .ThenInclude(pt => pt.Tag)
        );

        var response = _mapper.Map<PostResponse>(createdPost);
        response.Author = _mapper.Map<AuthorDto>(createdPost.Author);
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
        {
            throw new NotFoundException(_localizer["Social_NotFound"]);
        }

        var userId = _currentUserService.UserId;
        var isAdmin = _currentUserService.IsInRole("Admin");

        if (post.AuthorId.ToString() != userId && !isAdmin)
        {
            throw new ForbiddenException(_localizer["Social_NotAuthorized"]);
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
}