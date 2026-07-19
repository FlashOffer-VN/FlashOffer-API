using FlashOffer.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using FlashOffer.API.Domain.Interfaces;
using FlashOffer.API.Domain.Entities;

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

    // ========== QUERY METHODS ==========
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

    // ========== PAGED METHODS ==========
    public async Task<PagedList<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;
        if (predicate != null)
            query = query.Where(predicate);
        return await PagedList<T>.CreateAsync(query, pageNumber, pageSize);
    }

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

    // ========== ADVANCED QUERY METHODS ==========
    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync(cancellationToken);
    }

    public async Task<T?> GetFirstWithIncludesAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IQueryable<T>>? includes = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;
        if (includes != null)
            query = includes(query);
        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetListWithIncludesAsync(
        Func<IQueryable<T>, IQueryable<T>>? includes = null,
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isDescending = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;
        if (predicate != null)
            query = query.Where(predicate);
        if (includes != null)
            query = includes(query);
        if (orderBy != null)
            query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<T>> FromSqlRawAsync(string sql, params object[] parameters)
    {
        return await _dbSet.FromSqlRaw(sql, parameters).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        if (typeof(BaseEntity).IsAssignableFrom(typeof(T)))
        {
            return await _dbSet
                .Where(e => ((BaseEntity)(object)e).IsDeleted)
                .ToListAsync(cancellationToken);
        }
        return await _dbSet.ToListAsync(cancellationToken);
    }

    // ========== COMMAND METHODS ==========
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void Delete(T entity)
    {
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.IsDeleted = true;
            _dbSet.Update(entity);
            return;
        }
        _dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            Delete(entity);
        }
    }

    public void Restore(T entity)
    {
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.IsDeleted = false;
            _dbSet.Update(entity);
        }
    }

    public void RestoreRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            Restore(entity);
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<T> GetQueryable()
    {
        return _context.Set<T>().AsQueryable();
    }
    public async Task<IQueryable<T>> GetQueryableAsync()
    {
        return await Task.FromResult(_context.Set<T>().AsQueryable());
    }
}