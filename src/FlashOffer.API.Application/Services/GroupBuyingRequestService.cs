using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Common.Models;
using FlashOffer.API.Application.DTOs;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;

namespace FlashOffer.API.Application.Services;

public class GroupBuyingRequestService : IGroupBuyingRequestService
{
	private readonly IRepository<GroupBuyingRequest> _repository;
	private readonly IMapper _mapper;

	public GroupBuyingRequestService(IRepository<GroupBuyingRequest> repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<GroupBuyingRequestResponseDto> CreateAsync(CreateGroupBuyingRequestDto request)
	{
		var entity = _mapper.Map<GroupBuyingRequest>(request);
		entity.CurrentPeopleCount = 1;
		entity.Status = GroupBuyingStatus.Pending;

		await _repository.AddAsync(entity);
		await _repository.SaveChangesAsync();

		return _mapper.Map<GroupBuyingRequestResponseDto>(entity);
	}

	public async Task<PagedResultDto<GroupBuyingRequestResponseDto>> GetPagedAsync(GetGroupBuyingRequestsQueryDto query)
	{
		// Build predicate filter
		System.Linq.Expressions.Expression<Func<GroupBuyingRequest, bool>>? predicate = null;

		if (!string.IsNullOrEmpty(query.Status))
		{
			if (Enum.TryParse<GroupBuyingStatus>(query.Status, true, out var status))
			{
				predicate = x => x.Status == status;
			}
		}

		// Get paged data from repository
		var pagedEntities = await _repository.GetPagedAsync(
			query.Page,
			query.PageSize,
			predicate);

		// Sort after getting data (descending by CreatedAt)
		var sortedItems = pagedEntities.Items.OrderByDescending(x => x.CreatedAt).ToList();

		// Map to response DTOs
		var items = _mapper.Map<List<GroupBuyingRequestResponseDto>>(sortedItems);

		return new PagedResultDto<GroupBuyingRequestResponseDto>
		{
			Items = items,
			TotalCount = pagedEntities.TotalCount,
			PageNumber = query.Page,
			PageSize = query.PageSize
		};
	}
}