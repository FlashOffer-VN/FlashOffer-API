using FlashOffer.API.Application.DTOs.Responses;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface IBusinessFieldService
{
    Task<Guid> GetOrCreateBusinessFieldAsync(string name);
    Task<List<BusinessFieldDto>> GetActiveFieldsAsync();
}