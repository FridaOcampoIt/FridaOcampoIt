using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Request
{
	public class SaveTechnicalSpecificationDetailsRequest
	{
		public TechnicalSpecificationDetailsData technicalSpecificationDetails { get; set; }
		public int technicalSpecificationId { get; set; }
	}
}
