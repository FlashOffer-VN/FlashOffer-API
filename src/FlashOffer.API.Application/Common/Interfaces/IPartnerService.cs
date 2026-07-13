using FlashOffer.API.Application.DTOs.Requests;
using FlashOffer.API.Application.DTOs.Responses;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface IPartnerService
{
    Task<PartnerRegisterResponse> RegisterAsync(PartnerRegisterRequest request);
    Task<bool> IsReferralCodeValidAsync(string code);
}