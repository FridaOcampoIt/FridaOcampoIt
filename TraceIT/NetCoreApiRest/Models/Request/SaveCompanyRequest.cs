using System.Numerics;
using WSTraceIT.Models.Base.Companies;

namespace WSTraceIT.Models.Request
{
	public class SaveCompanyRequest
	{
		public string name { get; set; }
		public string businessName { get; set; }
		public string email { get; set; }
		public string webSite { get; set; }
		public string phone{ get; set; }
		public string country { get; set; }
		public string address { get; set; }
		public bool status { get; set; }
		public string facebook { get; set; }
		public string youtube { get; set; }
		public string linkedin { get; set; }
		public string clientNumber { get; set; }
        public int tipoGiro { get; set; }
        public ContactCompaniesData contactCompanies { get; set; }
	}
}
