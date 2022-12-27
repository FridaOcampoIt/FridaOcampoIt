using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class SearchAddressProviderResponse : TraceITResponse
	{
		public List<AddressProviderDataSQL> addressLst { get; set; }

		public SearchAddressProviderResponse()
		{
			this.addressLst = new List<AddressProviderDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchAddressProviderComboResponse : TraceITResponse
	{
		public List<AddressProviderComboSQL> addressComboLst { get; set; }

		public SearchAddressProviderComboResponse()
		{
			this.addressComboLst = new List<AddressProviderComboSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchProvidersDirectionResponse : TraceITResponse
	{
		public List<ProvidersSelect> providers { get; set; }

		public SearchProvidersDirectionResponse()
		{
			this.providers = new List<ProvidersSelect>();
        }
    }
}
