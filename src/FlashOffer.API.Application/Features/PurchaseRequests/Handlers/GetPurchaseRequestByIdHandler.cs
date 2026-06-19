using MediatR;
using AutoMapper;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Features.PurchaseRequests.Queries;
using FlashOffer.API.Application.DTOs.responses;

namespace FlashOffer.API.Application.Features.PurchaseRequests.Handlers;

public class GetPurchaseRequestByIdHandler : IRequestHandler<GetPurchaseRequestByIdQuery, PurchaseRequestResponseDto>
{
	private readonly IRepository<PurchaseRequest> _repository;
	private readonly IMapper _mapper;

	public GetPurchaseRequestByIdHandler(IRepository<PurchaseRequest> repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<PurchaseRequestResponseDto> Handle(GetPurchaseRequestByIdQuery request, CancellationToken cancellationToken)
	{
		var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
		if (entity == null || entity.IsDeleted)
			return null!;

		return _mapper.Map<PurchaseRequestResponseDto>(entity);
	}
}