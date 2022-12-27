using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class RegisterMobileResponse: TraceITResponse
	{
		public int mobileId { get; set; }

		public RegisterMobileResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.mobileId = 0;
		}
	}
}
