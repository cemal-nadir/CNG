using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CNG.Core.Exceptions;
using CNG.Http.Extensions;
using CNG.Http.Responses;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CNG.Http.Helpers
{
	public static class RequestHelper
	{
		public static async Task<HttpClientResponse<TResponse>> GenerateRequest<TResponse>(this HttpClient? client, HttpMethod method, string url, CancellationToken cancellationToken = default)
		{
			if (client is null)
				throw new BadRequestException("Please set client before using");

			var requestMessage = new HttpRequestMessage(method, url);
			requestMessage.Headers.Host = new Uri(url).Host;

			var response = await client.SendAsync(requestMessage, cancellationToken);

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
		public static async Task<HttpClientResponse> GenerateRequest(this HttpClient? client, HttpMethod method, string url, CancellationToken cancellationToken = default)
		{
			if (client is null)
				throw new BadRequestException("Please set client before using");
			

			var requestMessage = new HttpRequestMessage(method, url);
		
			requestMessage.Headers.Host = new Uri(url).Host;

			var response = await client.SendAsync(requestMessage, cancellationToken);

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
			catch
			{
				return new HttpClientErrorResponse(message, response.Headers, response.StatusCode);

			}
		}
		public static async Task<HttpClientResponse> GenerateRequest<TRequest>(this HttpClient? client, HttpMethod method,TRequest data,string url,CancellationToken cancellationToken=default)
		{
			if(client is null)
				throw new BadRequestException("Please set client before using");
			var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

			var requestMessage = new HttpRequestMessage(method, url);
			requestMessage.Content = content;
			requestMessage.Headers.Host = new Uri(url).Host;

			var response = await client.SendAsync(requestMessage, cancellationToken);

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
			catch
			{
				return new HttpClientErrorResponse(message, response.Headers, response.StatusCode);

			}
		}
		public static async Task<HttpClientResponse<TResponse>> GenerateRequest<TRequest,TResponse>(this HttpClient? client, HttpMethod method, TRequest data, string url, CancellationToken cancellationToken = default)
		{
			if (client is null)
				throw new BadRequestException("Please set client before using");
			var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

			var requestMessage = new HttpRequestMessage(method, url);
			requestMessage.Content = content;
			requestMessage.Headers.Host = new Uri(url).Host;

			var response = await client.SendAsync(requestMessage, cancellationToken);

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

	}
}
