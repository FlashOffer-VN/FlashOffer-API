// GetOfferRequestsHandler.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Extensions;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Features.OfferRequests.Queries;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FlashOffer.API.Application.Features.OfferRequests.Handlers;

public class GetOfferRequestsHandler : IRequestHandler<GetOfferRequestsQuery, PagedList<OfferRequestResponseDto>>
{
	private readonly IQueryService _queryService;
	private readonly IMapper _mapper;

	public GetOfferRequestsHandler(IQueryService queryService, IMapper mapper)
	{
		_queryService = queryService;
		_mapper = mapper;
	}

	public async Task<PagedList<OfferRequestResponseDto>> Handle(GetOfferRequestsQuery request, CancellationToken cancellationToken)
	{
		var search = request.Search?.Trim();
		var includeDeleted = request.IncludeDeleted == true;

		IQueryable<OfferRequest> source = includeDeleted
			? _queryService.GetQueryableNoTracking<OfferRequest>().IgnoreQueryFilters().Where(x => x.IsDeleted)
			: _queryService.GetAllNoTracking<OfferRequest>();

		var q = source
			.WhereIf(request.IsOfferSent.HasValue && !includeDeleted, x => x.IsOfferSent == request.IsOfferSent!.Value)
			.WhereIf(request.Status.HasValue && !includeDeleted, x => x.Status == request.Status!.Value)
			.WhereIf(!string.IsNullOrEmpty(search), x =>
				x.ProductName.Contains(search!) ||
				x.FullName.Contains(search!) ||
				x.Phone.Contains(search!) ||
				(x.Email != null && x.Email.Contains(search!)) ||
				(x.OfferRequestCode != null && x.OfferRequestCode.Contains(search!)))
			.WhereIf(request.FromDate.HasValue, x => x.CreatedAt >= request.FromDate!.Value.Date.ToUniversalTime())
			.WhereIf(request.ToDate.HasValue, x => x.CreatedAt < request.ToDate!.Value.Date.AddDays(1).ToUniversalTime());

		var pagedEntities = await q.ToPagedListAsync(
			request.Page,
			request.PageSize,
			request.SortBy,
			request.SortOrder,
			defaultSortBy: "CreatedAt",
			cancellationToken);

		return _mapper.MapPagedList<OfferRequest, OfferRequestResponseDto>(pagedEntities);
	}
}