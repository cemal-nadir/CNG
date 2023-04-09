#nullable enable
using CNG.Abstractions.Signatures;

namespace CNG.MongoDB.Configuration
{
  public interface IMongoDbEntityConfiguration<TEntity, TKey> where TEntity : IEntity<TKey>
  {
    void Configure(MongoDbEntityBuilder<TEntity, TKey> context);
  }
}
