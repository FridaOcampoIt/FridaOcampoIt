using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class DeleteProviderResponse : TraceITResponse
	{
		public DeleteProviderResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
