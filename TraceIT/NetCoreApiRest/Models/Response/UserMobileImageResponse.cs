using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class UserMobileImageResponse: TraceITResponse
	{
		public string image  { get; set; }

		public UserMobileImageResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.image = String.Empty;
		}
	}
}
