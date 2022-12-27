using System.Collections.Generic;
using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Request
{
	public class SaveFamilyRequest
	{
		public FamilyData familyData { get; set; }
		public List<DirectionFamilyData> directionFamily { get; set; }
	}
}
