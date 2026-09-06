// src/FlashOffer.API.Application/Features/OfferRequests/Handlers/RestoreOfferRequestCommandHandler.cs
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

public class RestoreOfferRequestCommandHandler : IRequestHandler<RestoreOfferRequestCommand, OfferRequestResponseDto>
{
	private readonly IRepository<OfferRequest> _repository;
	private readonly IMapper _mapper;
	private readonly IStringLocalizer<SharedResource> _localizer;

	public RestoreOfferRequestCommandHandler(
		IRepository<OfferRequest> repository,
		IMapper mapper,
		IStringLocalizer<SharedResource> localizer)
	{
		_repository = repository;
		_mapper = mapper;
		_localizer = localizer;
	}

	public async Task<OfferRequestResponseDto> Handle(RestoreOfferRequestCommand request, CancellationToken cancellationToken)
	{
		// FindAsync bypass global query filter → lấy được cả record đã xóa mềm.
		var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
		if (entity == null || !entity.IsDeleted)
			throw new NotFoundException(_localizer["OfferRequestNotFound"]);

		entity.IsDeleted = false;
		entity.UpdatedAt = DateTime.UtcNow;

		_repository.Update(entity);
		await _repository.SaveChangesAsync(cancellationToken);

		return _mapper.Map<OfferRequestResponseDto>(entity);
	}
}