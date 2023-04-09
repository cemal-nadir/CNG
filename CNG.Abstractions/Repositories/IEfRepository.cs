﻿
using CNG.Abstractions.Signatures;

namespace CNG.Abstractions.Repositories
{
  public interface IEfRepository<TEntity, in TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>, new()
    where TKey : IEquatable<TKey>
  {
    IQueryable<TEntity> Table { get; }

    IQueryable<TEntity> AsNoTracking { get; }
  }
}
