

namespace CNG.Abstractions.Signatures
{
    public interface IEntity
    {
    }
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }
}
