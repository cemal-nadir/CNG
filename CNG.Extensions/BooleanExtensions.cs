#nullable enable
namespace CNG.Extensions
{
  public static class BooleanExtensions
  {
    public static bool ToBool(this string? source)
    {
      if (string.IsNullOrEmpty(source))
        source = "false";
      source = source == "1" ? "true" : source;
      bool.TryParse(source.Trim(), out var result);
      return result;
    }
  }
}
