using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.User;

namespace WSTraceIT.Models.Response
{
	public class SearchUserDropDownResponse: TraceITResponse
	{
		public UserDropDown dropDown { get; set; }

		public SearchUserDropDownResponse()
		{
			this.dropDown = new UserDropDown();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
