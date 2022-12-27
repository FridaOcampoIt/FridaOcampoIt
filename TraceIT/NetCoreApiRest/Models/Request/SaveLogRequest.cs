namespace WSTraceIT.Models.Request
{
	public class SaveLogRequest
	{
		public string latitude { get; set; }
		public string longitude { get; set; }
		public string logName { get; set; }
		public int mobileId { get; set; }
		public int productId { get; set; }
		public int familyProductId { get; set; }
		public int linkId { get; set; }
	}
}
