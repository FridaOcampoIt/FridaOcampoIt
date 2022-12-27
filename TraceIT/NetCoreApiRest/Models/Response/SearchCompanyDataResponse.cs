using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Companies;

namespace WSTraceIT.Models.Response
{
	public class SearchCompanyDataResponse: TraceITResponse
	{
		public CompanyDataEdition companyData { get; set; }
		public ContactCompaniesData contactData { get; set; }

		public SearchCompanyDataResponse()
		{
			this.companyData = new CompanyDataEdition();
			this.contactData = new ContactCompaniesData();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
