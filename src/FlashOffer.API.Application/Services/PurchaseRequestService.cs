using AutoMapper;
using FlashOffer.API.Application.Common.Extensions;
using FlashOffer.API.Application.Common.Helpers;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Models;
using FlashOffer.API.Shared.Exceptions;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Services;

public class PurchaseRequestService : IPurchaseRequestService
{
	private readonly IRepository<PurchaseRequest> _repository;
	private readonly IMapper _mapper;
	private readonly IStringLocalizer<SharedResource> _localizer;
	private readonly IQueryService _queryService;

	public PurchaseRequestService(
		IRepository<PurchaseRequest> repository,
		IMapper mapper,
		IStringLocalizer<SharedResource> stringLocalizer,
		IQueryService queryService)
	{
		_repository = repository;
		_mapper = mapper;
		_localizer = stringLocalizer;
		_queryService = queryService;
	}

	public async Task<PurchaseRequestResponseDto> CreateAsync(CreatePurchaseRequestDto request)
	{
		var entity = _mapper.Map<PurchaseRequest>(request);
		entity.PurchaseRequestCode = CodeGenerator.Generate("PRQ");
		entity.Status = PurchaseRequestStatus.Pending;

		await _repository.AddAsync(entity);
		await _repository.SaveChangesAsync();

		return _mapper.Map<PurchaseRequestResponseDto>(entity);
	}

	public async Task<PagedList<PurchaseRequestResponseDto>> GetPagedAsync(PurchaseRequestQueryDto query)
	{
		var search = query.Search?.Trim();

		var q = _queryService.GetAllNoTracking<PurchaseRequest>()
			.WhereIf(query.Status.HasValue, x => x.Status == query.Status!.Value)
			.WhereIf(!string.IsNullOrEmpty(search), x =>
				(x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(search!)) ||
				x.ProductName.Contains(search!) ||
				x.FullName.Contains(search!) ||
				x.Phone.Contains(search!) ||
				(x.Email != null && x.Email.Contains(search!)));

		var pagedEntities = await q.ToPagedListAsync(
			query.Page,
			query.PageSize,
			query.SortBy,
			query.SortOrder,
			defaultSortBy: "CreatedAt"
		);

		return _mapper.MapPagedList<PurchaseRequest, PurchaseRequestResponseDto>(pagedEntities);
	}

	public async Task<PurchaseRequestStatusResponseDto> UpdateStatusAsync(
	Guid id,
	UpdatePurchaseRequestStatusDto dto)
	{
		var entity = await _repository.GetByIdAsync(id);
		if (entity == null)
			throw new NotFoundException(_localizer["PurchaseRequestNotFound"]);

		entity.Status = dto.Status;
		entity.UpdatedAt = DateTime.UtcNow;

		_repository.Update(entity);
		await _repository.SaveChangesAsync();

		return _mapper.Map<PurchaseRequestStatusResponseDto>(entity);
	}
}