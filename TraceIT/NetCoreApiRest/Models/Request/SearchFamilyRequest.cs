namespace WSTraceIT.Models.Request
{
	public class SearchFamilyRequest
	{
		public int idUser { get; set; }
		public string barCode { get; set; }
		public float latitude { get; set; }
		public float longitude { get; set; }
	}
}
