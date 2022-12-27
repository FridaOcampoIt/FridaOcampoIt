using System.Collections.Generic;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Request
{
	public class SaveDistributorRequest
	{
		public DistributorsDataSQL distributorData { get; set; }
	}

	public class SaveProductDistributorRequest
	{
		public List<ProductDistributorDataSQL> productData { get; set; }
	}
}
