using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class SearchProductorByAcopioResponse : TraceITResponse
	{ 
		public int acopioId { get; set; }
		public int numeroAcopio { get; set; }
		public string nombreAcopio { get; set; }
		public int paisId { get; set; }
		public string paisNombre { get; set; }
		public int estadoId { get; set; }
		public string nombreEstado { get; set; }
		public string ciudadAcopio { get; set; }
		public int codigoPostal { get; set; }
		public int productorId { get; set; }
		public int numeroProductor { get; set; }
		public string nombreProductor { get; set; }
		public string nombreRancho { get; set; }
		public SearchProductorByAcopioResponse()
		{
			this.acopioId = 0;
			this.numeroAcopio = 0;
			this.nombreAcopio = String.Empty;
			this.paisId = 0;
			this.paisNombre = String.Empty;
			this.estadoId = 0;
			this.nombreEstado = String.Empty;
			this.ciudadAcopio = String.Empty;
			this.codigoPostal = 0;
			this.productorId = 0;
			this.numeroProductor = 0;
			this.nombreProductor = String.Empty;
			this.nombreRancho = String.Empty;
		}
	}

	public class searchListProductorByAcopioResponse : TraceITResponse
	{
		public List<SearchProductorByAcopioResponse> searchListProductorByAcopio{ get; set; }
		public searchListProductorByAcopioResponse()
		{
			this.searchListProductorByAcopio = new List<SearchProductorByAcopioResponse>();
		}
	}


}
