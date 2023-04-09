#nullable enable
using System.Net;
using System.Net.Http.Headers;

namespace CNG.Http.Responses
{
  public class HttpClientErrorResponse : HttpClientResponse
  {
    public HttpClientErrorResponse(string message, HttpStatusCode statusCode)
      : base(false, message, statusCode)
    {
    }

    public HttpClientErrorResponse(Exception exception)
      : base(false, exception.Message, HttpStatusCode.BadGateway)
    {
    }

    public HttpClientErrorResponse(
      string message,
      HttpResponseHeaders headers,
      HttpStatusCode statusCode)
      : base(false, message, headers, statusCode)
    {
    }
  }
  public class HttpClientErrorResponse<T> : HttpClientResponse<T>
  {
      public HttpClientErrorResponse(string message, HttpStatusCode statusCode)
          : base(false, message, default(T), statusCode)
      {
      }

      public HttpClientErrorResponse(
          string message,
          HttpResponseHeaders headers,
          HttpStatusCode statusCode)
          : base(false, message, default(T), headers, statusCode)
      {
      }

      public HttpClientErrorResponse(Exception exception)
          : base(false, exception.Message, default(T), HttpStatusCode.BadRequest)
      {
      }

      public HttpClientErrorResponse(Exception exception, HttpResponseHeaders headers)
          : base(false, exception.Message, default(T), headers, HttpStatusCode.BadRequest)
      {
      }
  }
}
