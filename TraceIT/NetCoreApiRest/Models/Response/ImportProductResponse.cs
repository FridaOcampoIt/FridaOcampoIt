using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class SimpleProductResponse: TraceITResponse
	{
		public SimpleProductResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
