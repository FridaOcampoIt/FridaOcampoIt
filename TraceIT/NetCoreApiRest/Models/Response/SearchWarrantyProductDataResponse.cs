using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.User;

namespace WSTraceIT.Models.Response
{
	public class SearchWarrantyProductDataResponse: TraceITResponse
	{
		public WarrantiesDataUser warrantiesData { get; set; }

		public SearchWarrantyProductDataResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.warrantiesData = new WarrantiesDataUser();
		}
	}
}
