using Microsoft.EntityFrameworkCore;

namespace FlashOffer-API.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
