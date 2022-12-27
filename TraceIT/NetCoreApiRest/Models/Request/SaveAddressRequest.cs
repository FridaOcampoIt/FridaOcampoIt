namespace WSTraceIT.Models.Request
{
	public class SaveAddressRequest
	{
		public string name { get; set; }
		public string phone { get; set; }
		public string address { get; set; }
		public string postalCode { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string country { get; set; }
		public string latitude { get; set; }
		public string longitude { get; set; }
		public bool status { get; set; }
		public int idCompany { get; set; }
		public int idTypeAddress { get; set; }
		public int isType { get; set; }
	}
}
