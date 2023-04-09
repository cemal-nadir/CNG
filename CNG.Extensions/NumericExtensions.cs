#nullable enable
namespace CNG.Extensions
{
  public static class NumericExtensions
  {
    public static short ToShort(this string? source)
    {
      source ??= "0";
      short.TryParse(source.Trim(), out var result);
      return result;
    }

    public static int ToInt(this string? source)
    {
      source ??= "0";
      int.TryParse(source.Trim(), out var result);
      return result;
    }

    public static long ToLong(this string? source)
    {
      source ??= "0";
      long.TryParse(source.Trim(), out var result);
      return result;
    }

    public static double ToDouble(this string? source)
    {
      source ??= "0";
      if (source.IndexOf(",", StringComparison.Ordinal) > -1)
        source = source.Replace(",", ".");
      double.TryParse(source.Trim(), out var result);
      return result;
    }

    public static Decimal ToDecimal(this string? source)
    {
      source ??= "0";
      if (source.IndexOf(",", StringComparison.Ordinal) > -1)
        source = source.Replace(",", ".");
      decimal.TryParse(source.Trim(), out var result);
      return result;
    }
  }
}
