namespace WSTraceIT.Models.Request
{
    public class SearchProductRequest
    {
        public string udid { get; set; }
        public int idFamily { get; set; }
    }

    public class SearchDataProductCIURequest
    {
        public string ciu { get; set; }
    }
}
