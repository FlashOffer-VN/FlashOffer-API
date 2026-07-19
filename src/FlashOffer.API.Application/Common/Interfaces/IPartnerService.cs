using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.DTOs.Responses;
using FlashOffer.API.Domain.Models;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface IPartnerService
{
    Task<PartnerRegisterResponse> RegisterAsync(PartnerRegisterRequest request);
    Task<bool> IsReferralCodeValidAsync(string code);
    Task<PagedList<PartnerResponseDto>> GetPagedAsync(PartnerFilterRequest filter);
    Task<PartnerDetailResponseDto?> GetDetailAsync(Guid id);
    Task<PartnerResponseDto> ApproveAsync(Guid id);
    Task<PartnerResponseDto> RejectAsync(Guid id);
    Task<PartnerResponseDto> ActivateAsync(Guid id);
}