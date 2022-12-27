using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.User;

namespace WSTraceIT.Models.Response
{
	public class SearchUserDataResponse: TraceITResponse
	{
		public DataUserBackOfficeDatas dataUser { get; set; }

		public SearchUserDataResponse()
		{
			this.dataUser = new DataUserBackOfficeDatas();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
