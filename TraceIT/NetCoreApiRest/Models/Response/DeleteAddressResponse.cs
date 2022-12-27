using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class DeleteAddressResponse: TraceITResponse
	{
		public DeleteAddressResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
