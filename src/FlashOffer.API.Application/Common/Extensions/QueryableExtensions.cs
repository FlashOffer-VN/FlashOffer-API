using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace FlashOffer.API.Application.Common.Extensions;

/// <summary>
/// Extension methods cho IQueryable — dùng free-style query cầu kỳ.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Áp predicate khi condition đúng (dùng cho filter tùy chọn từ query param).
    /// </summary>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        => condition ? source.Where(predicate) : source;

    /// <summary>
    /// Áp predicate nếu không null (dùng cho expression xây dựng dần).
    /// </summary>
    public static IQueryable<T> WhereIfNotNull<T>(this IQueryable<T> source, Expression<Func<T, bool>>? predicate)
        => predicate == null ? source : source.Where(predicate);

    /// <summary>
    /// Áp predicate khi giá trị filter khác null/default (free-style filter theo DTO).
    /// </summary>
    public static IQueryable<T> WhereIfNotNull<T, TValue>(this IQueryable<T> source, TValue? value, Expression<Func<T, bool>> predicate)
        => value is null ? source : source.Where(predicate);

    /// <summary>
    /// Skip/Take theo trang.
    /// </summary>
    public static IQueryable<T> PageBy<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 1;

        return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    /// <summary>
    /// Sort động theo chuỗi (vd "CreatedAt" + "desc"). Tên cột được validate qua reflection
    /// để tránh SQL injection / lỗi runtime; nếu không hợp lệ sẽ fallback về defaultSortBy.
    /// </summary>
    public static IQueryable<T> OrderByDynamic<T>(
        this IQueryable<T> source,
        string? sortBy,
        string? sortOrder = "asc",
        string? defaultSortBy = null)
    {
        var direction = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase) ? "descending" : "ascending";
        var column = ResolveSortColumn<T>(sortBy) ?? ResolveSortColumn<T>(defaultSortBy);

        if (string.IsNullOrEmpty(column))
            return source;

        return source.OrderBy($"{column} {direction}");
    }

    /// <summary>
    /// Sort theo expression, kiểu trỏ thẳng (vd: e => e.CreatedAt).
    /// sortOrder: "asc" | "desc".
    /// </summary>
    public static IQueryable<T> SortBy<T, TKey>(
        this IQueryable<T> source,
        Expression<Func<T, TKey>> keySelector,
        string? sortOrder = "asc")
    {
        var useDescending = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);
        return useDescending
            ? source.OrderByDescending(keySelector)
            : source.OrderBy(keySelector);
    }

    /// <summary>
    /// Sort tùy biến theo delegate — kiểu sortOrder truyền thẳng Func<IQueryable, IQueryable>.
    /// VD: query.ApplySort(o => o.OrderByDescending(x => x.CreatedAt));
    /// </summary>
    public static IQueryable<T> ApplySort<T>(
        this IQueryable<T> source,
        Func<IQueryable<T>, IQueryable<T>>? sortFunc)
    {
        return sortFunc == null ? source : sortFunc(source);
    }

    /// <summary>
    /// Left Join cho IQueryable (LINQ không có sẵn).
    /// VD: query.LeftJoin(_queryService.GetQueryable<Partner>(), a => a.UserId, b => b.Id, (a,b) => new {...})
    /// </summary>
    public static IQueryable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
        this IQueryable<TOuter> outer,
        IQueryable<TInner> inner,
        Expression<Func<TOuter, TKey>> outerKeySelector,
        Expression<Func<TInner, TKey>> innerKeySelector,
        Expression<Func<TOuter, TInner?, TResult>> resultSelector)
    {
        // Nhóm bên phải theo key; khi không khớp thì Inner = null (Left Join)
        var grouped = outer.GroupJoin(
            inner,
            outerKeySelector,
            innerKeySelector,
            (o, inners) => new LeftJoinRow<TOuter, TInner?>
            {
                Outer = o,
                Inner = inners.FirstOrDefault()
            });

        // Build projection bằng Expression.Invoke để EF Core inline được body của resultSelector
        var xParam = Expression.Parameter(typeof(LeftJoinRow<TOuter, TInner?>), "x");
        var outerAccess = Expression.PropertyOrField(xParam, "Outer");
        var innerAccess = Expression.PropertyOrField(xParam, "Inner");
        var invokeBody = Expression.Invoke(resultSelector, outerAccess, innerAccess);
        var projection = Expression.Lambda<Func<LeftJoinRow<TOuter, TInner?>, TResult>>(
            invokeBody, xParam);

        return grouped.Select(projection);
    }

    /// <summary>
    /// Row trung gian cho LeftJoin — cho phép EF Core dịch sang LEFT JOIN.
    /// </summary>
    private sealed class LeftJoinRow<TOuter, TInner>
    {
        public TOuter? Outer { get; set; }
        public TInner? Inner { get; set; }
    }

    private static string? ResolveSortColumn<T>(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return null;

        var property = typeof(T).GetProperty(sortBy.Trim(),
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        return property?.Name;
    }
}