using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
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
using System.Linq.Expressions;

namespace FlashOffer.API.Application.Services;

public class CollaboratorService : ICollaboratorService
{
    private readonly IRepository<Collaborator> _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserService _userService;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<BusinessField> _businessFieldRepo;

    public CollaboratorService(
        IRepository<Collaborator> repository,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IUserService userService,
        IStringLocalizer<SharedResource> localizer,
        IRepository<User> userRepo,
        IRepository<BusinessField> businessFieldRepo)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userService = userService;
        _localizer = localizer;
        _userRepo = userRepo;
        _businessFieldRepo = businessFieldRepo;
    }

    public async Task<CollaboratorResponseDto> CreateAsync(CreateCollaboratorDto request)
    {
        // 1. Lấy UserId
        Guid userGuid;
        var userId = _currentUserService.UserId;

        if (!string.IsNullOrEmpty(userId))
        {
            userGuid = Guid.Parse(userId);
        }
        else
        {
            // 1a. Kiểm tra Email/Phone đã tồn tại trong User
            if (!string.IsNullOrEmpty(request.Email))
            {
                var existingByEmail = await _userRepo.GetFirstAsync(u => u.Email == request.Email);
                if (existingByEmail != null)
                    throw new BadRequestException(_localizer["Collaborator_EmailAlreadyExists"]);
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                var existingByPhone = await _userRepo.GetFirstAsync(u => u.Phone == request.Phone);
                if (existingByPhone != null)
                    throw new BadRequestException(_localizer["Collaborator_PhoneAlreadyExists"]);
            }

            userGuid = await _userService.GetOrCreateUserAsync(
                request.FullName,
                request.Phone,
                request.Email
            );
        }

        // 2. Kiểm tra User đã là Collaborator chưa
        var existingCollaborator = await _repository.GetFirstAsync(c => c.UserId == userGuid);
        if (existingCollaborator != null)
            throw new BadRequestException(_localizer["Collaborator_UserAlreadyExists"]);

        // 3. Tạo Collaborator
        var collaborator = _mapper.Map<Collaborator>(request);
        collaborator.UserId = userGuid;
        collaborator.ReferralCode = await GenerateUniqueReferralCodeAsync();
        collaborator.Status = CollaboratorStatus.Pending;
        collaborator.IsApproved = false;
        collaborator.Level = 1;

        // Xử lý BusinessField (find or create)
        if (!string.IsNullOrEmpty(request.BusinessField))
        {
            var normalizedName = request.BusinessField.Trim().ToLowerInvariant();
            var existingField = await _businessFieldRepo.GetFirstAsync(
                b => b.NormalizedName == normalizedName
            );

            if (existingField != null)
            {
                collaborator.BusinessFieldId = existingField.Id;
            }
            else
            {
                var newField = new BusinessField
                {
                    Id = Guid.NewGuid(),
                    Name = request.BusinessField.Trim(),
                    NormalizedName = normalizedName,
                    IsActive = true
                };
                await _businessFieldRepo.AddAsync(newField);
                await _businessFieldRepo.SaveChangesAsync();
                collaborator.BusinessFieldId = newField.Id;
            }
        }

        // 4. Xử lý Parent
        if (request.ParentCollaboratorId.HasValue)
        {
            var parent = await _repository.GetFirstAsync(c =>
                c.Id == request.ParentCollaboratorId.Value && !c.IsDeleted);

            if (parent == null)
                throw new NotFoundException(_localizer["Collaborator_ParentNotFound"]);

            if (!parent.IsApproved)
                throw new BadRequestException(_localizer["Collaborator_ParentNotApproved"]);

            if (parent.Level >= 10)
                throw new BadRequestException(_localizer["Collaborator_LevelExceeded"]);

            if (await IsCircularReferenceAsync(request.ParentCollaboratorId.Value, userGuid))
                throw new BadRequestException(_localizer["Collaborator_CircularReference"]);

            collaborator.Level = parent.Level + 1;
        }

        // 5. Lưu
        await _repository.AddAsync(collaborator);
        await _repository.SaveChangesAsync();

        return _mapper.Map<CollaboratorResponseDto>(collaborator);
    }

    private async Task<string> GenerateUniqueReferralCodeAsync()
    {
        string code;
        bool exists;
        do
        {
            code = $"CTV{DateTime.Now.Ticks:X8}{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
            exists = await _repository.AnyAsync(c => c.ReferralCode == code);
        } while (exists);
        return code;
    }

    private async Task<bool> IsCircularReferenceAsync(Guid parentId, Guid userId)
    {
        var currentId = parentId;
        var visitedIds = new HashSet<Guid>();

        while (currentId != Guid.Empty)
        {
            if (visitedIds.Contains(currentId))
                return true;

            visitedIds.Add(currentId);

            var parent = await _repository.GetFirstAsync(c => c.Id == currentId && !c.IsDeleted);
            if (parent == null || parent.UserId == userId)
                return parent?.UserId == userId;

            currentId = parent.ParentCollaboratorId ?? Guid.Empty;
        }
        return false;
    }

    public async Task<CollaboratorResponseDto> UpdateAsync(Guid id, UpdateCollaboratorDto request)
    {
        var collaborator = await _repository.GetByIdAsync(id);
        if (collaborator == null)
            throw new NotFoundException(_localizer["Collaborator_NotFound"]);

        _mapper.Map(request, collaborator);
        _repository.Update(collaborator);
        await _repository.SaveChangesAsync();

        return _mapper.Map<CollaboratorResponseDto>(collaborator);
    }

    public async Task<CollaboratorResponseDto> GetByIdAsync(Guid id)
    {
        var collaborator = await _repository.GetFirstWithIncludesAsync(
            c => c.Id == id,
            q => q.Include(c => c.User));

        if (collaborator == null)
            throw new NotFoundException(_localizer["Collaborator_NotFound"]);

        return _mapper.Map<CollaboratorResponseDto>(collaborator);
    }

    public async Task<PagedList<CollaboratorResponseDto>> GetPagedAsync(int page, int size, string? search = null)
    {
        Expression<Func<Collaborator, bool>> predicate = c => true;

        if (!string.IsNullOrEmpty(search))
        {
            predicate = c => c.FullName.Contains(search) ||
                             c.Phone.Contains(search) ||
                             (c.Email != null && c.Email.Contains(search));
        }

        var paged = await _repository.GetPagedWithIncludesAsync(
            page, size,
            includes: q => q.Include(c => c.User),
            predicate: predicate,
            orderBy: c => c.CreatedAt,
            isDescending: true);

        return new PagedList<CollaboratorResponseDto>(
            _mapper.Map<List<CollaboratorResponseDto>>(paged.Items),
            paged.TotalCount,
            paged.PageNumber,
            paged.PageSize);
    }

    public async Task ApproveAsync(Guid id)
    {
        var collaborator = await _repository.GetByIdAsync(id);
        if (collaborator == null)
            throw new NotFoundException(_localizer["Collaborator_NotFound"]);

        collaborator.Status = CollaboratorStatus.Approved;
        collaborator.IsApproved = true;
        collaborator.ApprovedAt = DateTime.UtcNow;

        _repository.Update(collaborator);
        await _repository.SaveChangesAsync();
    }

    public async Task RejectAsync(Guid id, string? reason = null)
    {
        var collaborator = await _repository.GetByIdAsync(id);
        if (collaborator == null)
            throw new NotFoundException(_localizer["Collaborator_NotFound"]);

        collaborator.Status = CollaboratorStatus.Rejected;
        collaborator.IsApproved = false;
        collaborator.RejectedAt = DateTime.UtcNow;
        collaborator.RejectionReason = reason;

        _repository.Update(collaborator);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var collaborator = await _repository.GetByIdAsync(id);
        if (collaborator == null)
            throw new NotFoundException(_localizer["Collaborator_NotFound"]);

        _repository.Delete(collaborator);
        await _repository.SaveChangesAsync();
    }

    public async Task RestoreAsync(Guid id)
    {
        var collaborator = await _repository.GetFirstAsync(c => c.Id == id && c.IsDeleted);
        if (collaborator == null)
            throw new NotFoundException(_localizer["Collaborator_NotFound"]);

        _repository.Restore(collaborator);
        await _repository.SaveChangesAsync();
    }
}