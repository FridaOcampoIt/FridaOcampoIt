using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Configuration;

namespace WSTraceIT.Models.Response
{
	public class SearchConfigurationCompanyResponse: TraceITResponse
	{
		public CompanyConfigurationData companyConfiguration { get; set; }

		public SearchConfigurationCompanyResponse()
		{
			this.companyConfiguration = new CompanyConfigurationData();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
