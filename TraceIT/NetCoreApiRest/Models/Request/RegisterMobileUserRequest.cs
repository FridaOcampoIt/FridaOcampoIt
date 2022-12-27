namespace WSTraceIT.Models.Request
{
	public class RegisterMobileUserRequest
	{
		public string name { get; set; }
		public string lastName { get; set; }
		public string email { get; set; }
		public string gender { get; set; }
		public int age { get; set; }
		public string country { get; set; }
		public string city { get; set; }
		public string postalCode { get; set; }
		public string password { get; set; }
		public string facebookId { get; set; }
		public string googleId { get; set; }
	}
}
