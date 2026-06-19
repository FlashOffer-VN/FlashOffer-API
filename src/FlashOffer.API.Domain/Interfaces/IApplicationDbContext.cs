using Microsoft.EntityFrameworkCore;

namespace FlashOffer.API.Domain.Interfaces;

public interface IApplicationDbContext
{
	DbSet<T> Set<T>() where T : class;
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}