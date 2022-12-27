using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class SearchDropDownListFamilyResponse: TraceITResponse
	{
		public List<TraceITListDropDown> companyData { get; set; }
		public List<TraceITListDropDown> categoryData { get; set; }
		public List<TraceITListDropDown> addressData { get; set; }
		public List<TraceITListDropDown> recommendedBy { get; set; }

		public SearchDropDownListFamilyResponse()
		{
			this.addressData = new List<TraceITListDropDown>();
			this.categoryData = new List<TraceITListDropDown>();
			this.companyData = new List<TraceITListDropDown>();
			this.recommendedBy = new List<TraceITListDropDown>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
