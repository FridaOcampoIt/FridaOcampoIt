using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class SearchGroupingResponse: TraceITResponse
	{
		public List<GroupingDataSQL> groupingList { get; set; }

		public SearchGroupingResponse()
		{
			this.groupingList = new List<GroupingDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
