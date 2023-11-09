using System.Text;
using CNG.Core.Exceptions;
using CNG.Http.Enums;
using CNG.Http.Helpers;
using CNG.Http.Responses;

namespace CNG.Http.Extensions
{
	public static class RequestHelper
	{

		public static async Task<HttpClientResponse<TResponse>> GenerateRequest<TResponse>(this HttpClient? client,
			HttpMethod method, string url, ResponseType responseType = ResponseType.Json,
			ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default)
		{

			if (client is null)
				throw new BadRequestException("Please set client before using");

			var requestMessage = new HttpRequestMessage(method, url);

			var response = await client.SendAsync(requestMessage, cancellationToken);

			return await response.PrepareResponse<TResponse>(responseType, exceptionResponseType, exceptionHandler,
				cancellationToken);

		}

		public static async Task<HttpClientResponse> GenerateRequest(this HttpClient? client, HttpMethod method,
			string url, ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default)
		{
			if (client is null)
				throw new BadRequestException("Please set client before using");

			var requestMessage = new HttpRequestMessage(method, url);

			var response = await client.SendAsync(requestMessage, cancellationToken);

			return await response.PrepareResponse(exceptionResponseType, exceptionHandler, cancellationToken);

		}

		public static async Task<HttpClientResponse> GenerateRequest<TRequest>(this HttpClient? client,
			HttpMethod method, TRequest data, string url, RequestType requestType = RequestType.Json,
			ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default)
		{
			if (client is null)
				throw new BadRequestException("Please set client before using");

			var requestMessage = new HttpRequestMessage(method, url);
			requestMessage.SetContent(data, requestType, method);


			var response = await client.SendAsync(requestMessage, cancellationToken);

			return await response.PrepareResponse(exceptionResponseType, exceptionHandler, cancellationToken);

		}

		public static async Task<HttpClientResponse<TResponse>> GenerateRequest<TRequest, TResponse>(
			this HttpClient? client, HttpMethod method, TRequest data, string url,
			RequestType requestType = RequestType.Json, ResponseType responseType = ResponseType.Json,
			ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default)
		{
			if (client is null)
				throw new BadRequestException("Please set client before using");

			var requestMessage = new HttpRequestMessage(method, url);
			requestMessage.SetContent(data, requestType, method);

			var response = await client.SendAsync(requestMessage, cancellationToken);

			return await response.PrepareResponse<TResponse>(responseType, exceptionResponseType, exceptionHandler,
				cancellationToken);

		}

		private static void SetContent<TData>(this HttpRequestMessage requestMessage, TData data,
			RequestType requestType, HttpMethod method)
		{
			if (method != HttpMethod.Post && method != HttpMethod.Put && method != HttpMethod.Patch)
				throw new BadRequestException("Request method not supported");
			switch (requestType)
			{
				default:
				case RequestType.Json:
				{
					requestMessage.Content = new StringContent(
						JsonHelper.SerializeObject(data), Encoding.UTF8, "application/json");

					break;
				}
				case RequestType.Xml:
				{
					requestMessage.Content = new StringContent(
						XmlHelper.SerializeObject(data) ?? "", Encoding.UTF8, "application/xml");
					break;
				}
				case RequestType.FormUrlEncoded:
				{
					if (method != HttpMethod.Post)
						throw new BadRequestException("FormUrlEncoded requests can only be made via post method");

					requestMessage.Content = new FormUrlEncodedContent(
						data as IEnumerable<KeyValuePair<string, string>> ?? new Dictionary<string, string>());
					break;
				}


			}
		}
	}
}
