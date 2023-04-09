#nullable enable
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Collections.Concurrent;
using CNG.Abstractions.Signatures;
using CNG.MongoDB.Configuration;
using CNG.MongoDB.Extensions;

namespace CNG.MongoDB.Context
{
  public class MongoDbContext : IMongoDbContext, IDisposable
  {
    private readonly SemaphoreSlim _semaphore;
    private readonly MongoDbRepositoryOptions _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMongoDatabase _database;
    private readonly ConcurrentBag<Type> _bootstrappedCollections = new ConcurrentBag<Type>();
    private bool _disposed;

    public MongoDbContext(MongoDbRepositoryOptions options, IServiceProvider serviceProvider)
    {
      this._options = options;
      this._serviceProvider = serviceProvider;
      var connectionString = options.ConnectionString;
      MongoUrl url = !string.IsNullOrEmpty(connectionString) ? new MongoUrl(connectionString) : throw new Exception("Must provide a mongo connection string");
      this._semaphore = new SemaphoreSlim(1, 1);
      this._database = new MongoClient(MongoClientSettings.FromUrl(url)).GetDatabase(options.DbName);
    }

    public async Task<IMongoCollection<TEntity>> GetCollectionAsync<TEntity, TKey>(
      CancellationToken cancellationToken = default)
      where TEntity : IEntity<TKey>
    {
      if (this._disposed)
        throw new ObjectDisposedException(nameof (MongoDbContext));
      string collectionName = this._options.GetCollectionName<TEntity>();
      IMongoCollection<TEntity> collection = this._database.GetCollection<TEntity>(collectionName);
      if (_bootstrappedCollections.Contains(typeof (TEntity)))
        return collection;
      try
      {
        await _semaphore.WaitAsync(cancellationToken);
        if (_bootstrappedCollections.Contains(typeof (TEntity)))
          return collection;
        IEnumerable<IMongoDbEntityConfiguration<TEntity, TKey>> configurations = this._serviceProvider.GetServices<IMongoDbEntityConfiguration<TEntity, TKey>>();
        MongoDbEntityBuilder<TEntity, TKey> builder = new MongoDbEntityBuilder<TEntity, TKey>();
        foreach (IMongoDbEntityConfiguration<TEntity, TKey> configuration in configurations)
          configuration.Configure(builder);
        IEnumerable<Task<string>> indexTasks = builder.Indexes.Select((Func<CreateIndexModel<TEntity>, Task<string>>) (index => collection.Indexes.CreateOneAsync(index, cancellationToken: cancellationToken)));
        await Task.WhenAll(indexTasks);
        IEnumerable<Task> seedTasks = builder.Seed.Select((Func<TEntity, Task>) (async seed =>
        {
          IAsyncCursor<TEntity> cursor = await collection.FindAsync(Builders<TEntity>.Filter.IdEq(seed.Id), cancellationToken: cancellationToken);
          if (await cursor.AnyAsync(cancellationToken))
          {
          }
          else
          {
            await collection.InsertOneAsync(seed, null, cancellationToken);
          }
        }));
        await Task.WhenAll(seedTasks);
        _bootstrappedCollections.Add(typeof (TEntity));
        return collection;
      }
      finally
      {
        _semaphore.Release();
      }
    }

    public async Task DropCollectionAsync<TEntity>(CancellationToken cancellationToken = default (CancellationToken))
    {
      string collectionName = this._options.GetCollectionName<TEntity>();
      await this._database.DropCollectionAsync(collectionName, cancellationToken);
    }

    public void Dispose()
    {
      if (this._disposed)
        return;
      this._disposed = true;
      this._semaphore.Dispose();
    }
  }
}
