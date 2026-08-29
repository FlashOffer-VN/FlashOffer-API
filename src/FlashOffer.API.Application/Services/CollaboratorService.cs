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

    public CollaboratorService(
        IRepository<Collaborator> repository,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IUserService userService,
        IStringLocalizer<SharedResource> localizer,
        IRepository<User> userRepo)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userService = userService;
        _localizer = localizer;
        _userRepo = userRepo;
    }

    public async Task<CollaboratorResponseDto> CreateAsync(CreateCollaboratorDto request)
    {
        // 1. Lấy UserId (tạo mới hoặc lấy existing)
        Guid userGuid;
        var userId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            // GetOrCreateUserAsync - đã có logic kiểm tra phone/email
            userGuid = await _userService.GetOrCreateUserAsync(
                request.FullName,
                request.Phone,
                request.Email
            );
        }
        else
        {
            userGuid = Guid.Parse(userId);
        }

        // 2. Map và tạo Collaborator
        var collaborator = _mapper.Map<Collaborator>(request);
        collaborator.UserId = userGuid;
        collaborator.ReferralCode = GenerateReferralCode();
        collaborator.Status = CollaboratorStatus.Pending;
        collaborator.IsApproved = false;
        collaborator.Level = 1;

        // 3. Tính Level nếu có Parent
        if (request.ParentCollaboratorId.HasValue)
        {
            var parent = await _repository.GetByIdAsync(request.ParentCollaboratorId.Value);
            if (parent != null)
            {
                collaborator.Level = parent.Level + 1;
            }
        }

        // 4. Lưu Collaborator
        await _repository.AddAsync(collaborator);
        await _repository.SaveChangesAsync();

        return _mapper.Map<CollaboratorResponseDto>(collaborator);
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

    private string GenerateReferralCode()
    {
        return $"CTV{DateTime.Now.Ticks:X8}{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
    }
}