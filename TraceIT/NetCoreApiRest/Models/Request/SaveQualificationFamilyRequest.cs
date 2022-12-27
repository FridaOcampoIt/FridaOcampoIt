namespace WSTraceIT.Models.Request
{
	public class SaveQualificationFamilyRequest
	{
		public int userMobileId { get; set; }
		public int familyProductId { get; set; }
		public decimal qualification { get; set; }
	}
}
