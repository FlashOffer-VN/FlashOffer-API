using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Domain.Entities;
using FlashOffer.API.Domain.Models;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface ICtvService
{
    Task<PagedList<CtvResponseDto>> GetPagedAsync(CtvFilterRequest filter);
    Task<CtvDetailResponseDto?> GetDetailAsync(Guid id);
    Task<CtvResponseDto> ApproveAsync(Guid id);
    Task<CtvResponseDto> RejectAsync(Guid id);
}