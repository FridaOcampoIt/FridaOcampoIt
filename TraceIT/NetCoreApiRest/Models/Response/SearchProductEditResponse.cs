using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Product;

namespace WSTraceIT.Models.Response
{
	public class SearchProductEditResponse: TraceITResponse
	{
		public ProductDetails productDetails { get; set; }

		public SearchProductEditResponse()
		{
			this.productDetails = new ProductDetails();
			this.messageEsp = String.Empty;
			this.messageEng = String.Empty;
		}
	}
}
