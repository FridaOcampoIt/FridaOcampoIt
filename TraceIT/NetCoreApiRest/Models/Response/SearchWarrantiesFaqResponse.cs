using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Response
{
	public class SearchWarrantiesFaqResponse: TraceITResponse
	{
		public WarrantiesFaqFamily warrantiesFaq { get; set; }
		public SearchWarrantiesFaqResponse()
		{
			this.warrantiesFaq = new WarrantiesFaqFamily();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
