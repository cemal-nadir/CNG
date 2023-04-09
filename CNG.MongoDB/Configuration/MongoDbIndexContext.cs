#nullable enable
using System.Collections;
using CNG.Abstractions.Signatures;
using MongoDB.Driver;

namespace CNG.MongoDB.Configuration
{
  public sealed class MongoDbIndexContext<TEntity, TKey> : 
    ICollection<CreateIndexModel<TEntity>>
    where TEntity : IEntity<TKey>
  {
    private readonly ICollection<CreateIndexModel<TEntity>> _listOfIndex = new List<CreateIndexModel<TEntity>>();

    public IEnumerator<CreateIndexModel<TEntity>> GetEnumerator() => this._listOfIndex.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(CreateIndexModel<TEntity> item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      var name = item.Options.Name;
      if (this._listOfIndex.Any((Func<CreateIndexModel<TEntity>, bool>) (x => x.Options.Name == name)))
        throw new ArgumentException("An index with the name " + name + " has already been added", "item");
      _listOfIndex.Add(item);
    }

    public void Clear() => this._listOfIndex.Clear();

    public bool Contains(CreateIndexModel<TEntity> item) => this._listOfIndex.Contains(item);

    public void CopyTo(CreateIndexModel<TEntity>[] array, int arrayIndex) => this._listOfIndex.CopyTo(array, arrayIndex);

    public bool Remove(CreateIndexModel<TEntity> item) => this._listOfIndex.Remove(item);

    public int Count => this._listOfIndex.Count;

    public bool IsReadOnly => false;
  }
}
