using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Response
{
	public class SearchTechnicalSpecificationFamilyResponse: TraceITResponse
	{
		public List<TechnicalSpecificationFamilyData> technicalSpecifications { get; set; }
		public SearchTechnicalSpecificationFamilyResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.technicalSpecifications = new List<TechnicalSpecificationFamilyData>();
		}
	}
}
