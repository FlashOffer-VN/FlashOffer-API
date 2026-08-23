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
        var query = await _repository.GetPagedWithOrderAsync(
        filter.PageNumber,
        filter.PageSize,
        x => (string.IsNullOrEmpty(filter.Search) ||
              x.FullName.Contains(filter.Search) ||
              x.Email.Contains(filter.Search) ||
              x.Phone.Contains(filter.Search)) &&
             (!filter.Status.HasValue || x.Status == filter.Status.Value),
        x => x.CreatedAt,
        true);

        var items = _mapper.Map<List<CtvResponseDto>>(query.Items);
        return new PagedList<CtvResponseDto>(items, query.TotalCount, query.PageNumber, query.PageSize);
    }

    public async Task<CtvDetailResponseDto?> GetDetailAsync(Guid id)
    {
        var entity = await _repository.GetFirstWithIncludesAsync(
        x => x.Id == id,
        includes: query => query.Include(x => x.User));

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

    private async Task<Collaborator> GetAndValidateAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException(_localizer["CTV_NotFound"]);
        return entity;
    }
}