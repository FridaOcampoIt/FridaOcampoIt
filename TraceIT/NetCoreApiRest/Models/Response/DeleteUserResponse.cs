using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class DeleteUserResponse: TraceITResponse
	{
		public DeleteUserResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
