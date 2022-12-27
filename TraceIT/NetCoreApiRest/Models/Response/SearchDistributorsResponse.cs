using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class SearchDistributorsResponse : TraceITResponse
	{
		public List<DistributorsDataSQL> distributors { get; set; }

		public SearchDistributorsResponse()
		{
			this.distributors = new List<DistributorsDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchProductDistributorResponse : TraceITResponse
	{
		public List<ProductDistributorDataSQL> productData { get; set; }
		public SearchProductDistributorResponse()
		{
			this.productData = new List<ProductDistributorDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
