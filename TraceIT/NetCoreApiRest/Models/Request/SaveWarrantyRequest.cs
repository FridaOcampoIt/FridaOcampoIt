using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Request
{
	public class SaveWarrantyRequest
	{
		public WarrantyData warranty { get; set; }
		public int familyId { get; set; }
	}
}
