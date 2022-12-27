using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Request
{
	public class UpdateLinkRequest
	{
		public linkData linkData { get; set; }
		public int recommendedById { get; set; }
	}
}
