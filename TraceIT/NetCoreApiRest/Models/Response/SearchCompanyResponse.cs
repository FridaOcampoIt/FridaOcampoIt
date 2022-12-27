using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Companies;

namespace WSTraceIT.Models.Response
{
	public class SearchCompanyResponse: TraceITResponse
	{
		public List<CompaniesData> companiesDataList { get; set; }

		public SearchCompanyResponse()
		{
			this.companiesDataList = new List<CompaniesData>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}


	public class SearchCompanyNameResponse : TraceITResponse
	{
		public CompaniesData companiaName { get; set; }

		public SearchCompanyNameResponse()
		{
			this.companiaName  = new CompaniesData();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
