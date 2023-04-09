
using CNG.Abstractions.Signatures;

namespace CNG.Abstractions.Repositories
{
  public interface IServiceRepository<TDto, TKey>
    where TDto : class, IDto, new()
    where TKey : notnull
  {
    Task<bool> AnyAsync(TKey id, CancellationToken cancellationToken = default (CancellationToken));

    Task<TDto> GetAsync(TKey id, CancellationToken cancellationToken = default (CancellationToken));

    Task InsertAsync(TDto dto, CancellationToken cancellationToken = default (CancellationToken));

    Task InsertRangeAsync(IEnumerable<TDto> listOfDto, CancellationToken cancellationToken = default (CancellationToken));

    Task UpdateAsync(TKey id, TDto dto, CancellationToken cancellationToken = default (CancellationToken));

    Task UpdateRangeAsync(Dictionary<TKey, TDto> listOfDto, CancellationToken cancellationToken = default (CancellationToken));

    Task DeleteAsync(TKey id, CancellationToken cancellationToken = default (CancellationToken));

    Task DeleteRangeAsync(List<TKey> listOfId, CancellationToken cancellationToken = default (CancellationToken));

    Task RemoveCacheAsync(CancellationToken cancellationToken = default (CancellationToken));
  }
}
