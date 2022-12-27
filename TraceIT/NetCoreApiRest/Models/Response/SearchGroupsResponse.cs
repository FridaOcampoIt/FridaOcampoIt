using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class SearchGroupResponse: TraceITResponse
	{
		public List<GroupDataSQL> Groups { get; set; }

		public SearchGroupResponse()
		{
			this.Groups = new List<GroupDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
