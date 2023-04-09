
using System.Security.Cryptography;
using System.Text;

namespace CNG.Core.Helpers
{
  public static class RandomHelper
  {
    public static string Numeric(int charSize)
    {
      var numArray = new byte[byte.MaxValue];
      using var randomNumberGenerator = RandomNumberGenerator.Create();
      randomNumberGenerator.GetBytes(numArray);
      var base64String = Convert.ToBase64String(numArray);
      var source = new char[10]
      {
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
      foreach (var ch in base64String.Where((Func<char, bool>) (x => source.Contains(x))))
      {
          stringBuilder.Append(ch);
          if (charSize <= stringBuilder.Length)
              break;
      }
      return stringBuilder.ToString();
    }

    public static string Character(int charSize)
    {
      var numArray = new byte[byte.MaxValue];
      using var randomNumberGenerator = RandomNumberGenerator.Create();
      randomNumberGenerator.GetBytes(numArray);
      var base64String = Convert.ToBase64String(numArray);
      var source = new char[50]
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
          'x'
      };
      var stringBuilder = new StringBuilder();
      foreach (var ch in base64String.Where((Func<char, bool>) (x => source.Contains(x))))
      {
          stringBuilder.Append(ch);
          if (charSize <= stringBuilder.Length)
              break;
      }
      return stringBuilder.ToString();
    }

    public static string Mixed(int charSize)
    {
      var numArray = new byte[byte.MaxValue];
      using var randomNumberGenerator = RandomNumberGenerator.Create();
      randomNumberGenerator.GetBytes(numArray);
      var base64String = Convert.ToBase64String(numArray);
      var source = new char[60]
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
      foreach (var ch in base64String.Where((Func<char, bool>) (x => source.Contains(x))))
      {
          stringBuilder.Append(ch);
          if (charSize <= stringBuilder.Length)
              break;
      }
      return stringBuilder.ToString();
    }
  }
}
