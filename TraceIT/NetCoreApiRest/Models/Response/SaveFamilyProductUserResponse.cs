using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.User;

namespace WSTraceIT.Models.Response
{
	public class SaveFamilyProductUserResponse: TraceITResponse
	{
		public List<ProductFamilyUsers> productFamilyUsers { get; set; }

		public SaveFamilyProductUserResponse()
		{
			this.productFamilyUsers = new List<ProductFamilyUsers>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
