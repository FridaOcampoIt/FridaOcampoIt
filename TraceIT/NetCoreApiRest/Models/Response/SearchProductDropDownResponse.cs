using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{

	public class familyDropDown{
		public int id { get; set; }
		public string data { get; set; }
		public int extra { get; set; }

		public familyDropDown(){
			this.id = 0;
			this.data = "";
			this.extra = 0;

		}
	}

	public class SearchProductDropDownResponse: TraceITResponse
	{
		public List<familyDropDown> familyDropDown { get; set; }
        public List<TraceITListDropDown> originDropDown { get; set; }
		public List<TraceITListDropDown> companyDropDown { get; set; }

		public SearchProductDropDownResponse()
		{
			this.familyDropDown = new List<familyDropDown>();
            this.originDropDown = new List<TraceITListDropDown>();
			this.companyDropDown = new List<TraceITListDropDown>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
