using CNG.Http.Responses;
using System.Net;
using CNG.Http.Enums;
using CNG.Http.Helpers;

namespace CNG.Http.Extensions
{
	public static class ResponseExtensions
	{
		public static async Task<HttpClientResponse<TResponse>> PrepareResponse<TResponse>(
			this HttpResponseMessage response, ResponseType responseType = ResponseType.Json,
			ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default)
		{
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				return new HttpClientErrorResponse<TResponse>("Unauthorized", response.StatusCode);

			var message = await response.Content.ReadAsStringAsync(cancellationToken);

			if (response.IsSuccessStatusCode)
				return new HttpClientSuccessResponse<TResponse>(
					responseType is ResponseType.Json
						? JsonHelper.DeserializeObject<TResponse>(message)
						: XmlHelper.DeserializeObject<TResponse>(message), response.Headers);

			return new HttpClientErrorResponse<TResponse>(
				exceptionHandler != null ? exceptionHandler(message, exceptionResponseType) : message, response.Headers,
				response.StatusCode);
		}

		public static async Task<HttpClientResponse> PrepareResponse(this HttpResponseMessage response,
			ExceptionResponseType? exceptionResponseType = null,
			Func<string, ExceptionResponseType?, string>? exceptionHandler = null,
			CancellationToken cancellationToken = default)
		{
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				return new HttpClientErrorResponse("Unauthorized", response.StatusCode);

			var message = await response.Content.ReadAsStringAsync(cancellationToken);

			if (response.IsSuccessStatusCode)
				return new HttpClientSuccessResponse(
					message, response.Headers);

			return new HttpClientErrorResponse(
				exceptionHandler != null ? exceptionHandler(message, exceptionResponseType) : message, response.Headers,
				response.StatusCode);
		}
	}
}
