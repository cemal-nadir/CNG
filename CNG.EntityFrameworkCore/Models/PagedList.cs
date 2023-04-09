#nullable enable
namespace CNG.EntityFrameworkCore.Models
{
  public class PagedList<T>
  {
    public PagedList()
    {
    }

    public PagedList(List<T> data, long count, Filter filter, Dictionary<string, int>? grouped = null)
    {
      this.TotalCount = count;
      this.PageSize = filter.PageSize;
      this.CurrentPage = filter.Page;
      this.TotalPage = count > 0L ? (int) Math.Ceiling(count / (Decimal) filter.PageSize) : 0;
      this.Data = data;
      this.Grouped = grouped;
      this.First = filter.Page > 1;
      this.Prior = filter.Page > 1;
      this.Next = this.TotalPage > filter.Page;
      this.Last = this.TotalPage > filter.Page;
      this.LastCachedDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
    }

    public List<T>? Data { get; }

    public Dictionary<string, int>? Grouped { get; }

    public int CurrentPage { get; }

    public int TotalPage { get; }

    public int PageSize { get; }

    public bool First { get; }

    public bool Next { get; }

    public bool Prior { get; }

    public bool Last { get; }

    public long TotalCount { get; }

    public string? LastCachedDate { get; }
  }
}
