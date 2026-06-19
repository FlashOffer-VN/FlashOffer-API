using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;

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
		entity.Status = "Pending";

		await _repository.AddAsync(entity);
		await _repository.SaveChangesAsync();

		return _mapper.Map<PurchaseRequestResponseDto>(entity);
	}
}