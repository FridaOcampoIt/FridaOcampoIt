using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Address;

namespace WSTraceIT.Models.Response
{
	public class SearchAddressResponse: TraceITResponse
	{
		public List<AddressesData> addressDataList { get; set; }

		public SearchAddressResponse()
		{
			this.addressDataList = new List<AddressesData>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
