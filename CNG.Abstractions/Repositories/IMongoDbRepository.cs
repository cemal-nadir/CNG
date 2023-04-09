
using CNG.Abstractions.Signatures;

namespace CNG.Abstractions.Repositories
{
  public interface IMongoDbRepository<TEntity, in TKey> : IRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
  {
  }
}
