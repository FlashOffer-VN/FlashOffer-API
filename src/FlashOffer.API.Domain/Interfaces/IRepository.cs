using System.Linq.Expressions;
using FlashOffer.API.Domain.Models;

namespace FlashOffer.API.Domain.Interfaces;

public interface IRepository<T> where T : class
{
	Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
	Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

	// Các overload GetPagedAsync
	Task<PagedList<T>> GetPagedAsync(
		int pageNumber,
		int pageSize,
		Expression<Func<T, bool>>? predicate = null,
		CancellationToken cancellationToken = default);

	Task<PagedList<T>> GetPagedWithOrderAsync(
		int pageNumber,
		int pageSize,
		Expression<Func<T, bool>>? predicate,
		Expression<Func<T, object>>? orderBy,
		bool isDescending = true,
		CancellationToken cancellationToken = default);

	Task<PagedList<T>> GetPagedWithIncludesAsync(
		int pageNumber,
		int pageSize,
		Func<IQueryable<T>, IQueryable<T>>? includes = null,
		Expression<Func<T, bool>>? predicate = null,
		Expression<Func<T, object>>? orderBy = null,
		bool isDescending = true,
		CancellationToken cancellationToken = default);

	Task AddAsync(T entity, CancellationToken cancellationToken = default);
	void Update(T entity);
	void Delete(T entity);
	Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}