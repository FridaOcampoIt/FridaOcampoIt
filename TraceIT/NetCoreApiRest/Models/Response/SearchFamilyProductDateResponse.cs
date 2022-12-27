using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Response
{
	public class SearchFamilyProductDateResponse: TraceITResponse
	{
		public FamilyDataEdition productFamily { get; set; }

		public SearchFamilyProductDateResponse()
		{
			this.productFamily = new FamilyDataEdition();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
