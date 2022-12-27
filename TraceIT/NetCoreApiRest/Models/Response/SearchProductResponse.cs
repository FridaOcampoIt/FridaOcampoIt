using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Product;

namespace WSTraceIT.Models.Response
{
	public class SearchProductResponse: TraceITResponse
	{
		public List<ProductList> products { get; set; }

		public SearchProductResponse()
		{
			this.products = new List<ProductList>();
			this.messageEsp = String.Empty;
			this.messageEng = String.Empty;
		}
	}

	public class SearchDataProductResponse : TraceITResponse
	{
		public List<ProductDetailsCIU> product { get; set; }

		public SearchDataProductResponse()
		{
			this.product = new List<ProductDetailsCIU>();
			this.messageEsp = String.Empty;
			this.messageEng = String.Empty;
		}
	}
}
