using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Application.Features.PurchaseRequests.Commands;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Common.Interfaces;
using MediatR;

namespace FlashOffer.API.Application.Features.PurchaseRequests.Handlers;

public class CreatePurchaseRequestHandler : IRequestHandler<CreatePurchaseRequestCommand, PurchaseRequestResponseDto>
{
    private readonly IRepository<PurchaseRequest> _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserService _userService;

    public CreatePurchaseRequestHandler(
        IRepository<PurchaseRequest> repository,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IUserService userService)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userService = userService;
    }

    public async Task<PurchaseRequestResponseDto> Handle(CreatePurchaseRequestCommand request, CancellationToken cancellationToken)
    {
        // 1. Lấy UserId từ token (string)
        var userIdString = _currentUserService.UserId;
        Guid userId;

        // 2. Nếu chưa đăng nhập, tạo User ngầm
        if (string.IsNullOrEmpty(userIdString))
        {
            userId = await _userService.GetOrCreateUserAsync(
                request.FullName,
                request.Phone,
                request.Email);
        }
        else
        {
            userId = Guid.Parse(userIdString);
        }

        // 3. Tạo entity và gán UserId
        var entity = _mapper.Map<PurchaseRequest>(request);
        entity.UserId = userId;
        entity.Status = PurchaseRequestStatus.Pending;

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return _mapper.Map<PurchaseRequestResponseDto>(entity);
    }
}