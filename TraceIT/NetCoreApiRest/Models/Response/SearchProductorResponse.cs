using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{

	public class searchListProductorResponse : TraceITResponse
	{
		public List<searchProductorResponse> searchListProductor { get; set; }
		public searchListProductorResponse()
		{
			this.searchListProductor = new List<searchProductorResponse>();
		}
	}

	public class searchProductorResponse: TraceITResponse
    {
		public int productorId { get; set; }
		public Boolean activo { get; set; }
		public int numeroProductor { get; set; }
		public string nombreProductor { get; set; }
		public string nombreRancho { get; set; }
		public int companiaId { get; set; }
		public string nombreAcopio { get; set; }
		public int numeroAcopio { get; set; }
		public Boolean status { get; set; }
	}

	public class searchProductorByIdResponse : TraceITResponse
	{
		public int productorId	{ get; set; }
		public Boolean activo { get; set; }
		public int numeroProductor { get; set; }
		public string nombreProductor { get; set; }
		public string nombreRancho { get; set; }
		public string direccion { get; set; }
		public string latitud { get; set; }
		public string longitud { get; set; }
		public int companiaId { get; set; }
		public string getAcopiosId { get; set; }
		//Variable Auxiliares para guardar la ubicación
		public string address { get; set; }
		public string latitude { get; set; }
		public string longitude { get; set; }
	
		public searchProductorByIdResponse()
        {
			this.productorId = 0;
			this.activo = true;
			this.numeroProductor = 0;
			this.nombreProductor =	String.Empty;
			this.nombreRancho =	String.Empty;
			this.direccion = String.Empty;
			this.latitud =	String.Empty;
			this.longitud =	String.Empty;
			this.companiaId =		0;
			this.getAcopiosId = String.Empty;
			this.address = String.Empty;
			this.latitude = String.Empty;
			this.longitude = String.Empty;
		}

	}
}
