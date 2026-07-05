using AutoMapper;
using FlashOffer.API.Application.Common.Exceptions;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Shared.Common.Interfaces;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Infrastructure.Services;

public class PartnerService : IPartnerService
{
    private readonly IRepository<Partner> _partnerRepo;
    private readonly IUserService _userService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public PartnerService(
        IRepository<Partner> partnerRepo,
        IRepository<User> userRepo,
        IUserService userService,
        ICurrentUserService currentUserService,
        IMapper mapper,
        IStringLocalizer<SharedResource> localizer)
    {
        _partnerRepo = partnerRepo;
        _userService = userService;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<PartnerRegisterResponse> RegisterAsync(PartnerRegisterRequest request)
    {
        // 1. Kiểm tra referral code (nếu có)
        if (!string.IsNullOrEmpty(request.ReferralCode))
        {
            var isValid = await ValidateReferralCodeAsync(request.ReferralCode);
            if (!isValid)
                throw new BusinessException(_localizer["PartnerReferralCodeInvalid"]);
        }

        // 2. Lấy hoặc tạo User
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            userId = await _userService.GetOrCreateUserAsync(
                request.FullName,
                request.Phone,
                request.Email
            ).ContinueWith(t => t.Result.ToString());
        }

        // 3. Map request -> Partner entity
        var partner = _mapper.Map<Partner>(request);
        partner.UserId = Guid.Parse(userId);

        // 4. Map products và gán PartnerId
        var products = _mapper.Map<List<PartnerProduct>>(request.Products);
        foreach (var product in products)
        {
            product.PartnerId = partner.Id;
        }
        partner.Products = products; // Dùng AddRange

        // 5. Map commission và gán PartnerId
        partner.Commission = _mapper.Map<PartnerCommission>(request);
        partner.Commission.PartnerId = partner.Id;

        // 6. Lưu vào DB
        await _partnerRepo.AddAsync(partner);
        await _partnerRepo.SaveChangesAsync();

        // 7. Return response
        return _mapper.Map<PartnerRegisterResponse>(partner);
    }

    public async Task<bool> ValidateReferralCodeAsync(string code)
    {
        // Giả lập - kiểm tra trong DB hoặc cache
        var validCodes = new[] { "KINDI-ABC123", "KINDI-DEF456" };
        return await Task.FromResult(validCodes.Contains(code.ToUpper()));
    }
}