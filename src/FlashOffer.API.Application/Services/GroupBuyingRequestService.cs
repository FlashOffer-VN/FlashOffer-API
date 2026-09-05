// src/FlashOffer.API.Application/Services/GroupBuyingRequestService.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Extensions;
using FlashOffer.API.Application.Common.Helpers;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Common.Mappings;
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
    private readonly IQueryService _queryService;

    public GroupBuyingRequestService(
        IRepository<GroupBuyingRequest> repository,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IUserService userService,
        IQueryService queryService)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userService = userService;
        _queryService = queryService;
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
        entity.GroupBuyingRequestCode = CodeGenerator.Generate("GBR");
        entity.UserId = Guid.Parse(userId);
        entity.CurrentPeopleCount = 1;
        entity.Status = GroupBuyingStatus.Pending;

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return _mapper.Map<GroupBuyingRequestResponseDto>(entity);
    }

    public async Task<PagedList<GroupBuyingRequestResponseDto>> GetPagedAsync(GetGroupBuyingRequestsQueryDto query)
    {
        // Admin - lấy tất cả; User - chỉ lấy của mình
        var userId = _currentUserService.IsInRole("Admin")
            ? null
            : _currentUserService.UserId;

        if (!_currentUserService.IsInRole("Admin") && string.IsNullOrEmpty(userId))
            return new PagedList<GroupBuyingRequestResponseDto>(new List<GroupBuyingRequestResponseDto>(), 0, query.Page, query.PageSize);

        var q = _queryService.GetQueryableNoTracking<GroupBuyingRequest>()
            .WhereIf(userId != null, x => x.UserId == Guid.Parse(userId!));

        var result = await q.ToPagedListAsync(
            query.Page, query.PageSize,
            query.SortBy, query.SortOrder,
            defaultSortBy: "CreatedAt");

        return _mapper.MapPagedList<GroupBuyingRequest, GroupBuyingRequestResponseDto>(result);
    }
}