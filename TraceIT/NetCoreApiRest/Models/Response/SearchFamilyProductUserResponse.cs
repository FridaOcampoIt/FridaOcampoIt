using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.User;

namespace WSTraceIT.Models.Response
{
	public class SearchFamilyProductUserResponse: TraceITResponse
	{
		public List<ProductFamilyUsers> productFamilyUsers { get; set; }

		public SearchFamilyProductUserResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.productFamilyUsers = new List<ProductFamilyUsers>();
		}
	}
}
