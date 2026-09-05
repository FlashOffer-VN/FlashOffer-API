// src/FlashOffer.API.Application/Features/OfferRequests/Commands/CreateOfferRequestCommandHandler.cs
using AutoMapper;
using FlashOffer.API.Application.Common.Helpers;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Features.OfferRequests.Commands;

public class CreateOfferRequestCommandHandler : IRequestHandler<CreateOfferRequestCommand, OfferRequestResponseDto>
{
    private readonly IRepository<OfferRequest> _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserService _userService;

    public CreateOfferRequestCommandHandler(
        IRepository<OfferRequest> repository,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IUserService userService)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userService = userService;
    }

    public async Task<OfferRequestResponseDto> Handle(CreateOfferRequestCommand request, CancellationToken cancellationToken)
    {
        // 1. Get or create user
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            var userIdGuid = await _userService.GetOrCreateUserAsync(
                request.FullName,
                request.Phone,
                request.Email
            );

            userId = userIdGuid.ToString();
        }

        // 2. Map to entity
        var entity = _mapper.Map<OfferRequest>(request);
        entity.OfferRequestCode = CodeGenerator.Generate("OFR");
        entity.UserId = Guid.Parse(userId);
        entity.Status = OfferStatus.Pending;

        // 3. Save to database
        await _repository.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        // 4. Map to response
        var response = _mapper.Map<OfferRequestResponseDto>(entity);
        return response;
    }
}