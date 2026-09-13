using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Models;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface ICompanyService
{
    Task<Company?> AddOrUpdateFromLegacyAsync(
        string? name,
        string? taxCode,
        string? address,
        string? website,
        Guid? businessFieldId = null,
        FlashOffer.API.Domain.Enums.BusinessType? businessType = null,
        FlashOffer.API.Domain.Enums.CompanySize? companySize = null);

    Task<CompanyResponseDto> CreateAsync(CreateCompanyDto request);
    Task<CompanyResponseDto> UpdateAsync(Guid id, UpdateCompanyDto request);
    Task<PagedList<CompanyResponseDto>> GetPagedAsync(int pageNumber, int pageSize, string? search = null);
}
