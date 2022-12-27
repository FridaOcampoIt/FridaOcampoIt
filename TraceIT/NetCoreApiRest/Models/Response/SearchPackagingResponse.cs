using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class SearchPackagingResponse : TraceITResponse
	{
		public List<PackagingFamilySQL> packagingList { get; set; }

		public SearchPackagingResponse()
		{
			this.packagingList = new List<PackagingFamilySQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
