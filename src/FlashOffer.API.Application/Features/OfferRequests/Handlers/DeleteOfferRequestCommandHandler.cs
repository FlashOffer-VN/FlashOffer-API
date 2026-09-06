// src/FlashOffer.API.Application/Features/OfferRequests/Handlers/DeleteOfferRequestCommandHandler.cs
using AutoMapper;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Features.OfferRequests.Commands;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Features.OfferRequests.Handlers;

public class DeleteOfferRequestCommandHandler : IRequestHandler<DeleteOfferRequestCommand, OfferRequestResponseDto>
{
	private readonly IRepository<OfferRequest> _repository;
	private readonly IMapper _mapper;
	private readonly IStringLocalizer<SharedResource> _localizer;

	public DeleteOfferRequestCommandHandler(
		IRepository<OfferRequest> repository,
		IMapper mapper,
		IStringLocalizer<SharedResource> localizer)
	{
		_repository = repository;
		_mapper = mapper;
		_localizer = localizer;
	}

	public async Task<OfferRequestResponseDto> Handle(DeleteOfferRequestCommand request, CancellationToken cancellationToken)
	{
		var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
		if (entity == null || entity.IsDeleted)
			throw new NotFoundException(_localizer["OfferRequestNotFound"]);

		_repository.Delete(entity);
		await _repository.SaveChangesAsync(cancellationToken);

		return _mapper.Map<OfferRequestResponseDto>(entity);
	}
}