using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.User;

namespace WSTraceIT.Models.Response
{
	public class SearchUserMobileDataResponse: TraceITResponse
	{
		public UserMobileData dataUserMobile { get; set; }
		public SearchUserMobileDataResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.dataUserMobile = new UserMobileData();
		}
	}
}
