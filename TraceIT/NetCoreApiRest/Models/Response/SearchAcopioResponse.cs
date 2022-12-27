using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class searchAcopioResponse : TraceITResponse
	{ 
		public int acopioId { get; set; }
		public Boolean activo { get; set; }
		public int companiaId { get; set; }
		public string nombreAcopio { get; set; }
		public int numeroAcopio { get; set; }
		public string paisNombre { get; set; }
		public string estadoNombre { get; set; }
		public searchAcopioResponse()
		{
			this.acopioId = 0;
			this.activo = true;
			this.companiaId = 0;
			this.nombreAcopio = String.Empty;
			this.numeroAcopio = 0;
		}
	}

	public class searchListAcopioResponse : TraceITResponse
	{
		public List<searchAcopioResponse> searchListAcopio { get; set; }
		public searchListAcopioResponse()
		{
			this.searchListAcopio = new List<searchAcopioResponse>();
		}
	}

	public class searchAcopioByIdResponse : TraceITResponse
    {
		public int acopioId { get; set; }
		public Boolean activo { get; set; }
		public int numeroAcopio { get; set; }
		public string nombreAcopio { get; set; }
		public int paisId { get; set; }
		public int estadoId { get; set; }
		public string ciudad { get; set; }
		public int codigoPostal { get; set; }
		public string address { get; set; }
		public string latitude { get; set; }
		public string longitude { get; set; }
		public searchAcopioByIdResponse()
        {
			this.acopioId = 0;
			this.activo = true;
			this.numeroAcopio = 0;
			this.nombreAcopio = String.Empty;
			this.paisId = 0;
			this.estadoId = 0;
			this.ciudad = String.Empty;
			this.codigoPostal = 0;
			this.address = String.Empty;
			this.latitude = String.Empty;
			this.longitude = String.Empty;
		}

	}
}
