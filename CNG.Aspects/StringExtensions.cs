using CNG.Extensions;
namespace CNG.Aspects
{
  public static class StringExtensions
  {
    public static string ReplaceService(this string source)
    {
      source = source.Replace("CapService", "");
      source = source.Replace("HandlerService", "");
      return source.Right(7) == "Service" ? source.Left(source.Length - 7) : source;
    }

    public static string ReplaceAsync(this string source) => source.Right(5) == "Async" ? source.Left(source.Length - 5) : source;
  }
}
