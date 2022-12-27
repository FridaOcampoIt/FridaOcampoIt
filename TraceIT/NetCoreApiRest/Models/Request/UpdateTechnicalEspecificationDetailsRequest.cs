using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Request
{
	public class UpdateTechnicalSpecificationDetailsRequest
	{
		public TechnicalSpecificationDetailsData technicalSpecificationDetails { get; set; }
		public bool imagenEliminado { get; set; }
	}
}
