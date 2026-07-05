using FlashOffer.API.Domain.Entities;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface IUserService
{
    Task<Guid> GetOrCreateUserAsync(string fullName, string phone, string? email = null);
    Task<User?> GetCurrentUserAsync();
}