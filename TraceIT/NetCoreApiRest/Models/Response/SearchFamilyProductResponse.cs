using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Response
{
	public class SearchFamilyProductResponse: TraceITResponse
	{
		public List<ProductFamily> productFamilyData { get; set; }
		public SearchFamilyProductResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.productFamilyData = new List<ProductFamily>();
		}
	}
}
