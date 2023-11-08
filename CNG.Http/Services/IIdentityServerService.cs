using CNG.Http.Responses;
using IdentityModel.Client;

namespace CNG.Http.Services
{
	public interface IIdentityServerService
	{
		void SetClient(
			string identityUrl,
			string clientId,
			string clientSecret,
			string httpClientName);

		Task<HttpClientResponse<ClientCredentialResponse>> GetClientCredentialTokenAsync(
			bool requireHttps = false,
			CancellationToken cancellationToken = default(CancellationToken));

		Task<HttpClientResponse<TokenResponse>> GetAccessTokenByRefreshToken(
			string refreshToken,
			bool requireHttps = false,
			CancellationToken cancellationToken = default(CancellationToken));

		Task<HttpClientResponse> RevokeRefreshToken(
			string refreshToken,
			bool requireHttps = false,
			CancellationToken cancellationToken = default(CancellationToken));

		Task<HttpClientResponse<UserSignResponse>> SignIn(
			string userName,
			string password,
			bool requireHttps,
			CancellationToken cancellationToken = default(CancellationToken));
	}
}
