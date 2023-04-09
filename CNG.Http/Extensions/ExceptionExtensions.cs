#nullable enable
using System.ComponentModel.DataAnnotations;
using CNG.Http.Responses;
using Newtonsoft.Json;

namespace CNG.Http.Extensions
{
  public static class ExceptionExtensions
  {
    public static string ParseToException(this ExceptionResponse? ex)
    {
      if (string.IsNullOrEmpty(ex?.Message))
        return "An unknown error has occurred.";
      if (!ex.Message.Contains("errorMessage"))
        return ex.Message ?? "An unknown error has occurred.";
      var source = JsonConvert.DeserializeObject<List<ValidationException>>(ex.Message);
      return source == null ? "An unknown error has occurred." : source.ToList().Aggregate(string.Empty, (Func<string, ValidationException, string>) ((current, e) => current + (string.IsNullOrEmpty(current) ? "" : ", ") + e.Message));
    }
  }
}
