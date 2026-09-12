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

    /// <summary>Xóa mềm đối tác.</summary>
    Task DeleteAsync(Guid id);
    /// <summary>Khôi phục đối tác đã xóa mềm.</summary>
    Task<PartnerResponseDto> RestoreAsync(Guid id);
    /// <summary>Danh sách đối tác đã xóa mềm (bỏ qua global query filter).</summary>
    Task<PagedList<PartnerResponseDto>> GetPagedDeletedAsync(int pageNumber, int pageSize, string? search = null);
}