namespace WSTraceIT.Models.Request
{
	public class EditProductRequest
	{
		public int idProduct { get; set; }
		public string udid { get; set; }
		public string expirationDate { get; set; }
		public int familyProductId { get; set; }
		public int directionId { get; set; }
		public int status { get; set; }
	}
}
