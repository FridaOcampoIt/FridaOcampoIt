using System.Collections.Generic;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Request
{
	public class SaveExternalPackedRequest
	{
		public ExternalPackedDataSQL packedData { get; set; }
	}

	public class SaveExternalPackedOperatorRequest
	{
		public ExternalPackedOperatorDataSQL operatorData { get; set; }
	}

	public class SaveExternalPackedProdLineRequest
	{
		public ExternalPackedProdLineDataSQL prodLineData { get; set; }
	}

	public class SaveProductExternalPackedRequest
	{
		public List<ProductExternalPackedDataSQL> productData { get; set; }
	}
}
