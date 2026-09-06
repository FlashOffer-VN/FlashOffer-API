// src/FlashOffer.API.Application/Features/OfferRequests/Handlers/GetOfferRequestByIdHandler.cs
using AutoMapper;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Features.OfferRequests.Queries;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using MediatR;

namespace FlashOffer.API.Application.Features.OfferRequests.Handlers;

public class GetOfferRequestByIdHandler : IRequestHandler<GetOfferRequestByIdQuery, OfferRequestResponseDto?>
{
	private readonly IRepository<OfferRequest> _repository;
	private readonly IMapper _mapper;

	public GetOfferRequestByIdHandler(IRepository<OfferRequest> repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<OfferRequestResponseDto?> Handle(GetOfferRequestByIdQuery request, CancellationToken cancellationToken)
	{
		var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
		if (entity == null || entity.IsDeleted)
			return null;

		return _mapper.Map<OfferRequestResponseDto>(entity);
	}
}