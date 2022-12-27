using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class SearchCountriesResponse: TraceITResponse
	{
		public List<TraceITListDropDown> countriesData { get; set; }

		public List<TraceITListDropDown> estadosData { get; set; }

		public SearchCountriesResponse()
		{
			this.countriesData = new List<TraceITListDropDown>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}


	public class SearchEstadosByPaisIdResponse : TraceITResponse
	{

		public List<TraceITListDropDown> estadosData { get; set; }

		public SearchEstadosByPaisIdResponse()
		{
			this.estadosData = new List<TraceITListDropDown>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}


	public class SearchEstadosByPaisIdRequest : TraceITResponse
	{

		public int paisId { get; set; }

		public SearchEstadosByPaisIdRequest()
		{
			this.paisId = 0;
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
