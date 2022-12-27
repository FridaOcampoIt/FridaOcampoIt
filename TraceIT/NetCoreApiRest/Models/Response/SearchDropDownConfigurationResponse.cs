using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Configuration;

namespace WSTraceIT.Models.Response
{
	public class SearchDropDownConfigurationResponse: TraceITResponse
	{
		public GeneralConfigurationDropDown configuration { get; set; }

		public SearchDropDownConfigurationResponse()
		{
			this.configuration = new GeneralConfigurationDropDown();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
