using AutoMapper;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Common.Mappings;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Models;

namespace FlashOffer.API.Application.Services;

public class AuditLogQueryService : IAuditLogQueryService
{
    private readonly IRepository<AuditLog> _auditLogRepository;
    private readonly IRepository<AuthAuditLog> _authAuditLogRepository;
    private readonly IMapper _mapper;

    public AuditLogQueryService(
        IRepository<AuditLog> auditLogRepository,
        IRepository<AuthAuditLog> authAuditLogRepository,
        IMapper mapper)
    {
        _auditLogRepository = auditLogRepository;
        _authAuditLogRepository = authAuditLogRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<AuditLogDto>> GetEntityLogsAsync(
        AuditLogQueryDto query,
        CancellationToken cancellationToken = default)
    {
        var result = await _auditLogRepository.GetPagedWithOrderAsync(
            query.PageNumber,
            query.PageSize,
            x =>
                (string.IsNullOrEmpty(query.EntityName) || x.EntityName.Contains(query.EntityName)) &&
                (string.IsNullOrEmpty(query.Action) || x.Action == query.Action) &&
                (string.IsNullOrEmpty(query.ActorId) || x.ActorId == query.ActorId) &&
                (!query.FromDate.HasValue || x.Timestamp >= query.FromDate.Value.ToUniversalTime()) &&
                (!query.ToDate.HasValue || x.Timestamp <= query.ToDate.Value.ToUniversalTime()),
            x => x.Timestamp,
            true,
            cancellationToken);

        return _mapper.MapPagedList<AuditLog, AuditLogDto>(result);
    }

    public async Task<AuditLogDto?> GetEntityLogByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _auditLogRepository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : _mapper.Map<AuditLogDto>(entity);
    }

    public async Task<PagedList<AuthAuditLogDto>> GetAuthLogsAsync(
        AuthAuditLogQueryDto query,
        CancellationToken cancellationToken = default)
    {
        var result = await _authAuditLogRepository.GetPagedWithOrderAsync(
            query.PageNumber,
            query.PageSize,
            x =>
                (string.IsNullOrEmpty(query.Username) || x.Username!.Contains(query.Username)) &&
                (string.IsNullOrEmpty(query.Action) || x.Action == query.Action) &&
                (!query.IsSuccess.HasValue || x.IsSuccess == query.IsSuccess) &&
                (!query.FromDate.HasValue || x.Timestamp >= query.FromDate.Value.ToUniversalTime()) &&
                (!query.ToDate.HasValue || x.Timestamp <= query.ToDate.Value.ToUniversalTime()),
            x => x.Timestamp,
            true,
            cancellationToken);

        return _mapper.MapPagedList<AuthAuditLog, AuthAuditLogDto>(result);
    }

    public async Task<AuthAuditLogDto?> GetAuthLogByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _authAuditLogRepository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : _mapper.Map<AuthAuditLogDto>(entity);
    }
}