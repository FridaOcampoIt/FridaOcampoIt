using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class UpdateLinkResponse: TraceITResponse
	{
		public UpdateLinkResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
