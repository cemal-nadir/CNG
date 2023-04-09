#nullable enable
namespace CNG.EntityFrameworkCore.Models
{
  public class Filter
  {
    private string? _keyword;
    private int _page;

    public int PageSize { get; set; }

    public int Page
    {
      get => this._page >= 1 ? this._page : 1;
      set => this._page = value < 1 ? 1 : value;
    }

    public string? Keyword
    {
      get => this._keyword;
      set => this._keyword = value ?? "";
    }
  }
}
