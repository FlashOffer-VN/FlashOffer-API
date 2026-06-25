// GetOfferRequestsHandler.cs
using AutoMapper;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Features.OfferRequests.Queries;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Models;
using MediatR;

namespace FlashOffer.API.Application.Features.OfferRequests.Handlers;

public class GetOfferRequestsHandler : IRequestHandler<GetOfferRequestsQuery, PagedList<OfferRequestResponseDto>>
{
	private readonly IRepository<OfferRequest> _repository;
	private readonly IMapper _mapper;

	public GetOfferRequestsHandler(IRepository<OfferRequest> repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<PagedList<OfferRequestResponseDto>> Handle(GetOfferRequestsQuery request, CancellationToken cancellationToken)
	{
		var predicate = request.IsOfferSent.HasValue
			? (System.Linq.Expressions.Expression<Func<OfferRequest, bool>>)(x => x.IsOfferSent == request.IsOfferSent.Value)
			: null;

		var pagedEntities = await _repository.GetPagedWithOrderAsync(
			request.Page,
			request.PageSize,
			predicate,
			x => x.CreatedAt,
			true,
			cancellationToken
		);

		var items = _mapper.Map<List<OfferRequestResponseDto>>(pagedEntities.Items);
		return new PagedList<OfferRequestResponseDto>(items, pagedEntities.TotalCount, request.Page, request.PageSize);
	}
}