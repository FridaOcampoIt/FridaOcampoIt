using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class EditProfileResponse: TraceITResponse
	{
		public EditProfileResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
