using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FlashOffer.API.Domain.Interfaces;

public interface IApplicationDbContext
{
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    // ✅ Thêm property Database
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }
}