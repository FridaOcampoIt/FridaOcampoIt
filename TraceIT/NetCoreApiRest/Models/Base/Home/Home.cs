using System;

namespace WSTraceIT.Models.Base.Home
{
	/// <summary>
	/// Modelo para mapear la información de etiquetado de empacadores
	/// Desarrollador: Hernán Gómez
	/// </summary>
	public class searchEtiquetaEmabaljeFruta
	{
		public int etiquetaEmpacador { get; set; }
		public int embalajeReproceso { get; set; }
		public int recepcionFruta { get; set; }
		public searchEtiquetaEmabaljeFruta()
		{
			this.etiquetaEmpacador = 0;
			this.embalajeReproceso = 0;
			this.recepcionFruta = 0;
 		}
	}

	public class searchEmpaquesEnviadosReproceso
    {
		public int productoId { get; set; }
		public string nombre { get; set; }
		public int	companyId { get; set; }
		public int	TotalProductos { get; set; }
		public searchEmpaquesEnviadosReproceso()
        {
			this.productoId = 0;
 			this.nombre = String.Empty;
			this.companyId = 0;
			this.TotalProductos = 0;

		}
	}

	public class searchFrutaRecibidaReproceso
	{
		public int productoId { get; set; }
		public string nombre { get; set; }
		public int companyId { get; set; }
		public int totalProductos { get; set; }
		public searchFrutaRecibidaReproceso()
		{
			this.productoId = 0;
			this.nombre = String.Empty;
			this.companyId = 0;
			this.totalProductos = 0;

		}
	}

	public class searchOperacionEmpacador
	{
		public int mes { get; set; }
		public int idEmpacador { get; set; }
		public int totalOperaciones { get; set; }
		public string numeroE { get; set; }
		public string nombreE { get; set; }
		public string tipoEmpacador { get; set; }
		public string FechaInicio { get; set; }
		public int CompaniaId { get; set; }

		public searchOperacionEmpacador()
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
