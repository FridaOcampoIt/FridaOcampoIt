namespace WSTraceIT.Models.Request
{
	public class SaveImportProductRequest
	{
		public bool byFile { get; set; }
		public string fileBase { get; set; }
		public int familyProductId { get; set; }
		public int directionId { get; set; }
		public int amount { get; set; }
		public string expiry { get; set; }
		public string udid { get; set; }
		public int userId { get; set; }
		public string company { get; set; }
		public string family { get; set; }
		public int packagingId { get; set; }
		public int columns { get; set; }
	}
}
