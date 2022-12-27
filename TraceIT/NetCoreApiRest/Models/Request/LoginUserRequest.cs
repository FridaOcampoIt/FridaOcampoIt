namespace WSTraceIT.Models.Request
{
	public class LoginUserRequest
	{
		public string user { get; set; }
		public string password { get; set; }
		public string id { get; set; }
		public string lat { get; set; }
		public string lon { get; set; }
		public int isOrigin { get; set; }

        #region Constructor
        public LoginUserRequest ()
        {
			this.isOrigin = 0;
        }
        #endregion
	}
}
