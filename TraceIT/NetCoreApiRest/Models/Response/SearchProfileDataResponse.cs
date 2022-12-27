using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Profiles;

namespace WSTraceIT.Models.Response
{
	public class SearchProfileDataResponse: TraceITResponse
	{
		public PermissionProfileData permissionProfileData { get; set; }

		public SearchProfileDataResponse()
		{
			this.permissionProfileData = new PermissionProfileData();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
