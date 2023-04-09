#nullable enable
using System.Linq.Expressions;
using CNG.Abstractions.Signatures;
using MongoDB.Driver;

namespace CNG.MongoDB.Extensions
{
  public static class FilterDefinitionBuilderExtensions
  {
    public static FilterDefinition<TEntity> IdEq<TEntity, TKey>(
      this FilterDefinitionBuilder<TEntity> filter,
      TKey id)
      where TEntity : IEntity<TKey>
    {
      return filter.Eq((Expression<Func<TEntity, TKey>>) (x => x.Id), id);
    }
  }
}
