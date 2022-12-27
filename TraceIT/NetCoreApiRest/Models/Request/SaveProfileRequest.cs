using System.Collections.Generic;

namespace WSTraceIT.Models.Request
{
	public class SaveProfileRequest
	{
		public string name { get; set; }
		public int company { get; set; }
		public List<int> permission { get; set; }
	}
}
