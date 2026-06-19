using AutoMapper;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Features.OfferRequests.Commands;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using MediatR;

namespace FlashOffer.API.Application.Features.OfferRequests.Handlers;

public class CreateOfferRequestHandler : IRequestHandler<CreateOfferRequestCommand, OfferRequestResponseDto>
{
	private readonly IRepository<OfferRequest> _repository;
	private readonly IMapper _mapper;

	public CreateOfferRequestHandler(IRepository<OfferRequest> repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<OfferRequestResponseDto> Handle(CreateOfferRequestCommand request, CancellationToken cancellationToken)
	{
		var entity = _mapper.Map<OfferRequest>(request);
		entity.IsOfferSent = false;

		await _repository.AddAsync(entity);
		await _repository.SaveChangesAsync();

		return _mapper.Map<OfferRequestResponseDto>(entity);
	}
}