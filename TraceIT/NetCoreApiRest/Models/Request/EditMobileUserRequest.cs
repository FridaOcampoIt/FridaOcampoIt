namespace WSTraceIT.Models.Request
{
	public class EditMobileUserRequest
	{
		public int userId { get; set; }
		public string name { get; set; }
		public string lastName { get; set; }
		public string email { get; set; }
		public int age { get; set; }
		public string gender { get; set; }
		public string country { get; set; }
		public string city { get; set; }
		public string postalCode { get; set; }
	}
}
