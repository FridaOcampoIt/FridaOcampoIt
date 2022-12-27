namespace WSTraceIT.Models.Request
{
	public class SaveProductRequest
	{
		public string udid { get; set; }
		public string expirationDate { get; set; }
		public int familyProductId { get; set; }
		public int directionId { get; set; }
		public int packagingId { get; set; }
	}
}
