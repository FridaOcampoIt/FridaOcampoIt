namespace WSTraceIT.Models.Request
{
	public class RestorePasswordRequest
	{
		public string email { get; set; }
		public string recoveryCode { get; set; }
		public string password { get; set; }
	}
}
