using AutoMapper;
using FlashOffer.API.Application.Common.Exceptions;
using FlashOffer.API.Application.Common.Extensions;
using FlashOffer.API.Application.Common.Helpers;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Application.Resources;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Enums;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Models;
using FlashOffer.API.Shared.Common.Interfaces;
using FlashOffer.API.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Services;

public class PartnerService : IPartnerService
{
    private readonly IRepository<Partner> _partnerRepo;
    private readonly IUserService _userService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly IRepository<User> _userRepo;
    private readonly IQueryService _queryService;

    public PartnerService(
        IRepository<Partner> partnerRepo,
        IRepository<User> userRepo,
        IUserService userService,
        ICurrentUserService currentUserService,
        IMapper mapper,
        IStringLocalizer<SharedResource> localizer,
        IQueryService queryService)
    {
        _partnerRepo = partnerRepo;
        _userRepo = userRepo;
        _userService = userService;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _localizer = localizer;
        _queryService = queryService;
    }

    public async Task<PartnerRegisterResponse> RegisterAsync(PartnerRegisterRequest request)
    {
        // 1. Kiểm tra referral code (nếu có)
        if (!string.IsNullOrEmpty(request.ReferralCode))
        {
            var isValid = await IsReferralCodeValidAsync(request.ReferralCode);
            if (!isValid)
                throw new BusinessException(_localizer["PartnerReferralCodeInvalid"]);
        }

        // 2. Lấy hoặc tạo User
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            var userGuid = await _userService.GetOrCreateUserAsync(
                request.FullName,
                request.Phone,
                request.Email
            );
            userId = userGuid.ToString();
        }

        // 3. Map request -> Partner entity
        var partner = _mapper.Map<Partner>(request);
        partner.UserId = Guid.Parse(userId);

        partner.PartnerCode = GeneratePartnerCode();

        // 4. Map products và gán PartnerId
        var products = _mapper.Map<List<PartnerProduct>>(request.Products);
        foreach (var product in products)
        {
            product.PartnerId = partner.Id;
            product.PartnerProductCode = CodeGenerator.Generate("PRDP");
        }
        partner.Products = products;

        // 5. Form mới không thu thập chính sách hoa hồng — tạo hoa hồng mặc định
        partner.Commission = new PartnerCommission
        {
            Type = CommissionType.Percentage,
            Rate = 0,
            PartnerId = partner.Id,
            PartnerCommissionCode = CodeGenerator.Generate("PCM")
        };

        // 6. Lưu vào DB
        await _partnerRepo.AddAsync(partner);
        await _partnerRepo.SaveChangesAsync();

        // 7. Return response
        return _mapper.Map<PartnerRegisterResponse>(partner);
    }

    private string GeneratePartnerCode()
    {
        // Format: PART-{DateTime:yyMMdd}-{Random4Digits}
        var datePart = DateTime.Now.ToString("yyMMdd");
        var randomPart = new Random().Next(1000, 9999).ToString();
        return $"PART-{datePart}-{randomPart}";
    }

    public async Task<bool> IsReferralCodeValidAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return false;

        // Mã giới thiệu có thể là SĐT hoặc UserCode (không phân biệt hoa/thường với code)
        var trimmed = code.Trim();
        var user = await _userRepo.GetFirstAsync(u =>
            u.Phone == trimmed ||
            (u.UserCode != null && u.UserCode.ToUpper() == trimmed.ToUpper()));
        return user != null;
    }

    public async Task<PagedList<PartnerResponseDto>> GetPagedAsync(PartnerFilterRequest filter)
    {
        var q = _queryService.GetAllNoTracking<Partner>()
            // Include nav lĩnh vực để map BusinessFieldName trong PartnerResponseDto
            .Include(x => x.BusinessField)
            // Search filter
            .WhereIf(!string.IsNullOrEmpty(filter.Search), x =>
                x.FullName.Contains(filter.Search!) ||
                x.Email.Contains(filter.Search!) ||
                x.Phone.Contains(filter.Search!) ||
                x.CompanyName.Contains(filter.Search!) ||
                x.CompanyTax.Contains(filter.Search!) ||
                x.PartnerCode.Contains(filter.Search!) ||
                (x.ReferralCode != null && x.ReferralCode.Contains(filter.Search!)))
            // Status filter
            .WhereIf(filter.Status.HasValue, x => x.Status == filter.Status!.Value)
            // Date range filter
            .WhereIf(filter.FromDate.HasValue, x => x.CreatedAt >= filter.FromDate!.Value.Date.ToUniversalTime())
            .WhereIf(filter.ToDate.HasValue, x => x.CreatedAt < filter.ToDate!.Value.Date.AddDays(1).ToUniversalTime());

        var result = await q.ToPagedListAsync(
            filter.PageNumber,
            filter.PageSize,
            filter.SortBy,
            filter.SortOrder,
            defaultSortBy: "CreatedAt"
        );

        return _mapper.MapPagedList<Partner, PartnerResponseDto>(result);
    }

    public async Task<PartnerDetailResponseDto?> GetDetailAsync(Guid id)
    {
        var entity = await _partnerRepo.GetFirstWithIncludesAsync(
            x => x.Id == id,
            query => query
                .Include(x => x.User)
                .Include(x => x.BusinessField)
                .Include(x => x.Commission)
                .Include(x => x.Products));

        if (entity == null)
            return null;

        return _mapper.Map<PartnerDetailResponseDto>(entity);
    }

    public async Task<PartnerResponseDto> ApproveAsync(Guid id)
    {
        var entity = await _partnerRepo.GetFirstWithIncludesAsync(
            x => x.Id == id,
            query => query.Include(x => x.BusinessField));
        if (entity == null)
            throw new NotFoundException(_localizer["Partner_NotFound"]);

        if (entity.Status != PartnerStatus.Pending)
            throw new InvalidOperationException(_localizer["Partner_InvalidStatusTransition"]);

        entity.Status = PartnerStatus.Approved;
        entity.ApprovedAt = DateTime.UtcNow;

        _partnerRepo.Update(entity);
        await _partnerRepo.SaveChangesAsync();

        return _mapper.Map<PartnerResponseDto>(entity);
    }

    public async Task<PartnerResponseDto> RejectAsync(Guid id)
    {
        var entity = await _partnerRepo.GetFirstWithIncludesAsync(
            x => x.Id == id,
            query => query.Include(x => x.BusinessField));
        if (entity == null)
            throw new NotFoundException(_localizer["Partner_NotFound"]);

        if (entity.Status != PartnerStatus.Pending)
            throw new InvalidOperationException(_localizer["Partner_InvalidStatusTransition"]);

        entity.Status = PartnerStatus.Rejected;

        _partnerRepo.Update(entity);
        await _partnerRepo.SaveChangesAsync();

        return _mapper.Map<PartnerResponseDto>(entity);
    }

    public async Task<PartnerResponseDto> ActivateAsync(Guid id)
    {
        var entity = await _partnerRepo.GetFirstWithIncludesAsync(
            x => x.Id == id,
            query => query.Include(x => x.BusinessField));
        if (entity == null)
            throw new NotFoundException(_localizer["Partner_NotFound"]);

        if (entity.Status != PartnerStatus.Approved)
            throw new InvalidOperationException(_localizer["Partner_InvalidStatusTransition"]);

        entity.Status = PartnerStatus.Active;

        _partnerRepo.Update(entity);
        await _partnerRepo.SaveChangesAsync();

        return _mapper.Map<PartnerResponseDto>(entity);
    }
}