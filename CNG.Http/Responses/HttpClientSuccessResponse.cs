#nullable enable
using System.Net;
using System.Net.Http.Headers;

namespace CNG.Http.Responses
{
  public class HttpClientSuccessResponse : HttpClientResponse
  {
    public HttpClientSuccessResponse()
      : base(true, "", HttpStatusCode.OK)
    {
    }

    public HttpClientSuccessResponse(string message)
      : base(true, message, HttpStatusCode.OK)
    {
    }

    public HttpClientSuccessResponse(string message, HttpResponseHeaders headers)
      : base(true, message, headers, HttpStatusCode.OK)
    {
    }

    public HttpClientSuccessResponse(HttpResponseHeaders headers)
      : base(true, "", headers, HttpStatusCode.OK)
    {
    }
  }
  public class HttpClientSuccessResponse<T> : HttpClientResponse<T>
  {
      public HttpClientSuccessResponse(T? data)
          : base(true, "", data, HttpStatusCode.OK)
      {
      }

      public HttpClientSuccessResponse(T? data, string message)
          : base(true, message, data, HttpStatusCode.OK)
      {
      }

      public HttpClientSuccessResponse(T? data, HttpResponseHeaders headers)
          : base(true, "", data, headers, HttpStatusCode.OK)
      {
      }

      public HttpClientSuccessResponse(T? data, string message, HttpResponseHeaders headers)
          : base(true, message, data, headers, HttpStatusCode.OK)
      {
      }
  }
}
