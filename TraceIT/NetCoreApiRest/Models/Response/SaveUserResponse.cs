using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class SaveUserResponse: TraceITResponse
	{
		public SaveUserResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
