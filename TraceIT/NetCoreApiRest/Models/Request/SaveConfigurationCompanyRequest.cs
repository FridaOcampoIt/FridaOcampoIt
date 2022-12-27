namespace WSTraceIT.Models.Request
{
	public class SaveConfigurationCompanyRequest
	{
		public int company { get; set; }
		public int nUseGuides { get; set; }
		public int nInstalationGuides { get; set; }
		public int nRelatedProduct { get; set; }
		public string notifyComments { get; set; }
		public string notifyWarranty { get; set; }
		public string notifyStolen { get; set; }
		public int nPDF { get; set; }
		public int nCharEspec { get; set; }
		public int nCharFAQ { get; set; }
		public int nImg { get; set; }
		public int nVid { get; set; }
	}
}
