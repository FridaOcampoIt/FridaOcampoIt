using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class SaveExternalPackedResponse : TraceITResponse
	{
		public int groupingId { get; set; }
		public SaveExternalPackedResponse()
		{
			this.groupingId = 0;
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class PrintOperationExtResponse : TraceITResponse
	{
		public string QRCode { get; set; }
		public string groupingName { get; set; }
		public int range { get; set; }
		public string productName { get; set; }
		public string company { get; set; }
		public string packagin { get; set; }
		public string ranges { get; set; }
		public string instructions { get; set; }
		public string etiquetaId { get; set; }
		public int operationScanned { get; set; }
		public string date { get; set; }
		public string line { get; set; }

		public PrintOperationExtResponse()
		{
			this.QRCode = String.Empty;
			this.groupingName = String.Empty;
			this.range = 0;
			this.productName = String.Empty;
			this.company = String.Empty;
			this.packagin = String.Empty;
			this.ranges = String.Empty;
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.etiquetaId = String.Empty;
			this.instructions = String.Empty;
			this.operationScanned = 0;
			this.date = String.Empty;
			this.line = String.Empty;
		}
	}
}
