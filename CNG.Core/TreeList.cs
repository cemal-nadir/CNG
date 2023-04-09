
namespace CNG.Core
{
  public class TreeList<TKey>
  {
    public TreeList(TKey id, string? description, List<TreeList<TKey>>? subItems = null)
    {
      Id = id;
      Description = description ?? "";
      SubItems = subItems;
    }

    public TKey Id { get; }

    public string Description { get; }

    public List<TreeList<TKey>>? SubItems { get; }
  }
}
