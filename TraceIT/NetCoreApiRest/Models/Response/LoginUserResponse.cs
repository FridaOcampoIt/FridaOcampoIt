using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.User;

namespace WSTraceIT.Models.Response
{
	public class LoginUserResponse: TraceITResponse
	{
		public string token { get; set; }
		public UserLoginData userData { get; set; }

		public LoginUserResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.token = String.Empty;
			this.userData = new UserLoginData();
		}
	}
}
