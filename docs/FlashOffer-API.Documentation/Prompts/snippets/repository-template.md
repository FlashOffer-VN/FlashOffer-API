# Repository Template

// IRepository<T> is already generic, no need to create separate repository interface
// Just use IRepository<{EntityName}> directly in controller

// If you need custom query methods, create a custom repository:

// 1. Interface in Application layer
namespace FlashOffer-API.Application.Common.Interfaces;

public interface I{EntityName}Repository : IRepository<{EntityName}>
{
    Task<{EntityName}?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}

// 2. Implementation in Infrastructure layer
namespace FlashOffer-API.Infrastructure.Repositories;

public class {EntityName}Repository : GenericRepository<{EntityName}>, I{EntityName}Repository
{
    public {EntityName}Repository(IApplicationDbContext context) : base(context) { }

    public async Task<{EntityName}?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }
}

// 3. Register in Infrastructure/DependencyInjection.cs
services.AddScoped<I{EntityName}Repository, {EntityName}Repository>();
