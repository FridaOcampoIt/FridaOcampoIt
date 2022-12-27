namespace WSTraceIT.Models.Request
{
	public class SearchAcopioRequest
	{
		public int companiaId { get; set; }
		public string nombreNumeroAcopio { get; set; }
	}

	public class SearchAcopioByIdRequest 
    {
		public int acopioId { get; set; }
    }
}
