namespace CNG.Abstractions.Signatures
{
    public interface IListDto
    {
    }
    public interface IListDto<TKey> : IListDto
    {
        TKey Id { get; set; }
    }
}
