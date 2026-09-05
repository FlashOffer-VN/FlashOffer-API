using AutoMapper;
using FlashOffer.API.Application.DTOs.requests;
using FlashOffer.API.Application.DTOs.responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Models;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface IAuditLogQueryService
{
    Task<PagedList<AuditLogDto>> GetEntityLogsAsync(AuditLogQueryDto query, CancellationToken cancellationToken = default);
    Task<AuditLogDto?> GetEntityLogByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedList<AuthAuditLogDto>> GetAuthLogsAsync(AuthAuditLogQueryDto query, CancellationToken cancellationToken = default);
    Task<AuthAuditLogDto?> GetAuthLogByIdAsync(Guid id, CancellationToken cancellationToken = default);
}