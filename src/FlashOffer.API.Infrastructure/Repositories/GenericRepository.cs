using FlashOffer.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using FlashOffer.API.Domain.Interfaces;

namespace FlashOffer.API.Infrastructure.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
	protected readonly IApplicationDbContext _context;
	protected readonly DbSet<T> _dbSet;

	public GenericRepository(IApplicationDbContext context)
	{
		_context = context;
		_dbSet = context.Set<T>();
	}

	public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
	}

	public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
	}

	public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return await _dbSet.ToListAsync(cancellationToken);
	}

	public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
	}

	public async Task<PagedList<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
	{
		IQueryable<T> query = _dbSet;
		if (predicate != null)
			query = query.Where(predicate);
		return await PagedList<T>.CreateAsync(query, pageNumber, pageSize);
	}

	// Overload 1: Có sắp xếp
	public async Task<PagedList<T>> GetPagedWithOrderAsync(
		int pageNumber,
		int pageSize,
		Expression<Func<T, bool>>? predicate,
		Expression<Func<T, object>>? orderBy,
		bool isDescending = true,
		CancellationToken cancellationToken = default)
	{
		IQueryable<T> query = _dbSet;
		if (predicate != null)
			query = query.Where(predicate);
		if (orderBy != null)
			query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
		return await PagedList<T>.CreateAsync(query, pageNumber, pageSize);
	}

	// Overload 2: Có Include + sắp xếp
	public async Task<PagedList<T>> GetPagedWithIncludesAsync(
		int pageNumber,
		int pageSize,
		Func<IQueryable<T>, IQueryable<T>>? includes = null,
		Expression<Func<T, bool>>? predicate = null,
		Expression<Func<T, object>>? orderBy = null,
		bool isDescending = true,
		CancellationToken cancellationToken = default)
	{
		IQueryable<T> query = _dbSet;
		if (includes != null)
			query = includes(query);
		if (predicate != null)
			query = query.Where(predicate);
		if (orderBy != null)
			query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
		return await PagedList<T>.CreateAsync(query, pageNumber, pageSize);
	}

	public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
	{
		await _dbSet.AddAsync(entity, cancellationToken);
	}

	public void Update(T entity)
	{
		_dbSet.Update(entity);
	}

	public void Delete(T entity)
	{
		_dbSet.Remove(entity);
	}

	public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await _dbSet.AnyAsync(predicate, cancellationToken);
	}

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return await _context.SaveChangesAsync(cancellationToken);
	}
}