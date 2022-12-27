using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Request
{
	public class SaveTechnicalSpecificationRequest
	{
		public TechnicalSpecificationData technicalSpecification { get; set; }
		public int familyId { get; set; }
	}
}
