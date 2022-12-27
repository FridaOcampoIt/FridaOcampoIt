using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Profiles;

namespace WSTraceIT.Models.Response
{
	public class SearchDropDownPermissionResponse: TraceITResponse
	{
		public DropDownProfilesPermission dropDownProfilesPermission { get; set; }

		public SearchDropDownPermissionResponse()
		{
			this.dropDownProfilesPermission = new DropDownProfilesPermission();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
