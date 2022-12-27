using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class SearchDropDownListAddressResponse: TraceITResponse
	{
		public List<TraceITListDropDown> companyData { get; set; }
		public List<TraceITListDropDown> addressTypeData { get; set; }
		public List<TraceITListDropDown> familyData { get; set; }

		public SearchDropDownListAddressResponse()
		{
			this.addressTypeData = new List<TraceITListDropDown>();
			this.companyData = new List<TraceITListDropDown>();
			this.familyData = new List<TraceITListDropDown>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
