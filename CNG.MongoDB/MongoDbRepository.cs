#nullable enable
using System.Linq.Expressions;
using CNG.Abstractions.Repositories;
using CNG.Abstractions.Signatures;
using CNG.MongoDB.Configuration;
using CNG.MongoDB.Context;
using CNG.MongoDB.Extensions;
using MongoDB.Driver;

namespace CNG.MongoDB
{
    public class MongoDbRepository<TEntity, TKey> :
      IMongoDbRepository<TEntity, TKey>
      where TEntity : IEntity<TKey>
      where TKey : IEquatable<TKey>
    {
        private readonly IMongoDbContext _context;
        private readonly MongoDbRepositoryOptions _mongoDbRepositoryOptions;
        public MongoDbRepository(IMongoDbContext context, MongoDbRepositoryOptions mongoDbRepositoryOptions)
        {
            _context = context;
            _mongoDbRepositoryOptions = mongoDbRepositoryOptions;
        }

        private Expression<Func<TEntity, bool>>? GetSoftDeleteFilter()
        {
            var property = typeof(TEntity).GetProperty("IsDeleted");
            if (property == null) return null;

            var param = Expression.Parameter(typeof(TEntity), "x");
            var propertyAccess = Expression.MakeMemberAccess(param, property);
            var falseValue = Expression.Constant(false);
            var comparison = Expression.Equal(propertyAccess, falseValue);

            var filterExpression = Expression.Lambda<Func<TEntity, bool>>(comparison, param);

            return filterExpression;

        }

        public virtual async Task<ICollection<TEntity>> GetAllAsync(
          Expression<Func<TEntity, bool>>? filter = null,
          CancellationToken cancellationToken = default(CancellationToken))
        {

            ICollection<TEntity> async = await FindAsync(filter,
                cancellationToken: cancellationToken);
            return async;
        }

        public virtual async Task<bool> AnyAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entity = await FindOneAsync(Filter.IdEq(id),
                cancellationToken: cancellationToken);

            var result = entity;
            var flag = result != null;
            return flag;
        }
        public virtual async Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {

            var oneAsync = await FindOneAsync(Filter.IdEq(id),
                cancellationToken: cancellationToken);
            return oneAsync;
        }

        public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            IMongoCollection<TEntity> collection = await GetCollectionAsync(cancellationToken);
            await collection.InsertOneAsync(entity, null, cancellationToken);
        }

        public virtual async Task InsertRangeAsync(
          List<TEntity> entities,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            IMongoCollection<TEntity> collection = await GetCollectionAsync(cancellationToken);
            await collection.InsertManyAsync(entities, cancellationToken: cancellationToken);
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entity == null)
                throw new Exception(typeof(TEntity).Name);
            IMongoCollection<TEntity> collection = await GetCollectionAsync(cancellationToken);
            FilterDefinition<TEntity> filter = Filter.IdEq(entity.Id);
            FindOneAndReplaceOptions<TEntity> andReplaceOptions = new FindOneAndReplaceOptions<TEntity>();
            andReplaceOptions.ReturnDocument = ReturnDocument.After;
            FindOneAndReplaceOptions<TEntity> option = andReplaceOptions;
            await collection.FindOneAndReplaceAsync(filter, entity, option, cancellationToken);

        }

        public virtual async Task UpdateRangeAsync(
          List<TEntity> entities,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            List<Task> tasks = entities.Select((entity => UpdateAsync(entity, cancellationToken))).ToList();
            await Task.WhenAll(tasks);
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            IMongoCollection<TEntity> collection = await GetCollectionAsync(cancellationToken);
            FilterDefinition<TEntity> filter = Filter.IdEq(entity.Id);
            if (_mongoDbRepositoryOptions.UseSoftDelete)
            {
                if (entity.GetType().GetProperty("IsDeleted") != null)
                {
                    entity.GetType().GetProperty("IsDeleted")?.SetValue(entity, true);
                    await collection.FindOneAndReplaceAsync(filter, entity,
                        new FindOneAndReplaceOptions<TEntity>()
                        {
                            ReturnDocument = ReturnDocument.After
                        }, cancellationToken);
                    return;
                }
            }
            await collection.FindOneAndDeleteAsync(filter, cancellationToken: cancellationToken);
        }

        public virtual async Task DeleteRangeAsync(
          List<TEntity> entities,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            var tasks = entities
                .Select((Func<TEntity, Task>)(entity => DeleteAsync(entity, cancellationToken)))
                .ToList();
            await Task.WhenAll(tasks);
        }

        protected async Task<TEntity?> FindOneAsync(
          FilterDefinition<TEntity> filter,
          FindOptions<TEntity, TEntity>? options = null,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            var sdFilter = GetSoftDeleteFilter();
            if (sdFilter != null && _mongoDbRepositoryOptions.UseSoftDelete)
                filter &= sdFilter;
            IMongoCollection<TEntity> collection = await GetCollectionAsync(cancellationToken);
            IAsyncCursor<TEntity> cursor = await collection.FindAsync(filter, options, cancellationToken);
            TEntity oneAsync = await cursor.FirstOrDefaultAsync(cancellationToken);
            return oneAsync;
        }

        protected async Task<ICollection<TEntity>> FindAsync(
          FilterDefinition<TEntity>? filter,
          FindOptions<TEntity, TEntity>? options = null,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            var sdFilter = GetSoftDeleteFilter();
            if (sdFilter != null && _mongoDbRepositoryOptions.UseSoftDelete)
                filter &= sdFilter;

            IMongoCollection<TEntity> collection = await GetCollectionAsync(cancellationToken);
            if (filter == null)
            {
                IAsyncCursor<TEntity> source = await collection.FindAsync(Filter.Empty, cancellationToken: cancellationToken);
                List<TEntity> listAsync = await source.ToListAsync(cancellationToken);
                return listAsync;
            }
            IAsyncCursor<TEntity> cursor = await collection.FindAsync(filter, options, cancellationToken);
            List<TEntity> listAsync1 = await cursor.ToListAsync(cancellationToken);
            return listAsync1;
        }

        public async Task<IMongoCollection<T>> GetCollectionAsync<T, TK>(
          CancellationToken cancellationToken = default(CancellationToken))
          where T : IEntity<TK>
          where TK : IEquatable<TK>
        {
            IMongoCollection<T> collectionAsync = await _context.GetCollectionAsync<T, TK>(cancellationToken);
            return collectionAsync;
        }

        protected async Task<IMongoCollection<TEntity>> GetCollectionAsync(
          CancellationToken cancellationToken = default(CancellationToken))
        {
            IMongoCollection<TEntity> collectionAsync = await _context.GetCollectionAsync<TEntity, TKey>(cancellationToken);
            return collectionAsync;
        }

        protected static FilterDefinitionBuilder<TEntity> Filter => Builders<TEntity>.Filter;

        protected static SortDefinitionBuilder<TEntity> Sort => Builders<TEntity>.Sort;

        protected static UpdateDefinitionBuilder<TEntity> Update => Builders<TEntity>.Update;

        protected static ProjectionDefinitionBuilder<TEntity> Projection => Builders<TEntity>.Projection;
    }
}
