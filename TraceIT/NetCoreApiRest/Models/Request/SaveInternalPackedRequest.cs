using System.Collections.Generic;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Request
{
	public class SaveInternalPackedRequest
	{
		public InternalPackedDataSQL packedData { get; set; }
	}

	public class SaveInternalPackedOperatorRequest
	{
		public InternalPackedOperatorDataSQL operatorData { get; set; }
	}

	public class SaveInternalPackedProdLineRequest
	{
		public InternalPackedProdLineDataSQL prodLineData { get; set; }
	}

	public class SaveProductInternalPackedRequest
	{
		public List<ProductInternalPackedDataSQL> productData { get; set; }
	}
}
