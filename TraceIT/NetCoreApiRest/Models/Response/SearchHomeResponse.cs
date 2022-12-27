using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class searchEtiquetaEmabalajeFrutaResponse: TraceITResponse
	{
		public int etiquetaEmpacador { get; set; }
		public int embalajeReproceso { get; set; }
		public int recepcionFruta { get; set; }

		public searchEtiquetaEmabalajeFrutaResponse()
		{
			this.etiquetaEmpacador = 0;
			this.embalajeReproceso = 0;
			this.recepcionFruta = 0;
		}
	}

	public class searchEmpaquesEnviadosReprocesoResponse: TraceITResponse
    {
		public int productoId { get; set; }
		public string nombre { get; set; }	
		public int companyId	{ get; set; }
		public int totalProductos { get; set; }

		public List<searchEmpaquesEnviadosReprocesoResponse> listEmpaquesEnviadosReproceso { get; set; }

		public searchEmpaquesEnviadosReprocesoResponse()
        {
			this.productoId = 0;
			this.nombre = String.Empty;
			this.companyId = 0;
			this.totalProductos = 0;

		}

	}


	public class searchFrutaRecibidaReprocesoResponse : TraceITResponse
	{
		public int productoId { get; set; }
		public string nombre { get; set; }
		public int companyId { get; set; }
		public int totalProductos { get; set; }

		public List<searchFrutaRecibidaReprocesoResponse> listFrutaRecibidaReprocesoResponse { get; set; }

		public searchFrutaRecibidaReprocesoResponse()
		{
			this.productoId = 0;
			this.nombre = String.Empty;
			this.companyId = 0;
			this.totalProductos = 0;

		}

	}


	public class searchOperacionEmpacadorResponse : TraceITResponse
	{
		public int mes { get; set; }
		public int idEmpacador { get; set; }
		public int totalOperaciones { get; set; }
		public string numeroE { get; set; }
		public string nombreE { get; set; }	
		public string tipoEmpacador { get; set; }
		public string FechaInicio { get; set; }
		public int CompaniaId { get; set; }
		public List<searchOperacionEmpacadorResponse> listsearchOperacionEmpacadoresResponse { get; set; }
		public List<searchOperacionEmpacadorResponse> listsearchOperacionEmpacadorResponse { get; set; }

		public searchOperacionEmpacadorResponse()
		{
			this.mes = 0;
			this.idEmpacador = 0;
			this.totalOperaciones = 0;
			this.numeroE = String.Empty;
			this.nombreE = String.Empty;
			this.tipoEmpacador = String.Empty;
			this.FechaInicio = String.Empty;
			this.CompaniaId = 0;
		}

	}
}
