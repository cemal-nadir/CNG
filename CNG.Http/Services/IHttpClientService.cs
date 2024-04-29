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


		Task<HttpClientResponse<TResponse>> GetAsync<TResponse>(string url,
			ResponseType responseType = ResponseType.Json, ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default);

        Task<HttpClientResponse> GetAsync(string url,
            ExceptionResponseType? exceptionResponseType = null,
            Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
            CancellationToken cancellationToken = default);


        Task<HttpClientResponse> PostAsync<TRequest>(string url, TRequest data,
			RequestType requestType = RequestType.Json,
			ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default);

		Task<HttpClientResponse> PostAsync(string url, ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default);

		Task<HttpClientResponse<TResponse>> PostAsync<TResponse>(string url,
			ResponseType responseType = ResponseType.Json, ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default);

		Task<HttpClientResponse<TResponse>> PostAsync<TRequest, TResponse>(
			string url,
			TRequest data,
			RequestType requestType = RequestType.Json,
			ResponseType responseType = ResponseType.Json,
			ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default);

		Task<HttpClientResponse> HttpPutAsync<TRequest>(
			string url,
			TRequest data,
			RequestType requestType = RequestType.Json,
			ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default);

		Task<HttpClientResponse> PatchAsync<TRequest>(string url, TRequest data,
			RequestType requestType = RequestType.Json, ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default);

		Task<HttpClientResponse> DeleteAsync(string url, ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default);


	}
}
