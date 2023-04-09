#nullable enable
using CNG.Http.Responses;

namespace CNG.Http.Services
{
  public interface IHttpClientService
  {
    void SetBaseUrl(string baseUrl);

    void SetBearerAuthentication(string accessToken);

    void SetBasicAuthentication(string apiKey, string secretKey);

    void SetHeader(Dictionary<string, string> headers);

    Task<HttpClientResponse> GetAsync(string url, CancellationToken cancellationToken = default (CancellationToken));

    Task<HttpClientResponse<T>> GetAsync<T>(string url,
        CancellationToken cancellationToken = default(CancellationToken));

    Task<HttpClientResponse> PostAsync<T>(string url, T data, CancellationToken cancellationToken = default (CancellationToken));

    Task<HttpClientResponse> PostAsync(string url, CancellationToken cancellationToken = default (CancellationToken));

    Task<HttpClientResponse> HttpPutAsync<T>(
      string url,
      T data,
      CancellationToken cancellationToken = default (CancellationToken));

    Task<HttpClientResponse> PatchAsync<T>(string url, T data, CancellationToken cancellationToken = default (CancellationToken));

    Task<HttpClientResponse> DeleteAsync(string url, CancellationToken cancellationToken = default (CancellationToken));
  }
}
