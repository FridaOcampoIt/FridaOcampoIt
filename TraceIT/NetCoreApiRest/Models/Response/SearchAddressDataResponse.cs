using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Address;

namespace WSTraceIT.Models.Response
{
	public class SearchAddressDataResponse: TraceITResponse
	{
		public AddressDataEdition addressData { get; set; }

		public SearchAddressDataResponse()
		{
			this.addressData = new AddressDataEdition();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
