using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Response
{
	public class SearchLinkFamilyResponse: TraceITResponse
	{
		public LinksData linkFamilies { get; set; }
		public SearchLinkFamilyResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.linkFamilies = new LinksData();
		}
	}

}
