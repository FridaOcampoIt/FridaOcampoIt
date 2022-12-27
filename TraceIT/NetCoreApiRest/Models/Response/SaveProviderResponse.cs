using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class SaveProviderResponse : TraceITResponse
	{
		public SaveProviderResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
