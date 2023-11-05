using System.Net.Http.Headers;
using System.Text;
using CNG.Core.Exceptions;
using CNG.Http.Helpers;
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

		public void SetBaseUrl(string baseUrl) => this._baseUrl = baseUrl;

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

		public async Task<HttpClientResponse> GetAsync(string url, CancellationToken cancellationToken = default) =>
			await _client.GenerateRequest(HttpMethod.Get, this._baseUrl + url, cancellationToken);

		public async Task<HttpClientResponse<TResponse>> GetAsync<TResponse>(
			string url,
			CancellationToken cancellationToken = default) =>
			await _client.GenerateRequest<TResponse>(HttpMethod.Get, _baseUrl + url, cancellationToken);

		public async Task<HttpClientResponse> PostAsync<TRequest>(
			string url,
			TRequest data,
			CancellationToken cancellationToken = default) =>
			await _client.GenerateRequest(HttpMethod.Post, data, _baseUrl + url, cancellationToken);

		public async Task<HttpClientResponse<TResponse>> PostAsync<TRequest,TResponse>(
			string url,
			TRequest data,
			CancellationToken cancellationToken = default) =>
			await _client.GenerateRequest<TRequest, TResponse>(HttpMethod.Post, data, _baseUrl + url,cancellationToken);

		public async Task<HttpClientResponse> PostAsync(string url, CancellationToken cancellationToken = default) =>
			await _client.GenerateRequest(HttpMethod.Post, _baseUrl + url, cancellationToken);

		public async Task<HttpClientResponse<TResponse>> PostAsync<TResponse>(string url,
			CancellationToken cancellationToken = default) =>
			await _client.GenerateRequest<TResponse>(HttpMethod.Post, _baseUrl + url, cancellationToken);


		public async Task<HttpClientResponse> HttpPutAsync<T>(
			string url,
			T data,
			CancellationToken cancellationToken = default) =>
			await _client.GenerateRequest(HttpMethod.Put, data, _baseUrl + url, cancellationToken);

		public async Task<HttpClientResponse> PatchAsync<T>(
			string url,
			T data,
			CancellationToken cancellationToken = default) =>
			await _client.GenerateRequest(HttpMethod.Patch, data, _baseUrl + url, cancellationToken);

		public async Task<HttpClientResponse> DeleteAsync(
			string url,
			CancellationToken cancellationToken = default) =>
			await _client.GenerateRequest(HttpMethod.Delete, _baseUrl + url, cancellationToken);
	}
}
