#nullable enable
using System.Linq.Expressions;
using CNG.Abstractions.Models;
using CNG.Abstractions.Repositories;
using CNG.Abstractions.Signatures;
using CNG.Core.Exceptions;
using CNG.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CNG.EntityFrameworkCore
{
    public class EfRepository<TEntity, TKey> : IEfRepository<TEntity, TKey>, IRepository<TEntity, TKey>
      where TEntity : class, IEntity<TKey>, new()
      where TKey : IEquatable<TKey>
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _entities;

        protected EfRepository(DbContext context)
        {
            this._context = context;
            this._entities = this._context.Set<TEntity>();
        }

        public IQueryable<TEntity> Table => this._entities;

        public IQueryable<TEntity> AsNoTracking => this._entities.AsNoTracking<TEntity>();

        public virtual async Task<ICollection<TEntity>> GetAllAsync(
          Expression<Func<TEntity, bool>>? predicate = null,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            if (predicate == null)
            {
                var listAsync = await this.AsNoTracking.ToListAsync<TEntity>(cancellationToken);
                return listAsync;
            }
            var listAsync1 = await this.AsNoTracking.Where<TEntity>(predicate).ToListAsync<TEntity>(cancellationToken);
            return listAsync1;
        }

        public virtual async Task<(ICollection<TEntity>,int)> GetAllByFilterAsync(SearchFilter filter,
            CancellationToken cancellationToken = default)
        {
            var countQuery = _context.Set<TEntity>().AsQueryable();

            if (filter.SearchValues.Count <= 0)
            {
               var data= await GetAllAsync(null, cancellationToken);
               return (data, data.Count);
            }
            var whereClause = GenericFilterExtensions<TEntity>.Create(filter) ??
                              throw new BadRequestException($"{nameof(SearchFilter)} can\'t be null");
            countQuery = countQuery.Where(whereClause);
            var filteredCount = await countQuery.CountAsync(cancellationToken);
            var dataQuery = GenericFilterExtensions<TEntity>.OrderByDynamicEX(countQuery, filter.OrderBy?.fieldName ?? "Id",
                filter.OrderBy?.isDescending ?? true);


            return (
                await dataQuery.Skip((filter.CurrentPage - 1) * filter.PageSize).Take(filter.PageSize)
                    .ToListAsync(cancellationToken), filteredCount);

        }
        public virtual async Task<bool> AnyAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entity1 = await this._entities.FindAsync(new object[1]
            {
        id
            }, cancellationToken);
            var entity2 = entity1;
            entity1 = default(TEntity);
            if (entity2 != null)
                this._context.Entry<TEntity>(entity2).State = EntityState.Detached;
            var flag = entity2 != null;
            entity2 = default(TEntity);
            return flag;
        }

        public virtual async Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entity1 = await this._entities.FindAsync(new object[1]
            {
        id
            }, cancellationToken);
            var entity2 = entity1;
            entity1 = default(TEntity);
            if (entity2 != null)
                this._context.Entry<TEntity>(entity2).State = EntityState.Detached;
            var async = entity2;
            entity2 = default(TEntity);
            return async;
        }

        public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entity == null)
                throw new DbNullException(typeof(TEntity).Name);
            this._context.Entry<TEntity>(entity).State = EntityState.Detached;
            EntityEntry<TEntity> entityEntry = await this._entities.AddAsync(entity, cancellationToken);
            await this.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task InsertRangeAsync(
          List<TEntity> entities,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entities == null || !entities.Any<TEntity>())
                throw new DbNullException(typeof(TEntity).Name);
            await this._entities.AddRangeAsync(entities, cancellationToken);
            await this.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entity == null)
                throw new DbNullException(typeof(TEntity).Name);
            this._context.Update<TEntity>(entity);
            await this.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateRangeAsync(
          List<TEntity> entities,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entities == null)
                throw new DbNullException(typeof(TEntity).Name);
            this._context.UpdateRange(entities);
            await this.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entity == null)
                throw new DbNullException(typeof(TEntity).Name);
            this._context.Remove<TEntity>(entity);
            await this.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteRangeAsync(
          List<TEntity> entities,
          CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!entities.Any<TEntity>())
                throw new DbNullException(typeof(TEntity).Name);
            foreach (var entity1 in entities)
            {
                var entity = entity1;
                this._entities.Remove(entity);
                entity = default(TEntity);
            }
            await this.SaveChangesAsync(cancellationToken);
        }

        private async Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var num = await this._context.SaveChangesAsync(cancellationToken);
                foreach (var entry in this._context.ChangeTracker.Entries())
                    entry.State = EntityState.Detached;
            }
            catch (DbUpdateException ex)
            {
                throw new Core.Exceptions.DbException(this.GetFullError(ex));
            }
            catch (Exception ex)
            {
                throw new Core.Exceptions.DbException(this.GetFullError(ex));
            }
        }

        private string GetFullError(Exception e)
        {
            foreach (var entry in this._context.ChangeTracker.Entries())
                entry.State = EntityState.Detached;
            return e.ToString();
        }
    }
}
