using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class SaveImportProductResponse: TraceITResponse
	{
		public string ciusFile { get; set; }
		public string urlFile { get; set; }
		public SaveImportProductResponse()
		{
			this.ciusFile = String.Empty;
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.urlFile = String.Empty;
		}
	}
}
