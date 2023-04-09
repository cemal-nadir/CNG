
using System.Security.Cryptography;
using System.Text;

namespace CNG.Core.Helpers
{
  public static class HashingHelper
  {
    public static void CreatePasswordHash(
      string? password,
      out byte[]? passwordHash,
      out byte[]? passwordSalt)
    {
      if (string.IsNullOrEmpty(password))
        throw new Exception("Password cannot be blank");
      using var hmacshA512 = new HMACSHA512();
      passwordSalt = hmacshA512.Key;
      passwordHash = hmacshA512.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public static bool VerifyPasswordHash(
      string? password,
      byte[]? passwordHash,
      byte[]? passwordSalt)
    {
      if (string.IsNullOrEmpty(password))
        throw new Exception("Password cannot be blank");
      if (passwordSalt == null) return false;
      using var hmacshA512 = new HMACSHA512(passwordSalt);
      return !hmacshA512.ComputeHash(Encoding.UTF8.GetBytes(password)).Where((t, i) => passwordHash != null && t != passwordHash[i]).Any();

    }
  }
}
