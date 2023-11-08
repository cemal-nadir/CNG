using IdentityModel.Client;

namespace CNG.Http.Responses
{
	public class UserSignResponse
	{
		public TokenResponse? Token { get; set; }

		public UserInfoResponse? UserInfo { get; set; }
	}
}
