using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Profiles;

namespace WSTraceIT.Models.Response
{
	public class SearchProfileResponse: TraceITResponse
	{
		public List<Profiles> profiles { get; set; }

		public SearchProfileResponse()
		{
			this.profiles = new List<Profiles>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
