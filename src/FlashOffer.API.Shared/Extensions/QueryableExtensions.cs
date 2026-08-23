using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FlashOffer.API.Shared.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    /// Include nhiều navigation properties cùng lúc
    /// </summary>
    public static IQueryable<T> IncludeMultiple<T>(
        this IQueryable<T> query,
        params Expression<Func<T, object>>[] includes)
        where T : class
    {
        if (includes == null || includes.Length == 0)
            return query;

        var result = query;
        foreach (var include in includes)
        {
            result = result.Include(include);
        }
        return result;
    }

    /// <summary>
    /// Include + ThenInclude với cú pháp ngắn
    /// </summary>
    public static IQueryable<T> IncludeThen<T, TProperty, TThen>(
        this IQueryable<T> query,
        Expression<Func<T, TProperty>> include,
        Expression<Func<TProperty, TThen>> thenInclude)
        where T : class
    {
        return query.Include(include).ThenInclude(thenInclude);
    }
}