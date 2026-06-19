using AutoMapper;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Features.PurchaseRequests.Commands;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using MediatR;

namespace FlashOffer.API.Application.Features.PurchaseRequests.Handlers;

public class CreatePurchaseRequestHandler : IRequestHandler<CreatePurchaseRequestCommand, PurchaseRequestResponseDto>
{
	private readonly IRepository<PurchaseRequest> _repository;
	private readonly IMapper _mapper;

	public CreatePurchaseRequestHandler(IRepository<PurchaseRequest> repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<PurchaseRequestResponseDto> Handle(CreatePurchaseRequestCommand request, CancellationToken cancellationToken)
	{
		var entity = _mapper.Map<PurchaseRequest>(request);
		entity.Status = "Pending";

		await _repository.AddAsync(entity);
		await _repository.SaveChangesAsync();

		return _mapper.Map<PurchaseRequestResponseDto>(entity);
	}
}