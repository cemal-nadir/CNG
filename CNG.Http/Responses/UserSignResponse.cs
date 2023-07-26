using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNG.Http.Responses
{
	public class UserSignResponse
	{
		public TokenResponse? Token { get; set; }

		public UserInfoResponse? UserInfo { get; set; }
	}
}
