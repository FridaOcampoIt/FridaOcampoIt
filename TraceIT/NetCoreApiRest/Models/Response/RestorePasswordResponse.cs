using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class RestorePasswordResponse: TraceITResponse
	{
		public RestorePasswordResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
