namespace WSTraceIT.Models.Request
{
	public class SearchCompanyRequest
	{
		public string name { get; set; }
		public string businessName { get; set; }

		public int packedId { get; set; }
	}


	public class SearchCompanyNameRequest
	{
		public int companiaId { get; set; }
	}
}
