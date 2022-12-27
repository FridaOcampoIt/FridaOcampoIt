using System.Collections.Generic;
using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Request
{
	public class UpdateFamilyRequest
	{
		public FamilyData familyData { get; set; }
		public List<DirectionFamilyData> directionFamily { get; set; }
		public int option { get; set; }
	}
}
