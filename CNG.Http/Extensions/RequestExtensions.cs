using System.Text;
using CNG.Core.Exceptions;
using CNG.Http.Enums;
using CNG.Http.Helpers;
using CNG.Http.Responses;

namespace CNG.Http.Extensions
{
    public static class RequestHelper
    {
        public static async Task<HttpClientResponse<TResponse>> GenerateRequest<TResponse>(this HttpClient? client, HttpMethod method, string url, MediaType mediaType = MediaType.Json, Func<string,MediaType,string>? exceptionHandler = null, CancellationToken cancellationToken = default)
        {

            if (client is null)
                throw new BadRequestException("Please set client before using");

            var requestMessage = new HttpRequestMessage(method, url);

            var response = await client.SendAsync(requestMessage, cancellationToken);

            return await response.PrepareResponse<TResponse>(mediaType, exceptionHandler, cancellationToken);

        }
        public static async Task<HttpClientResponse> GenerateRequest(this HttpClient? client, HttpMethod method, string url,MediaType mediaType=MediaType.Json, Func<string,MediaType,string>? exceptionHandler = null, CancellationToken cancellationToken = default)
        {
            if (client is null)
                throw new BadRequestException("Please set client before using");


            var requestMessage = new HttpRequestMessage(method, url);

            var response = await client.SendAsync(requestMessage, cancellationToken);

            return await response.PrepareResponse(mediaType,exceptionHandler, cancellationToken);

        }
        public static async Task<HttpClientResponse> GenerateRequest<TRequest>(this HttpClient? client, HttpMethod method, TRequest data, string url, MediaType mediaType = MediaType.Json, Func<string,MediaType,string>? exceptionHandler = null, CancellationToken cancellationToken = default)
        {
            if (client is null)
                throw new BadRequestException("Please set client before using");

            var content = new StringContent(
	            mediaType is MediaType.Json
		            ? JsonHelper.SerializeObject(data) ?? ""
		            : XmlHelper.SerializeObject(data) ?? "", Encoding.UTF8,
	            mediaType is MediaType.Json ? "application/json" : "application/xml");

			var requestMessage = new HttpRequestMessage(method, url)
			{
				Content = content
			};

			var response = await client.SendAsync(requestMessage, cancellationToken);

            return await response.PrepareResponse(mediaType, exceptionHandler, cancellationToken);

        }
        public static async Task<HttpClientResponse<TResponse>> GenerateRequest<TRequest, TResponse>(this HttpClient? client, HttpMethod method, TRequest data, string url, MediaType mediaType = MediaType.Json, Func<string,MediaType,string>? exceptionHandler = null, CancellationToken cancellationToken = default)
        {
            if (client is null)
                throw new BadRequestException("Please set client before using");

            var content = new StringContent(
                mediaType is MediaType.Json
                    ? JsonHelper.SerializeObject(data)??""
                    : XmlHelper.SerializeObject(data) ?? "", Encoding.UTF8,
                mediaType == MediaType.Json ? "application/json" : "application/xml");

			var requestMessage = new HttpRequestMessage(method, url)
			{
				Content = content
			};

			var response = await client.SendAsync(requestMessage, cancellationToken);

            return await response.PrepareResponse<TResponse>(mediaType, exceptionHandler, cancellationToken);

        }
    }
}
