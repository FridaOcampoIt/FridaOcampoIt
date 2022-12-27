using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class SearchProvidersResponse : TraceITResponse
	{
		public List<ProvidersDataSQL> providers { get; set; }

		public SearchProvidersResponse()
		{
			this.providers = new List<ProvidersDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchProductProviderResponse : TraceITResponse
	{
		public List<ProductProviderDataSQL> productData { get; set; }
		public SearchProductProviderResponse()
		{
			this.productData = new List<ProductProviderDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
