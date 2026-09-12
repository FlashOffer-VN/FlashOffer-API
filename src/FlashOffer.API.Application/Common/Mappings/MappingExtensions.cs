using AutoMapper;
using FlashOffer.API.Domain.Models;

namespace FlashOffer.API.Application.Common.Mappings;

public static class MappingExtensions
{
	public static PagedList<TDestination> MapPagedList<TSource, TDestination>(this IMapper mapper, PagedList<TSource> source)
	{
		var items = mapper.Map<List<TDestination>>(source.Items);
		// Truyền source.PageSize thay vì source.Items.Count — trước đây truyền Items.Count
		// làm pageSize/totalPages trả về sai ở trang cuối (fix #143).
		return new PagedList<TDestination>(items, source.TotalCount, source.PageNumber, source.PageSize);
	}
}