using System.Collections.Generic;
using WSTraceIT.Models.Base.Home;

namespace WSTraceIT.Models.ModelsSQL
{
	#region Mobile application
	/// <summary>
	/// Modelo utilizado para regresar los datos de Etiquetado Empacador
	/// </summary>
	public class searchEtiquetaEmabalajeFruta
	{
		public int etiquetaEmpacador { get; set; }
		public int embalajeReproceso { get; set; }
		public int recepcionFruta { get; set; }

	}

	public class searchEmpaquesEnviadosReproceso 
	{
		public int productoId { get; set; }
		public string nombre { get; set; }
		public int companyId { get; set; }
		public int totalProductos { get; set; }

	}


	public class searchFrutaRecibidaReproceso
	{
		public int productoId { get; set; }
		public string nombre { get; set; }
		public int companyId { get; set; }
		public int totalProductos { get; set; }

	}
}
#endregion
