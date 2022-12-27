using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class SaveAddressResponse: TraceITResponse
	{
		public SaveAddressResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
