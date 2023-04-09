
using System.Linq.Expressions;
using CNG.Abstractions.Signatures;

namespace CNG.Abstractions.Repositories
{
  public interface IRepository<TEntity, in TKey>
    where TEntity : IEntity<TKey>
    where TKey : notnull
  {
    Task<ICollection<TEntity>> GetAllAsync(
      Expression<Func<TEntity, bool>>? predicate = null,
      CancellationToken cancellationToken = default (CancellationToken));

    Task<bool> AnyAsync(TKey id, CancellationToken cancellationToken = default (CancellationToken));

    Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default (CancellationToken));

    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default (CancellationToken));

    Task InsertRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default (CancellationToken));

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default (CancellationToken));

    Task UpdateRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default (CancellationToken));

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default (CancellationToken));

    Task DeleteRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default (CancellationToken));
  }
}
