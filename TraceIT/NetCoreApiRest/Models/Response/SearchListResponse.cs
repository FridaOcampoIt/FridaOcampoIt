using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.User;

namespace WSTraceIT.Models.Response
{
	public class SearchListResponse: TraceITResponse
	{
		public ListSystem listSystem { get; set; }

		public SearchListResponse()
		{
			this.listSystem = new ListSystem();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
