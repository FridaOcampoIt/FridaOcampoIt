using System.Collections.Generic;

namespace WSTraceIT.Models.Request
{
	public class EditProfileRequest
	{
		public int profileId { get; set; }
		public string name { get; set; }
		public int company { get; set; }
		public List<int> permission { get; set; }
	}
}
