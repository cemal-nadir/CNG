using System.Net;
using System.Net.Http.Headers;
using System.Text;
using CNG.Core.Exceptions;
using CNG.Http.Extensions;
using CNG.Http.Responses;
using Newtonsoft.Json;

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

		public async Task<HttpClientResponse> GetAsync(string url, CancellationToken cancellationToken = default)
		{
			if (_client == null)
				throw new BadRequestException("Please set client before using");
			var response = await _client.GetAsync(this._baseUrl + url, cancellationToken);
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				return new HttpClientErrorResponse("Unauthorized", response.StatusCode);
			var message = await response.Content.ReadAsStringAsync(cancellationToken);
			if (response.IsSuccessStatusCode)
				return new HttpClientSuccessResponse(message, response.Headers);
			try
			{
				var ex = JsonConvert.DeserializeObject<ExceptionResponse?>(message);
				if (ex is null)
					return new HttpClientErrorResponse(message, response.Headers, response.StatusCode);

				if (!string.IsNullOrEmpty(ex.ExceptionMessage))
					ex.Message = ex.ExceptionMessage;
				return new HttpClientErrorResponse(ex.ParseToException(), response.Headers, response.StatusCode);
			}
			catch (Exception ex)
			{
				return new HttpClientErrorResponse(ex.Message, response.Headers, response.StatusCode);
			}
		}

		public async Task<HttpClientResponse<T>> GetAsync<T>(
		  string url,
		  CancellationToken cancellationToken = default)
		{
			if (_client == null)
				throw new BadRequestException("Please set client before using");
			var response = await _client.GetAsync(_baseUrl + url, cancellationToken);
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				return new HttpClientErrorResponse<T>("Unauthorized", response.StatusCode);
			var message = await response.Content.ReadAsStringAsync(cancellationToken);
			if (response.IsSuccessStatusCode)
				return new HttpClientSuccessResponse<T>(JsonConvert.DeserializeObject<T>(message), response.Headers);
			try
			{
				var ex = JsonConvert.DeserializeObject<ExceptionResponse?>(message);
				if (ex is null)
					return new HttpClientErrorResponse<T>(message, response.Headers, response.StatusCode);

				if (!string.IsNullOrEmpty(ex.ExceptionMessage))
					ex.Message = ex.ExceptionMessage;
				return new HttpClientErrorResponse<T>(ex.ParseToException(), response.Headers, response.StatusCode);
			}
			catch {
				return new HttpClientErrorResponse<T>(message, response.Headers, response.StatusCode);
			}
		}

		public async Task<HttpClientResponse> PostAsync<TRequest>(
		  string url,
		  TRequest data,
		  CancellationToken cancellationToken = default)
		{
			if (_client == null)
				throw new BadRequestException("Please set client before using");
			var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
			content.Headers.Add("Host",new Uri(url).Host);
			var response = await _client.PostAsync(_baseUrl + url, content, cancellationToken);
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				return new HttpClientErrorResponse("Unauthorized", response.StatusCode);
			var message = await response.Content.ReadAsStringAsync(cancellationToken);
			if (response.IsSuccessStatusCode)
				return new HttpClientSuccessResponse(message, response.Headers);
			try
			{
				var ex = JsonConvert.DeserializeObject<ExceptionResponse?>(message);
				if(ex is null)
					return new HttpClientErrorResponse(message, response.Headers, response.StatusCode);
				if (!string.IsNullOrEmpty(ex.ExceptionMessage))
					ex.Message = ex.ExceptionMessage;
				return new HttpClientErrorResponse(ex.ParseToException(), response.Headers, response.StatusCode);
			}
			catch {
				return new HttpClientErrorResponse(message, response.Headers, response.StatusCode);

			}
		}
		public async Task<HttpClientResponse<TResponse>> PostAsync<TRequest,TResponse>(
			string url,
			TRequest data,
			CancellationToken cancellationToken = default)
		{
			if (_client == null)
				throw new BadRequestException("Please set client before using");
			var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
			content.Headers.Add("Host", new Uri(url).Host);
			var response = await _client.PostAsync(_baseUrl + url, content, cancellationToken);
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				return new HttpClientErrorResponse<TResponse>("Unauthorized", response.StatusCode);
			var message = await response.Content.ReadAsStringAsync(cancellationToken);
			if (response.IsSuccessStatusCode)
				return new HttpClientSuccessResponse<TResponse>(JsonConvert.DeserializeObject<TResponse>(message), response.Headers);
			try
			{
				var ex = JsonConvert.DeserializeObject<ExceptionResponse?>(message);
				if (ex is null)
					return new HttpClientErrorResponse<TResponse>(message, response.Headers, response.StatusCode);
				if (!string.IsNullOrEmpty(ex.ExceptionMessage))
					ex.Message = ex.ExceptionMessage;
				return new HttpClientErrorResponse<TResponse>(ex.ParseToException(), response.Headers, response.StatusCode);
			}
			catch
			{
				return new HttpClientErrorResponse<TResponse>(message, response.Headers, response.StatusCode);

			}
		}

		public async Task<HttpClientResponse> PostAsync(string url, CancellationToken cancellationToken = default)
		{
			if (_client == null)
				throw new BadRequestException("Please set client before using");
			var response = await _client.PostAsync(_baseUrl + url, null, cancellationToken);
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				return new HttpClientErrorResponse("Unauthorized", response.StatusCode);
			var message = await response.Content.ReadAsStringAsync(cancellationToken);
			if (response.IsSuccessStatusCode)
				return new HttpClientSuccessResponse(message, response.Headers);
			try
			{
				var ex = JsonConvert.DeserializeObject<ExceptionResponse?>(message);
				if (!string.IsNullOrEmpty(ex?.ExceptionMessage))
					ex.Message = ex.ExceptionMessage;
				return new HttpClientErrorResponse(ex.ParseToException(), response.Headers, response.StatusCode);
			}
			catch
			{
				return new HttpClientErrorResponse(message, response.Headers, response.StatusCode);
			}
		}
		public async Task<HttpClientResponse<TResponse>> PostAsync<TResponse>(string url, CancellationToken cancellationToken = default)
		{
			if (_client == null)
				throw new BadRequestException("Please set client before using");
			var response = await _client.PostAsync(_baseUrl + url, null, cancellationToken);
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				return new HttpClientErrorResponse<TResponse>("Unauthorized", response.StatusCode);
			var message = await response.Content.ReadAsStringAsync(cancellationToken);
			if (response.IsSuccessStatusCode)
				return new HttpClientSuccessResponse<TResponse>(JsonConvert.DeserializeObject<TResponse>(message), response.Headers);
			try
			{
				var ex = JsonConvert.DeserializeObject<ExceptionResponse?>(message);
				if (!string.IsNullOrEmpty(ex?.ExceptionMessage))
					ex.Message = ex.ExceptionMessage;
				return new HttpClientErrorResponse<TResponse>(ex.ParseToException(), response.Headers, response.StatusCode);
			}
			catch
			{
				return new HttpClientErrorResponse<TResponse>(message, response.Headers, response.StatusCode);
			}
		}


		public async Task<HttpClientResponse> HttpPutAsync<T>(
		  string url,
		  T data,
		  CancellationToken cancellationToken = default)
		{
			if (_client == null)
				throw new BadRequestException("Please set client before using");
			var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
			content.Headers.Add("Host", new Uri(url).Host);
			var response = await _client.PutAsync(_baseUrl + url, content, cancellationToken);
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				return new HttpClientErrorResponse("Unauthorized", response.StatusCode);
			var message = await response.Content.ReadAsStringAsync(cancellationToken);
			if (response.IsSuccessStatusCode)
				return new HttpClientSuccessResponse(message, response.Headers);
			try
			{
				var ex = JsonConvert.DeserializeObject<ExceptionResponse?>(message);
				if (!string.IsNullOrEmpty(ex?.ExceptionMessage))
					ex.Message = ex.ExceptionMessage;
				return new HttpClientErrorResponse(ex.ParseToException(), response.Headers, response.StatusCode);
			}
			catch {
				return new HttpClientErrorResponse(message, response.Headers, response.StatusCode);
			}
		}

		public async Task<HttpClientResponse> PatchAsync<T>(
		  string url,
		  T data,
		  CancellationToken cancellationToken = default)
		{
			if (_client == null)
				throw new BadRequestException("Please set client before using");
			var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
			content.Headers.Add("Host", new Uri(url).Host);
			var response = await _client.PatchAsync(_baseUrl + url, content, cancellationToken);
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				return new HttpClientErrorResponse("Unauthorized", response.StatusCode);
			var message = await response.Content.ReadAsStringAsync(cancellationToken);
			if (response.IsSuccessStatusCode)
				return new HttpClientSuccessResponse(message, response.Headers);
			try
			{
				var ex = JsonConvert.DeserializeObject<ExceptionResponse?>(message);
				if (!string.IsNullOrEmpty(ex?.ExceptionMessage))
					ex.Message = ex.ExceptionMessage;
				return new HttpClientErrorResponse(ex.ParseToException(), response.Headers, response.StatusCode);
			}
			catch {
				return new HttpClientErrorResponse(message, response.Headers, response.StatusCode);
			}
		}

		public async Task<HttpClientResponse> DeleteAsync(
		  string url,
		  CancellationToken cancellationToken = default)
		{
			if (_client == null)
				throw new BadRequestException("Please set client before using");
			var response = await _client.DeleteAsync(this._baseUrl + url, cancellationToken);
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				return new HttpClientErrorResponse("Unauthorized", response.StatusCode);
			var message = await response.Content.ReadAsStringAsync(cancellationToken);
			if (response.IsSuccessStatusCode)
				return new HttpClientSuccessResponse(message, response.Headers);
			try
			{
				var ex = JsonConvert.DeserializeObject<ExceptionResponse?>(message);
				if (!string.IsNullOrEmpty(ex?.ExceptionMessage))
					ex.Message = ex.ExceptionMessage;
				return new HttpClientErrorResponse(ex.ParseToException(), response.Headers, response.StatusCode);
			}
			catch {
				return new HttpClientErrorResponse(message, response.Headers, response.StatusCode);
			}
		}
	}
}
