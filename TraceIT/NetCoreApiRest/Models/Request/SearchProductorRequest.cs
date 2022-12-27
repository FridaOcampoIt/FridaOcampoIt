namespace WSTraceIT.Models.Request
{
	public class SearchProductorRequest
	{
		public int companiaId { get; set; }
		public string nombreProductorNumeroNombreAcopio { get; set; }
	}

	public class SearchProductorByIdRequest 
    {
		public int productorId { get; set; }
    }
}
