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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Infrastructure.Services;

public class CtvService : ICtvService
{
    private readonly IRepository<Collaborator> _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public CtvService(
        IRepository<Collaborator> repository,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IStringLocalizer<SharedResource> localizer)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _localizer = localizer;
    }

    public async Task<PagedList<CtvResponseDto>> GetPagedAsync(CtvFilterRequest filter)
    {
        // Build predicate
        System.Linq.Expressions.Expression<Func<Collaborator, bool>> predicate = x => true;
        if (!string.IsNullOrEmpty(filter.Search))
        {
            var s = filter.Search;
            var sUpper = filter.Search.ToUpperInvariant();
            predicate = x =>
                (x.FullName.Contains(s) ||
                 x.Email.Contains(s) ||
                 x.Phone.Contains(s) ||
                 (x.CollaboratorCode != null && x.CollaboratorCode.Contains(s)) ||
                 (x.BusinessFieldName != null && x.BusinessFieldName.Contains(s)) ||
                 (x.BusinessField != null && x.BusinessField.Name.Contains(s)) ||
                 (x.BusinessField != null && x.BusinessField.NormalizedName.Contains(sUpper)))
                && (!filter.Status.HasValue || x.Status == filter.Status.Value)
                && (!filter.FromDate.HasValue || x.CreatedAt >= filter.FromDate.Value.Date.ToUniversalTime())
                && (!filter.ToDate.HasValue || x.CreatedAt < filter.ToDate.Value.Date.AddDays(1).ToUniversalTime());
        }
        else
        {
            predicate = x => (!filter.Status.HasValue || x.Status == filter.Status.Value)
                        && (!filter.FromDate.HasValue || x.CreatedAt >= filter.FromDate.Value.Date.ToUniversalTime())
                        && (!filter.ToDate.HasValue || x.CreatedAt < filter.ToDate.Value.Date.AddDays(1).ToUniversalTime());
        }

        var paged = await _repository.GetPagedWithIncludesAsync(
            filter.PageNumber,
            filter.PageSize,
            includes: q => q.Include(x => x.User)
            .Include(x => x.BusinessField)
            .Include(x => x.Company),
            predicate: predicate,
            orderBy: x => x.CreatedAt,
            isDescending: true);

        var items = _mapper.Map<List<CtvResponseDto>>(paged.Items);
        return new PagedList<CtvResponseDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }

    public async Task<CtvDetailResponseDto?> GetDetailAsync(Guid id)
    {
        var entity = await _repository.GetFirstWithIncludesAsync(
        x => x.Id == id,
        includes: query => query.Include(x => x.User).Include(x => x.BusinessField).Include(x => x.Company));

        return entity == null ? null : _mapper.Map<CtvDetailResponseDto>(entity);
    }

    public async Task<CtvResponseDto> ApproveAsync(Guid id)
    {
        var entity = await GetAndValidateAsync(id);

        if (entity.Status != CollaboratorStatus.Pending)
            throw new InvalidOperationException(string.Format(
                _localizer["CTV_InvalidStatusTransition"],
                entity.Status.ToString()));

        entity.Status = CollaboratorStatus.Approved;
        entity.IsApproved = true;
        entity.ApprovedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();

        return _mapper.Map<CtvResponseDto>(entity);
    }

    public async Task<CtvResponseDto> RejectAsync(Guid id)
    {
        var entity = await GetAndValidateAsync(id);

        if (entity.Status != CollaboratorStatus.Pending)
            throw new InvalidOperationException(string.Format(
                _localizer["CTV_InvalidStatusTransition"],
                entity.Status.ToString()));

        entity.Status = CollaboratorStatus.Rejected;
        entity.IsApproved = false;

        await _repository.SaveChangesAsync();

        return _mapper.Map<CtvResponseDto>(entity);
    }

    public async Task<PagedList<CtvResponseDto>> GetPagedDeletedAsync(int pageNumber, int pageSize, string? search = null)
    {
        var query = _repository.GetQueryable();
        query = query.IgnoreQueryFilters().Where(x => x.IsDeleted);

        if (!string.IsNullOrEmpty(search))
        {
            var s = search;
            query = query.Where(x =>
                x.FullName.Contains(s) ||
                x.Phone.Contains(s) ||
                (x.Email != null && x.Email.Contains(s)) ||
                (x.CollaboratorCode != null && x.CollaboratorCode.Contains(s)) ||
                (x.BusinessFieldName != null && x.BusinessFieldName.Contains(s)) ||
                (x.BusinessField != null && x.BusinessField.Name.Contains(s)));
        }

        query = query.Include(x => x.BusinessField).Include(x => x.Company).OrderByDescending(x => x.CreatedAt);

        var paged = await PagedList<Collaborator>.CreateAsync(query, pageNumber, pageSize);
        var items = _mapper.Map<List<CtvResponseDto>>(paged.Items);
        return new PagedList<CtvResponseDto>(items, paged.TotalCount, pageNumber, pageSize);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetAndValidateAsync(id);
        _repository.Delete(entity);
        await _repository.SaveChangesAsync();
    }

    public async Task<CtvResponseDto> RestoreAsync(Guid id)
    {
        // GetByIdIncludingDeletedAsync bỏ qua global soft-delete filter → tìm được record đã xóa
        var entity = await _repository.GetByIdIncludingDeletedAsync(id);
        if (entity == null || !entity.IsDeleted)
            throw new KeyNotFoundException(_localizer["CTV_NotFound"]);

        entity.IsDeleted = false;
        entity.UpdatedAt = DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        return _mapper.Map<CtvResponseDto>(entity);
    }

    private async Task<Collaborator> GetAndValidateAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException(_localizer["CTV_NotFound"]);
        return entity;
    }
}