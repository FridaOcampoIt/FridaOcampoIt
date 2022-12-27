namespace WSTraceIT.Models.Request
{
	public class SaveWarrantyUserRequest
	{
		public string dateBuy { get; set; }
		public string placeBuy { get; set; }
		public string photoTicket { get; set; }
		public int daysNotification { get; set; }
		public string expiration { get; set; }
		public int periodMonth { get; set; }
		public string serialNumber { get; set; }
		public string registerName { get; set; }
		public string lastNameRegister { get; set; }
		public string emailRegister { get; set; }
		public bool sendNotification { get; set; }
		public int warrantyId { get; set; }
		public int userMobileId { get; set; }
		public string country { get; set; }
		public string city { get; set; }
		public int age { get; set; }
		public string gender { get; set; }
	}
}
