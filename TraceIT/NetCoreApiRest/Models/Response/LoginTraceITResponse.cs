using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class LoginTraceITResponse: TraceITResponse
	{
		public string token { get; set; }

		public LoginTraceITResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.token = String.Empty;
		}
	}
}
