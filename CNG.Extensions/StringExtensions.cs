#nullable enable
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CNG.Extensions
{
  public static class StringExtensions
  {
    public static string Left(this string source, int length) => string.IsNullOrEmpty(source) ? "" : (source.Length > length ? source.Substring(0, length) : source);

    public static string Right(this string source, int length) => string.IsNullOrEmpty(source) ? "" : (source.Length > length ? source.Substring(source.Length - length, length) : source);

    public static string Pluralize(this string source)
    {
      if (string.IsNullOrEmpty(source))
        return "";
      string str1;
      if (!source.EndsWith("y", StringComparison.OrdinalIgnoreCase) || source.EndsWith("ay", StringComparison.OrdinalIgnoreCase) || source.EndsWith("ey", StringComparison.OrdinalIgnoreCase) || source.EndsWith("iy", StringComparison.OrdinalIgnoreCase) || source.EndsWith("oy", StringComparison.OrdinalIgnoreCase) || source.EndsWith("uy", StringComparison.OrdinalIgnoreCase))
      {
        if (!source.EndsWith("us", StringComparison.InvariantCultureIgnoreCase) && !source.EndsWith("ss", StringComparison.InvariantCultureIgnoreCase) && !source.EndsWith("x", StringComparison.InvariantCultureIgnoreCase) && !source.EndsWith("ch", StringComparison.InvariantCultureIgnoreCase) && !source.EndsWith("sh", StringComparison.InvariantCultureIgnoreCase))
        {
          if (!source.EndsWith("f", StringComparison.InvariantCultureIgnoreCase) || source.Length <= 1)
          {
            if (!source.EndsWith("fe", StringComparison.InvariantCultureIgnoreCase) || source.Length <= 2)
            {
              str1 = source.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) ? source : source + "s";
            }
            else
            {
              var str2 = source;
              str1 = str2.Substring(0, str2.Length - 2) + "ves";
            }
          }
          else
          {
            var str3 = source;
            str1 = str3.Substring(0, str3.Length - 1) + "ves";
          }
        }
        else
          str1 = source + "es";
      }
      else
      {
        var str4 = source;
        str1 = str4.Substring(0, str4.Length - 1) + "ies";
      }
      return str1;
    }

    public static List<string> Split(this string source, string separator)
    {
      if (string.IsNullOrEmpty(source))
        return new List<string>();
      List<string> stringList = new List<string>();
      if (source.IndexOf(separator, StringComparison.Ordinal) < 0)
      {
        stringList.Add(source);
        return stringList;
      }
      string[] source1 = source.Split(new[]
      {
        separator
      }, StringSplitOptions.None);
      stringList.AddRange(source1.Where((Func<string, bool>) (item => !string.IsNullOrEmpty(item))));
      return stringList;
    }

    public static string GetDisplayName(this Type type) => type.GetCustomAttributes(typeof (DisplayNameAttribute), true).FirstOrDefault() is DisplayNameAttribute displayNameAttribute ? displayNameAttribute.DisplayName : type.Name;

    public static string StripHtmlTags(this string source) => Regex.Replace(source, "<.*?>|&.*?;", string.Empty);

    public static string ClearSymbol(this string source)
    {
      var values = new[]
      {
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
        'I',
        'J',
        'K',
        'L',
        'M',
        'N',
        'O',
        'P',
        'R',
        'S',
        'T',
        'U',
        'V',
        'Y',
        'Z',
        'W',
        'X',
        'a',
        'b',
        'c',
        'd',
        'e',
        'f',
        'g',
        'h',
        'i',
        'j',
        'k',
        'l',
        'm',
        'n',
        'o',
        'p',
        'r',
        's',
        't',
        'u',
        'v',
        'y',
        'z',
        'w',
        'x',
        '0',
        '1',
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8',
        '9'
      };
      var stringBuilder = new StringBuilder();
      foreach (var ch in source.Where(ch => values.Contains(ch)).ToList())
        stringBuilder.Append(ch);
      return stringBuilder.ToString().Replace(" ", "");
    }

    public static string ToTitleCase(this string source) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source);

    public static string Base64Encode(this string plainText) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

    public static string Base64Decode(this string base64EncodedData) => Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
  }
}
