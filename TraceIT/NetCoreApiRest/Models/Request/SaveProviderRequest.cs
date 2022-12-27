using System.Collections.Generic;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Request
{
	public class SaveProviderRequest
	{
		public ProvidersDataSQL providerData { get; set; }
	}

	public class SaveProductProviderRequest
	{
		public List<ProductProviderDataSQL> productData { get; set; }
	}
}
