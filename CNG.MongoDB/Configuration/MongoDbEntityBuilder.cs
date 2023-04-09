#nullable enable
using CNG.Abstractions.Signatures;

namespace CNG.MongoDB.Configuration
{
  public sealed class MongoDbEntityBuilder<TEntity, TKey> where TEntity : IEntity<TKey>
  {
    public MongoDbIndexContext<TEntity, TKey> Indexes { get; } = new();

    public IList<TEntity> Seed { get; } = new List<TEntity>();
  }
}
