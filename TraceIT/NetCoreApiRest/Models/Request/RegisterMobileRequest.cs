namespace WSTraceIT.Models.Request
{
	public class RegisterMobileRequest
	{
		public int mobileId { get; set; }
		public string model { get; set; }
		public string tokenFCM { get; set; }
		public int userId { get; set; }
		public string imei { get; set; }
	}
}
