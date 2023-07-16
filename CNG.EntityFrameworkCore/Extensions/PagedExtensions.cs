
using AutoMapper;
using CNG.EntityFrameworkCore.Models;

namespace CNG.EntityFrameworkCore.Extensions
{
  public static class PagedExtensions
  {
    public static IQueryable<TEntity> SkipTake<TEntity>(
      this IQueryable<TEntity> source,
      Filter filter)
    {
      return source.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
    }

    public static PagedList<TDto> ToPagedList<TEntity, TDto>(
      this IQueryable<TEntity> source,
      Filter filter,
      IMapper mapper)
    {
      var count = source.Count();
      var list = source.SkipTake(filter).ToList();
      var data = mapper.Map<List<TDto>>(list);
      filter.PageSize = filter.PageSize > count ? count : filter.PageSize;
      return new PagedList<TDto>(data, count, filter);
    }
  }
}
