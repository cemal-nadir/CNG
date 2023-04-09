#nullable enable
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using CNG.Http.Extensions;
using CNG.Http.Responses;
using Newtonsoft.Json;

namespace CNG.Http.Services
{
  public class HttpClientService : IHttpClientService
  {
    private readonly HttpClient _httpClient;
    private string _baseUrl = "";

    public HttpClientService(HttpClient httpClient)
    {
      this._httpClient = httpClient;
      this._httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      this._httpClient.Timeout = TimeSpan.FromMinutes(10.0);
    }

    public void SetBaseUrl(string baseUrl) => this._baseUrl = baseUrl;

    public void SetBearerAuthentication(string accessToken) => this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

    public void SetBasicAuthentication(string apiKey, string secretKey) => this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":" + secretKey)));

    public void SetHeader(Dictionary<string, string> headers)
    {
      foreach (KeyValuePair<string, string> header1 in headers)
      {
        KeyValuePair<string, string> header = header1;
        if (this._httpClient.DefaultRequestHeaders.Any(x => x.Key == header.Key))
          this._httpClient.DefaultRequestHeaders.Remove(header.Key);
        this._httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
      }
    }

    public async Task<HttpClientResponse> GetAsync(string url, CancellationToken cancellationToken = default (CancellationToken))
    {
      var response = await this._httpClient.GetAsync(this._baseUrl + url, cancellationToken);
      if (response.StatusCode == HttpStatusCode.Unauthorized)
        return new HttpClientErrorResponse("Unauthorized", response.StatusCode);
      var contentItem = await response.Content.ReadAsStringAsync(cancellationToken);
      if (response.IsSuccessStatusCode)
        return new HttpClientSuccessResponse(contentItem, response.Headers);
      try
      {
        var ex = JsonConvert.DeserializeObject<ExceptionResponse>(contentItem);
        if (!string.IsNullOrEmpty(ex?.ExceptionMessage))
          ex.Message = ex.ExceptionMessage;
        return new HttpClientErrorResponse(ex.ParseToException(), response.Headers, response.StatusCode);
      }
      catch (Exception)
      {
        return new HttpClientErrorResponse(contentItem, response.Headers, response.StatusCode);
      }
    }

    public async Task<HttpClientResponse<T>> GetAsync<T>(string url,
        CancellationToken cancellationToken = default(CancellationToken))
    {
      var response = await this._httpClient.GetAsync(this._baseUrl + url, cancellationToken);
      if (response.StatusCode == HttpStatusCode.Unauthorized)
        return new HttpClientErrorResponse<T>("Unauthorized", response.StatusCode);
      var contentItem = await response.Content.ReadAsStringAsync(cancellationToken);
      if (response.IsSuccessStatusCode)
      {
        var data = JsonConvert.DeserializeObject<T>(contentItem);
        return new HttpClientSuccessResponse<T>(data, response.Headers);
      }
      try
      {
        var ex = JsonConvert.DeserializeObject<ExceptionResponse>(contentItem);
        if (!string.IsNullOrEmpty(ex?.ExceptionMessage))
          ex.Message = ex.ExceptionMessage;
        return new HttpClientErrorResponse<T>(ex.ParseToException(), response.Headers, response.StatusCode);
      }
      catch (Exception)
      {
        return new HttpClientErrorResponse<T>(contentItem, response.Headers, response.StatusCode);
      }
    }

    public async Task<HttpClientResponse> PostAsync<T>(
      string url,
      T data,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      var content = JsonConvert.SerializeObject(data);
      var body = new StringContent(content, Encoding.UTF8, "application/json");
      var response = await this._httpClient.PostAsync(this._baseUrl + url, body, cancellationToken);
      if (response.StatusCode == HttpStatusCode.Unauthorized)
        return new HttpClientErrorResponse("Unauthorized", response.StatusCode);
      var contentItem = await response.Content.ReadAsStringAsync(cancellationToken);
      if (response.IsSuccessStatusCode)
        return new HttpClientSuccessResponse(contentItem, response.Headers);
      try
      {
        var ex = JsonConvert.DeserializeObject<ExceptionResponse>(contentItem);
        if (!string.IsNullOrEmpty(ex?.ExceptionMessage))
          ex.Message = ex.ExceptionMessage;
        return new HttpClientErrorResponse(ex.ParseToException(), response.Headers, response.StatusCode);
      }
      catch (Exception)
      {
        return new HttpClientErrorResponse(contentItem, response.Headers, response.StatusCode);
      }
    }

    public async Task<HttpClientResponse> PostAsync(string url, CancellationToken cancellationToken = default (CancellationToken))
    {
      var response = await this._httpClient.PostAsync(this._baseUrl + url, null, cancellationToken);
      if (response.StatusCode == HttpStatusCode.Unauthorized)
        return new HttpClientErrorResponse("Unauthorized", response.StatusCode);
      var contentItem = await response.Content.ReadAsStringAsync(cancellationToken);
      if (response.IsSuccessStatusCode)
        return new HttpClientSuccessResponse(contentItem, response.Headers);
      try
      {
        var ex = JsonConvert.DeserializeObject<ExceptionResponse>(contentItem);
        if (!string.IsNullOrEmpty(ex?.ExceptionMessage))
          ex.Message = ex.ExceptionMessage;
        return new HttpClientErrorResponse(ex.ParseToException(), response.Headers, response.StatusCode);
      }
      catch (Exception)
      {
        return new HttpClientErrorResponse(contentItem, response.Headers, response.StatusCode);
      }
    }

    public async Task<HttpClientResponse> HttpPutAsync<T>(
      string url,
      T data,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      var content = JsonConvert.SerializeObject(data);
      var body = new StringContent(content, Encoding.UTF8, "application/json");
      var response = await this._httpClient.PutAsync(this._baseUrl + url, body, cancellationToken);
      if (response.StatusCode == HttpStatusCode.Unauthorized)
        return new HttpClientErrorResponse("Unauthorized", response.StatusCode);
      var contentItem = await response.Content.ReadAsStringAsync(cancellationToken);
      if (response.IsSuccessStatusCode)
        return new HttpClientSuccessResponse(contentItem, response.Headers);
      try
      {
        var ex = JsonConvert.DeserializeObject<ExceptionResponse>(contentItem);
        if (!string.IsNullOrEmpty(ex?.ExceptionMessage))
          ex.Message = ex.ExceptionMessage;
        return new HttpClientErrorResponse(ex.ParseToException(), response.Headers, response.StatusCode);
      }
      catch (Exception)
      {
        return new HttpClientErrorResponse(contentItem, response.Headers, response.StatusCode);
      }
    }

    public async Task<HttpClientResponse> PatchAsync<T>(
      string url,
      T data,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      var content = JsonConvert.SerializeObject(data);
      var body = new StringContent(content, Encoding.UTF8, "application/json");
      var response = await this._httpClient.PatchAsync(this._baseUrl + url, body, cancellationToken);
      if (response.StatusCode == HttpStatusCode.Unauthorized)
        return new HttpClientErrorResponse("Unauthorized", response.StatusCode);
      var contentItem = await response.Content.ReadAsStringAsync(cancellationToken);
      if (response.IsSuccessStatusCode)
        return new HttpClientSuccessResponse(contentItem, response.Headers);
      try
      {
        var ex = JsonConvert.DeserializeObject<ExceptionResponse>(contentItem);
        if (!string.IsNullOrEmpty(ex?.ExceptionMessage))
          ex.Message = ex.ExceptionMessage;
        return new HttpClientErrorResponse(ex.ParseToException(), response.Headers, response.StatusCode);
      }
      catch (Exception)
      {
        return new HttpClientErrorResponse(contentItem, response.Headers, response.StatusCode);
      }
    }

    public async Task<HttpClientResponse> DeleteAsync(
      string url,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      var response = await this._httpClient.DeleteAsync(this._baseUrl + url, cancellationToken);
      if (response.StatusCode == HttpStatusCode.Unauthorized)
        return new HttpClientErrorResponse("Unauthorized", response.StatusCode);
      var contentItem = await response.Content.ReadAsStringAsync(cancellationToken);
      if (response.IsSuccessStatusCode)
        return new HttpClientSuccessResponse(contentItem, response.Headers);
      try
      {
        var ex = JsonConvert.DeserializeObject<ExceptionResponse>(contentItem);
        if (!string.IsNullOrEmpty(ex?.ExceptionMessage))
          ex.Message = ex.ExceptionMessage;
        return new HttpClientErrorResponse(ex.ParseToException(), response.Headers, response.StatusCode);
      }
      catch (Exception)
      {
        return new HttpClientErrorResponse(contentItem, response.Headers, response.StatusCode);
      }
    }
  }
}
