using CNG.Core.Exceptions;
using CNG.Http.Responses;
using IdentityModel.Client;
using Newtonsoft.Json;
using TokenResponse = IdentityModel.Client.TokenResponse;

namespace CNG.Http.Services
{
	public class IdentityServerService : IIdentityServerService
	{
		private HttpClient? _client;
		private readonly IHttpClientFactory _httpClientFactory;
		private string? _identityUrl;
		private string? _clientId;
		private string? _clientSecret;

		public IdentityServerService(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

		public void SetClient(
		  string identityUrl,
		  string clientId,
		  string clientSecret,
		  string httpClientName)
		{
			_clientId = clientId;
			_identityUrl = identityUrl;
			_clientSecret = clientSecret;
			_client = _httpClientFactory.CreateClient(httpClientName);
			_client.Timeout = TimeSpan.FromMinutes(10.0);
		}

		public async Task<HttpClientResponse<ClientCredentialResponse>> GetClientCredentialTokenAsync(
		  bool requireHttps = false,
		  CancellationToken cancellationToken = default)
		{
			if (_clientId == null || _clientSecret == null || _identityUrl == null || _client == null)
				throw new BadRequestException("Please set client before using");
			var client = _client;
			var request1 = new DiscoveryDocumentRequest();
			request1.Address = _identityUrl;
			request1.Policy = new DiscoveryPolicy()
			{
				RequireHttps = requireHttps
			};
			var cancellationToken1 = cancellationToken;
			var discoveryDocumentAsync = await client.GetDiscoveryDocumentAsync(request1, cancellationToken1);
			if (discoveryDocumentAsync.IsError)
				return new HttpClientErrorResponse<ClientCredentialResponse>(discoveryDocumentAsync.Error ?? "", discoveryDocumentAsync.HttpStatusCode);
			var request2 = new ClientCredentialsTokenRequest();
			request2.ClientId = _clientId;
			request2.ClientSecret = _clientSecret;
			request2.Address = discoveryDocumentAsync.TokenEndpoint;
			var tokenResponse = await _client.RequestClientCredentialsTokenAsync(request2, cancellationToken);
			if (tokenResponse.IsError)
			{
				return new HttpClientErrorResponse<ClientCredentialResponse>(string.IsNullOrEmpty(tokenResponse.Error) ? JsonConvert.DeserializeObject<ExceptionResponse>(tokenResponse.Raw ?? string.Empty)?.Message??"":tokenResponse.Error, tokenResponse.HttpStatusCode);
			}
			return new HttpClientSuccessResponse<ClientCredentialResponse>(new ClientCredentialResponse()
			{
				AccessToken = tokenResponse.AccessToken,
				EndTime = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn)
			});
		}

		public async Task<HttpClientResponse<TokenResponse>> GetAccessTokenByRefreshToken(
		  string refreshToken,
		  bool requireHttps = false,
		  CancellationToken cancellationToken = default)
		{
			if (_clientId == null || _clientSecret == null || _identityUrl == null || _client == null)
				throw new BadRequestException("Please set client before using");
			var client = _client;
			var request1 = new DiscoveryDocumentRequest();
			request1.Address = _identityUrl;
			request1.Policy = new DiscoveryPolicy()
			{
				RequireHttps = requireHttps
			};
			var cancellationToken1 = cancellationToken;
			var discoveryDocumentAsync = await client.GetDiscoveryDocumentAsync(request1, cancellationToken1);
			if (discoveryDocumentAsync.IsError)
				return new HttpClientErrorResponse<TokenResponse>(discoveryDocumentAsync.Error ?? "", discoveryDocumentAsync.HttpStatusCode);
			var request2 = new RefreshTokenRequest();
			request2.ClientId = _clientId;
			request2.ClientSecret = _clientSecret;
			request2.RefreshToken = refreshToken;
			request2.Address = discoveryDocumentAsync.TokenEndpoint;
			var data = await _client.RequestRefreshTokenAsync(request2, cancellationToken);
			return !data.IsError ? new HttpClientSuccessResponse<TokenResponse>(data) :new HttpClientErrorResponse<TokenResponse>(string.IsNullOrEmpty(data.Error) ? JsonConvert.DeserializeObject<ExceptionResponse>(data.Raw ?? string.Empty)?.Message ?? "" : data.Error, data.HttpStatusCode);
		}
		public async Task<HttpClientResponse> RevokeRefreshToken(
		string refreshToken,
		bool requireHttps = false,
		  CancellationToken cancellationToken = default)
		{
			if (_clientId == null || _clientSecret == null || _identityUrl == null || _client == null)
				throw new BadRequestException("Please set client before using");
			var client = _client;
			var request1 = new DiscoveryDocumentRequest();
			request1.Address = _identityUrl;
			request1.Policy = new DiscoveryPolicy()
			{
				RequireHttps = requireHttps
			};
			var cancellationToken1 = cancellationToken;
			var discoveryDocumentAsync = await client.GetDiscoveryDocumentAsync(request1, cancellationToken1);
			if (discoveryDocumentAsync.IsError)
				return new HttpClientErrorResponse(discoveryDocumentAsync.Error ?? "", discoveryDocumentAsync.HttpStatusCode);
			var request2 = new TokenRevocationRequest();
			request2.ClientId = _clientId;
			request2.ClientSecret = _clientSecret;
			request2.Address = discoveryDocumentAsync.RevocationEndpoint;
			request2.Token = refreshToken;
			request2.TokenTypeHint = "refresh_token";
			var revocationResponse = await _client.RevokeTokenAsync(request2, cancellationToken);
			return !revocationResponse.IsError ? new HttpClientSuccessResponse() : new HttpClientErrorResponse(string.IsNullOrEmpty(revocationResponse.Error) ? JsonConvert.DeserializeObject<ExceptionResponse>(revocationResponse.Raw ?? string.Empty)?.Message ?? "" : revocationResponse.Error, revocationResponse.HttpStatusCode);
		}
		public async Task<HttpClientResponse<UserSignResponse>> SignIn(
		string userName,
		string password,
		bool requireHttps,
		  CancellationToken cancellationToken = default)
		{
			if (_clientId == null || _clientSecret == null || _identityUrl == null || _client == null)
				throw new BadRequestException("Please set client before using");
			var client = _client;
			var request1 = new DiscoveryDocumentRequest();
			request1.Address = _identityUrl;
			request1.Policy = new DiscoveryPolicy()
			{
				RequireHttps = requireHttps
			};
			var cancellationToken1 = cancellationToken;
			var disco = await client.GetDiscoveryDocumentAsync(request1, cancellationToken1);
			if (disco.IsError)
				return new HttpClientErrorResponse<UserSignResponse>(disco.Error ?? "", disco.HttpStatusCode);
			var request2 = new PasswordTokenRequest();
			request2.ClientId = _clientId;
			request2.ClientSecret = _clientSecret;
			request2.UserName = userName;
			request2.Password = password;
			request2.Address = disco.TokenEndpoint;
			var token = await _client.RequestPasswordTokenAsync(request2, cancellationToken);
			if (token.IsError)
				return new HttpClientErrorResponse<UserSignResponse>(string.IsNullOrEmpty(token.Error) ? JsonConvert.DeserializeObject<ExceptionResponse>(token.Raw ?? string.Empty)?.Message ?? "" : token.Error, token.HttpStatusCode);
			var request3 = new UserInfoRequest();
			request3.Token = token.AccessToken;
			request3.Address = disco.UserInfoEndpoint;
			var userInfoAsync = await _client.GetUserInfoAsync(request3, cancellationToken);
			if (userInfoAsync.IsError)
				return new HttpClientErrorResponse<UserSignResponse>(string.IsNullOrEmpty(userInfoAsync.Error) ? JsonConvert.DeserializeObject<ExceptionResponse>(userInfoAsync.Raw ?? string.Empty)?.Message ?? "" : userInfoAsync.Error, userInfoAsync.HttpStatusCode);
			return new HttpClientSuccessResponse<UserSignResponse>(new UserSignResponse()
			{
				Token = token,
				UserInfo = userInfoAsync
			});
		}
	}
}
