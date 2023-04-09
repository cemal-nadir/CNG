#nullable enable
namespace CNG.Extensions
{
  public static class GuidExtensions
  {
    public static Guid ToGuid(this string? source)
    {
      source ??= Guid.Empty.ToString();
      Guid.TryParse(source.Trim(), out var result);
      return result;
    }
  }
}
