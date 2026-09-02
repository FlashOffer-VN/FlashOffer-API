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
using FlashOffer.API.Shared.Resources;

namespace FlashOffer.API.Application.Services;

public class CollaboratorService : ICollaboratorService
{
    private readonly IRepository<Collaborator> _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserService _userService;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly IStringLocalizer<ExceptionMessages> _exceptionLocalizer;
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<BusinessField> _businessFieldRepo;

    public CollaboratorService(
        IRepository<Collaborator> repository,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IUserService userService,
        IStringLocalizer<SharedResource> localizer,
        IRepository<User> userRepo,
        IStringLocalizer<ExceptionMessages> exceptionLocalizer,
        IRepository<BusinessField> businessFieldRepo)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userService = userService;
        _localizer = localizer;
        _exceptionLocalizer = exceptionLocalizer;
        _userRepo = userRepo;
        _businessFieldRepo = businessFieldRepo;
    }

    public async Task<CollaboratorResponseDto> CreateAsync(CreateCollaboratorDto request)
    {
        // Lấy hoặc tạo User (dùng UserService)
        Guid userGuid;
        var userId = _currentUserService.UserId;

        if (!string.IsNullOrEmpty(userId))
        {
            userGuid = Guid.Parse(userId);
        }
        else
        {
            // UserService sẽ tự kiểm tra phone/email và throw exception nếu trùng
            userGuid = await _userService.GetOrCreateUserWithPhonePasswordAsync(
                request.FullName,
                request.Phone,
                request.Email
            );
        }

        //  Tạo Collaborator
        var collaborator = _mapper.Map<Collaborator>(request);
        collaborator.UserId = userGuid;
        collaborator.ReferralCode = await GenerateUniqueReferralCodeAsync();
        collaborator.Status = CollaboratorStatus.Pending;
        collaborator.IsApproved = false;
        collaborator.Level = 1;

        //  Xử lý BusinessField (find or create)
        if (!string.IsNullOrEmpty(request.BusinessFieldName))
        {
            var normalizedName = request.BusinessFieldName.Trim().ToLowerInvariant();
            var existingField = await _businessFieldRepo.GetFirstAsync(
                b => b.NormalizedName == normalizedName
            );

            if (existingField != null)
            {
                collaborator.BusinessFieldId = existingField.Id;
                collaborator.BusinessFieldName = request.BusinessFieldName.Trim();
            }
            else
            {
                var newField = new BusinessField
                {
                    Id = Guid.NewGuid(),
                    Name = request.BusinessFieldName.Trim(),
                    NormalizedName = normalizedName,
                    IsActive = true
                };
                await _businessFieldRepo.AddAsync(newField);
                await _businessFieldRepo.SaveChangesAsync();
                collaborator.BusinessFieldId = newField.Id;
                collaborator.BusinessFieldName = request.BusinessFieldName.Trim();
            }
        }

        //  Xử lý Parent
        if (request.ParentCollaboratorId.HasValue)
        {
            var parent = await _repository.GetFirstAsync(c =>
                c.Id == request.ParentCollaboratorId.Value && !c.IsDeleted);

            if (parent == null)
                throw CollaboratorException.ParentNotFound(_exceptionLocalizer, request.ParentCollaboratorId.Value);

            if (!parent.IsApproved)
                throw CollaboratorException.ParentNotApproved(_exceptionLocalizer, request.ParentCollaboratorId.Value);

            if (parent.Level >= 10)
                throw CollaboratorException.LevelExceeded(_exceptionLocalizer, 10);

            if (await IsCircularReferenceAsync(request.ParentCollaboratorId.Value, userGuid))
                throw CollaboratorException.CircularReference(_exceptionLocalizer, request.ParentCollaboratorId.Value);

            collaborator.Level = parent.Level + 1;
        }

        //  Lưu
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
            throw CollaboratorException.NotFound(_exceptionLocalizer, id);

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
            throw CollaboratorException.NotFound(_exceptionLocalizer, id);

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
            throw CollaboratorException.NotFound(_exceptionLocalizer, id);

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
            throw CollaboratorException.NotFound(_exceptionLocalizer, id);

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
            throw CollaboratorException.NotFound(_exceptionLocalizer, id);

        _repository.Delete(collaborator);
        await _repository.SaveChangesAsync();
    }

    public async Task RestoreAsync(Guid id)
    {
        var collaborator = await _repository.GetFirstAsync(c => c.Id == id && c.IsDeleted);
        if (collaborator == null)
            throw CollaboratorException.NotFound(_exceptionLocalizer, id);

        _repository.Restore(collaborator);
        await _repository.SaveChangesAsync();
    }
}