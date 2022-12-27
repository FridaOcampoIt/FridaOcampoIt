namespace WSTraceIT.Models.Request
{
	public class SaveQualificationLinkRequest
	{
		public int userMobileId { get; set; }
		public int linkId { get; set; }
		public decimal qualification { get; set; }
	}
}
