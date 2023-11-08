#nullable enable
using CNG.Http.Enums;
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


		Task<HttpClientResponse<TResponse>> GetAsync<TResponse>(string url, MediaType mediaType = MediaType.Json, Func<string,MediaType,string>? exceptionHandler = null,
			CancellationToken cancellationToken = default);

		Task<HttpClientResponse> PostAsync<TRequest>(string url, TRequest data, MediaType mediaType = MediaType.Json, Func<string,MediaType,string>? exceptionHandler = null, CancellationToken cancellationToken = default);

		Task<HttpClientResponse> PostAsync(string url,MediaType mediaType=MediaType.Json, Func<string,MediaType,string>? exceptionHandler = null, CancellationToken cancellationToken = default);

		Task<HttpClientResponse<TResponse>> PostAsync<TResponse>(string url, MediaType mediaType = MediaType.Json, Func<string,MediaType,string>? exceptionHandler = null,
			CancellationToken cancellationToken = default);

		Task<HttpClientResponse<TResponse>> PostAsync<TRequest, TResponse>(
			string url,
			TRequest data,
			MediaType mediaType = MediaType.Json,
			Func<string,MediaType,string>? exceptionHandler = null,
			CancellationToken cancellationToken = default);
		Task<HttpClientResponse> HttpPutAsync<TRequest>(
		  string url,
		  TRequest data,
		  MediaType mediaType = MediaType.Json,
		  Func<string,MediaType,string>? exceptionHandler = null,
		  CancellationToken cancellationToken = default);

		Task<HttpClientResponse> PatchAsync<TRequest>(string url, TRequest data, MediaType mediaType = MediaType.Json, Func<string,MediaType,string>? exceptionHandler = null, CancellationToken cancellationToken = default);

		Task<HttpClientResponse> DeleteAsync(string url,MediaType mediaType=MediaType.Json, Func<string,MediaType,string>? exceptionHandler = null, CancellationToken cancellationToken = default);

	
	}
}
