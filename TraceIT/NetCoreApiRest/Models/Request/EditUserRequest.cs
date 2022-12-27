using System.Collections.Generic;

namespace WSTraceIT.Models.Request
{
	public class EditUserRequest
	{
		public int idUser { get; set; }
		public string name { get; set; }
		public string lastName { get; set; }
		public string email { get; set; }
		public string password { get; set; }
		public string position { get; set; }
		public int companyId { get; set; }
		public int rolId { get; set; }
		public int profile { get; set; }
		public List<int> acopiosIds { get; set; }
		public List<int> auxAcopiosIds { get; set; }
	}
}
