#nullable enable
using CNG.Http.Responses;

namespace CNG.Http.Services
{
	public interface IHttpClientService
	{
		void SetClient(string clientName);
		void SetBaseUrl(string baseUrl);

		void SetBearerAuthentication(string accessToken);

		void SetBasicAuthentication(string apiKey, string secretKey);

		void SetHeader(Dictionary<string, string> headers);

		Task<HttpClientResponse> GetAsync(string url, CancellationToken cancellationToken = default(CancellationToken));

		Task<HttpClientResponse<T>> GetAsync<T>(string url,
			CancellationToken cancellationToken = default(CancellationToken));

		Task<HttpClientResponse> PostAsync<T>(string url, T data, CancellationToken cancellationToken = default(CancellationToken));

		Task<HttpClientResponse> PostAsync(string url, CancellationToken cancellationToken = default(CancellationToken));

		Task<HttpClientResponse<TResponse>> PostAsync<TResponse>(string url,
			CancellationToken cancellationToken = default);

		Task<HttpClientResponse<TResponse>> PostAsync<TRequest, TResponse>(
			string url,
			TRequest data,
			CancellationToken cancellationToken = default);
		Task<HttpClientResponse> HttpPutAsync<T>(
		  string url,
		  T data,
		  CancellationToken cancellationToken = default(CancellationToken));

		Task<HttpClientResponse> PatchAsync<T>(string url, T data, CancellationToken cancellationToken = default(CancellationToken));

		Task<HttpClientResponse> DeleteAsync(string url, CancellationToken cancellationToken = default(CancellationToken));
	}
}
