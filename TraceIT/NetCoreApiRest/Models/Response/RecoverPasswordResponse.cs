using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class RecoverPasswordResponse: TraceITResponse
	{
		public RecoverPasswordResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
