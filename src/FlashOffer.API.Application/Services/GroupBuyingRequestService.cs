// src/FlashOffer.API.Application/Services/GroupBuyingRequestService.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Models;
using FlashOffer.API.Shared.Common.Interfaces;

namespace FlashOffer.API.Application.Services;

public class GroupBuyingRequestService : IGroupBuyingRequestService
{
    private readonly IRepository<GroupBuyingRequest> _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserService _userService;

    public GroupBuyingRequestService(
        IRepository<GroupBuyingRequest> repository,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IUserService userService)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userService = userService;
    }

    public async Task<GroupBuyingRequestResponseDto> CreateAsync(CreateGroupBuyingRequestDto request)
    {
        // 1. Lấy UserId từ token (nếu có)
        var userId = _currentUserService.UserId;

        // 2. Nếu chưa đăng nhập, tạo User ngầm
        if (string.IsNullOrEmpty(userId))
        {
            var userGuid = await _userService.GetOrCreateUserAsync(
                request.FullName,
                request.Phone,
                request.Email
            );

            userId = userGuid.ToString();
        }

        // 3. Map và gán UserId
        var entity = _mapper.Map<GroupBuyingRequest>(request);
        entity.UserId = Guid.Parse(userId);
        entity.CurrentPeopleCount = 1;
        entity.Status = GroupBuyingStatus.Pending;

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return _mapper.Map<GroupBuyingRequestResponseDto>(entity);
    }

    public async Task<PagedList<GroupBuyingRequestResponseDto>> GetPagedAsync(GetGroupBuyingRequestsQueryDto query)
    {
        // Admin - lấy tất cả
        if (_currentUserService.IsInRole("Admin"))
        {
            var result = await _repository.GetPagedAsync(query.Page, query.PageSize);
            var sortedItems = result.Items.OrderByDescending(x => x.CreatedAt).ToList();
            var items = _mapper.Map<List<GroupBuyingRequestResponseDto>>(sortedItems);
            return new PagedList<GroupBuyingRequestResponseDto>(items, result.TotalCount, query.Page, query.PageSize);
        }
        // User - chỉ lấy của mình
        else
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return new PagedList<GroupBuyingRequestResponseDto>(new List<GroupBuyingRequestResponseDto>(), 0, query.Page, query.PageSize);

            var result = await _repository.GetPagedAsync(query.Page, query.PageSize, x => x.UserId == Guid.Parse(userId));
            var sortedItems = result.Items.OrderByDescending(x => x.CreatedAt).ToList();
            var items = _mapper.Map<List<GroupBuyingRequestResponseDto>>(sortedItems);
            return new PagedList<GroupBuyingRequestResponseDto>(items, result.TotalCount, query.Page, query.PageSize);
        }
    }
}