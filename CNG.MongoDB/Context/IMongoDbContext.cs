#nullable enable
using CNG.Abstractions.Signatures;
using MongoDB.Driver;

namespace CNG.MongoDB.Context
{
  public interface IMongoDbContext
  {
    Task<IMongoCollection<TEntity>> GetCollectionAsync<TEntity, TKey>(
      CancellationToken cancellationToken = default (CancellationToken))
      where TEntity : IEntity<TKey>;

    Task DropCollectionAsync<TEntity>(CancellationToken cancellationToken = default (CancellationToken));
  }
}
