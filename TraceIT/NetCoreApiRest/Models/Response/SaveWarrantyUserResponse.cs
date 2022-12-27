using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class SaveWarrantyUserResponse: TraceITResponse
	{
		public int productId { get; set; }
		public string qrCode { get; set; }

		public SaveWarrantyUserResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.productId = 0;
			this.qrCode = String.Empty;
		}
	}
}
