using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class DeleteExternalPackedResponse : TraceITResponse
	{
		public DeleteExternalPackedResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
