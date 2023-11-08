using CNG.Http.Responses;
using System.Net;
using CNG.Http.Enums;
using CNG.Http.Helpers;

namespace CNG.Http.Extensions
{
	public static class ResponseExtensions
	{
		public static async Task<HttpClientResponse<TResponse>> PrepareResponse<TResponse>(this HttpResponseMessage response, MediaType mediaType = MediaType.Json, Func<string, MediaType, string>? exceptionHandler = null, CancellationToken cancellationToken = default)
		{
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				return new HttpClientErrorResponse<TResponse>("Unauthorized", response.StatusCode);

			var message = await response.Content.ReadAsStringAsync(cancellationToken);

			if (response.IsSuccessStatusCode)
				return new HttpClientSuccessResponse<TResponse>(
					mediaType is MediaType.Json
						? JsonHelper.DeserializeObject<TResponse>(message)
						: XmlHelper.DeserializeObject<TResponse>(message), response.Headers);

			return new HttpClientErrorResponse<TResponse>(exceptionHandler != null ? exceptionHandler(message,mediaType) : message, response.Headers,
				   response.StatusCode);
		}
		public static async Task<HttpClientResponse> PrepareResponse(this HttpResponseMessage response,MediaType mediaType=MediaType.Json, Func<string,MediaType,string>? exceptionHandler = null, CancellationToken cancellationToken = default)
		{
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				return new HttpClientErrorResponse("Unauthorized", response.StatusCode);

			var message = await response.Content.ReadAsStringAsync(cancellationToken);

			if (response.IsSuccessStatusCode)
				return new HttpClientSuccessResponse(
					message, response.Headers);

			return new HttpClientErrorResponse(exceptionHandler != null ? exceptionHandler(message,mediaType) : message, response.Headers,
				response.StatusCode);
		}
	}
}
