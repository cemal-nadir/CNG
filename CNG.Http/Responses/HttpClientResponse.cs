#nullable enable
using System.Net;
using System.Net.Http.Headers;

namespace CNG.Http.Responses
{
  public abstract class HttpClientResponse
  {
    protected HttpClientResponse(bool success, string message, HttpStatusCode statusCode)
    {
      this.Success = success;
      this.Message = message;
      this.StatusCode = statusCode;
    }

    protected HttpClientResponse(
      bool success,
      string message,
      HttpResponseHeaders headers,
      HttpStatusCode statusCode)
    {
      this.Success = success;
      this.Message = message;
      this.Headers = headers;
      this.StatusCode = statusCode;
    }

    public bool Success { get; }

    public string Message { get; }

    public HttpResponseHeaders? Headers { get; }

    public HttpStatusCode StatusCode { get; }
  }
  public abstract class HttpClientResponse<T>
  {
      protected HttpClientResponse(bool success, string message, T? data, HttpStatusCode statusCode)
      {
          this.Success = success;
          this.Message = message;
          this.Data = data;
          this.StatusCode = statusCode;
      }

      protected HttpClientResponse(
          bool success,
          string message,
          T? data,
          HttpResponseHeaders headers,
          HttpStatusCode statusCode)
      {
          this.Success = success;
          this.Message = message;
          this.Data = data;
          this.Headers = headers;
          this.StatusCode = statusCode;
      }

      public bool Success { get; }

      public T? Data { get; }

      public string Message { get; }

      public HttpResponseHeaders? Headers { get; }

      public HttpStatusCode StatusCode { get; }
  }
}
