using System.Net.Http.Headers;
using System.Text;
using CNG.Core.Exceptions;
using CNG.Http.Enums;
using CNG.Http.Extensions;
using CNG.Http.Responses;

namespace CNG.Http.Services
{
    public class HttpClientService : IHttpClientService
	{
		private HttpClient? _client;
		private readonly IHttpClientFactory _httpClientFactory;
		private string _baseUrl = "";

		public HttpClientService(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

		public void SetClient(string httpClientName)
		{
			_client = _httpClientFactory.CreateClient(httpClientName);
			_client.Timeout = TimeSpan.FromMinutes(10.0);
		}

		public void SetBaseUrl(string baseUrl) => _baseUrl = baseUrl;

		public void SetBearerAuthentication(string accessToken)
		{
			if (_client == null)
				throw new BadRequestException("Please set client before using");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
		}

		public void SetBasicAuthentication(string apiKey, string secretKey)
		{
			if (_client == null)
				throw new BadRequestException("Please set client before using");
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":" + secretKey)));
		}

		public void SetHeader(Dictionary<string, string> headers)
		{
			if (_client == null)
				throw new BadRequestException("Please set client before using");
			foreach (KeyValuePair<string, string> header1 in headers)
			{
				var header = header1;
				if (_client.DefaultRequestHeaders.Any(x => x.Key == header.Key))
					_client.DefaultRequestHeaders.Remove(header.Key);
				_client.DefaultRequestHeaders.Add(header.Key, header.Value);
			}
		}

		public async Task<HttpClientResponse<TResponse>> GetAsync<TResponse>(string url, ResponseType responseType = ResponseType.Json,
			ExceptionResponseType? exceptionResponseType = null, Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default)
		{
			return await _client.GenerateRequest<TResponse>(method: HttpMethod.Get, url: $"{_baseUrl}{url}",
				responseType: responseType, exceptionResponseType: exceptionResponseType,
				exceptionHandler: exceptionHandler,
				cancellationToken: cancellationToken);
		}
        public async Task<HttpClientResponse> GetAsync(string url,
            ExceptionResponseType? exceptionResponseType = null, Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
            CancellationToken cancellationToken = default)
        {
            return await _client.GenerateRequest(method: HttpMethod.Get, url: $"{_baseUrl}{url}",
                exceptionResponseType: exceptionResponseType,
                exceptionHandler: exceptionHandler,
                cancellationToken: cancellationToken);
        }
        public async Task<HttpClientResponse> PostAsync<TRequest>(string url, TRequest data, RequestType requestType = RequestType.Json,
			ExceptionResponseType? exceptionResponseType = null, Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default)
		{
			return await _client.GenerateRequest(method: HttpMethod.Post, data: data, url: $"{_baseUrl}{url}",
				requestType: requestType, exceptionResponseType: exceptionResponseType,
				exceptionHandler: exceptionHandler,
				cancellationToken: cancellationToken);
		}

		public async Task<HttpClientResponse> PostAsync(string url, ExceptionResponseType? exceptionResponseType = null, Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default)
		{
			return await _client.GenerateRequest(method: HttpMethod.Post, url: $"{_baseUrl}{url}",
				exceptionResponseType: exceptionResponseType,
				exceptionHandler: exceptionHandler, cancellationToken: cancellationToken);

		}

		public async Task<HttpClientResponse<TResponse>> PostAsync<TResponse>(string url, ResponseType responseType = ResponseType.Json,
			ExceptionResponseType? exceptionResponseType = null, Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default)
		{
			return await _client.GenerateRequest<TResponse>(method: HttpMethod.Post, url: $"{_baseUrl}{url}",
				responseType: responseType, exceptionResponseType: exceptionResponseType,
				exceptionHandler: exceptionHandler, cancellationToken: cancellationToken);

		}

		public async Task<HttpClientResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest data, RequestType requestType = RequestType.Json,
			ResponseType responseType = ResponseType.Json, ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null, CancellationToken cancellationToken = default)
		{
			return await _client.GenerateRequest<TRequest, TResponse>(method: HttpMethod.Post, url: $"{_baseUrl}{url}",
				data: data, requestType: requestType, responseType: responseType,
				exceptionResponseType: exceptionResponseType, exceptionHandler: exceptionHandler,
				cancellationToken: cancellationToken);
		}

		public async Task<HttpClientResponse> HttpPutAsync<TRequest>(string url, TRequest data, RequestType requestType = RequestType.Json,
			ExceptionResponseType? exceptionResponseType = null, Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default)
		{
			return await _client.GenerateRequest(method: HttpMethod.Put, url: $"{_baseUrl}{url}", data: data,
				requestType: requestType, exceptionResponseType: exceptionResponseType,
				exceptionHandler: exceptionHandler, cancellationToken: cancellationToken);
		}

		public async Task<HttpClientResponse> PatchAsync<TRequest>(string url, TRequest data, RequestType requestType = RequestType.Json,
			ExceptionResponseType? exceptionResponseType = null, Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default)
		{
			return await _client.GenerateRequest(method: HttpMethod.Patch, url: $"{_baseUrl}{url}", data: data,
				requestType: requestType, exceptionResponseType: exceptionResponseType,
				exceptionHandler: exceptionHandler, cancellationToken: cancellationToken);
		}

		public async Task<HttpClientResponse> DeleteAsync(string url, ExceptionResponseType? exceptionResponseType = null, Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default)
		{
			return await _client.GenerateRequest(method: HttpMethod.Delete, url: $"{_baseUrl}{url}",
				exceptionResponseType: exceptionResponseType, exceptionHandler: exceptionHandler,
				cancellationToken: cancellationToken);
		}

	}
}

