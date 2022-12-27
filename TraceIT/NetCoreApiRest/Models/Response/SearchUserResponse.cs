using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.User;

namespace WSTraceIT.Models.Response
{
	public class SearchUserResponse: TraceITResponse
	{
		public List<DataUserBackOffice> dataUser { get; set; }

		public SearchUserResponse()
		{
			this.dataUser = new List<DataUserBackOffice>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}


	public class SearchUserByCompanyIdResponse : TraceITResponse
	{
		public List<UserByCompanyId> dataUserByCompanyId { get; set; }

		public SearchUserByCompanyIdResponse()
		{
			this.dataUserByCompanyId = new List<UserByCompanyId>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
