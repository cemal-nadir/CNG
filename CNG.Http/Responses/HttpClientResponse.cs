#nullable enable
using System.Net;
using System.Net.Http.Headers;

namespace CNG.Http.Responses
{
  public abstract class HttpClientResponse
  {
    protected HttpClientResponse(bool success, string message, HttpStatusCode statusCode)
    {
      Success = success;
      Message = message;
      StatusCode = statusCode;
    }

    protected HttpClientResponse(
      bool success,
      string message,
      HttpResponseHeaders headers,
      HttpStatusCode statusCode)
    {
      Success = success;
      Message = message;
      Headers = headers;
      StatusCode = statusCode;
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
          Success = success;
          Message = message;
          Data = data;
          StatusCode = statusCode;
      }

      protected HttpClientResponse(
          bool success,
          string message,
          T? data,
          HttpResponseHeaders headers,
          HttpStatusCode statusCode)
      {
          Success = success;
          Message = message;
          Data = data;
          Headers = headers;
          StatusCode = statusCode;
      }

      public bool Success { get; }

      public T? Data { get; }

      public string Message { get; }

      public HttpResponseHeaders? Headers { get; }

      public HttpStatusCode StatusCode { get; }
  }
}
