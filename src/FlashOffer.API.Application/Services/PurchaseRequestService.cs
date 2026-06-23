using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Models;

namespace FlashOffer.API.Application.Services;

public class PurchaseRequestService : IPurchaseRequestService
{
	private readonly IRepository<PurchaseRequest> _repository;
	private readonly IMapper _mapper;

	public PurchaseRequestService(IRepository<PurchaseRequest> repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<PurchaseRequestResponseDto> CreateAsync(CreatePurchaseRequestDto request)
	{
		var entity = _mapper.Map<PurchaseRequest>(request);
		entity.Status = PurchaseRequestStatus.Pending;

		await _repository.AddAsync(entity);
		await _repository.SaveChangesAsync();

		return _mapper.Map<PurchaseRequestResponseDto>(entity);
	}

	public async Task<PagedList<PurchaseRequestResponseDto>> GetPagedAsync(PurchaseRequestQueryDto query)
	{
		var predicate = BuildPredicate(query);
		var pagedEntities = await _repository.GetPagedWithOrderAsync(
			pageNumber: query.Page,
			pageSize: query.PageSize,
			predicate: predicate,
			orderBy: x => x.CreatedAt,
			isDescending: true
		);

		var items = _mapper.Map<List<PurchaseRequestResponseDto>>(pagedEntities.Items);
		return new PagedList<PurchaseRequestResponseDto>(
			items,
			pagedEntities.TotalCount,
			query.Page,
			query.PageSize
		);
	}

	private System.Linq.Expressions.Expression<Func<PurchaseRequest, bool>>? BuildPredicate(PurchaseRequestQueryDto query)
	{
		if (query.Status.HasValue)
		{
			return x => x.Status == query.Status.Value;
		}
		return null;
	}
}