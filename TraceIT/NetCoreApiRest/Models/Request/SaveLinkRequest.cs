using System.Collections.Generic;
using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Request
{
	public class SaveLinkRequest
	{
		public List<linkData> linkData { get; set; }
		public int sectionType { get; set; }
		public int idFamily { get; set; }
		public int recommendedById { get; set; }
		public int userCompanyId { get; set; }
	}
}
