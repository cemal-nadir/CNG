#nullable enable
namespace CNG.Http.Responses
{
  public class ExceptionResponse
  {
    public int StatusCode { get; set; }

    public string? ExceptionType { get; set; }

    public string? Message { get; set; }

    public string? ExceptionMessage { get; set; }

    public string? StackTrace { get; set; }
  }
}
