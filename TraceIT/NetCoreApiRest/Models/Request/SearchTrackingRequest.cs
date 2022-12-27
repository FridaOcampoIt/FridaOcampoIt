using System;

namespace WSTraceIT.Models.Request
{
	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de rastreos
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchTrackingRequest
	{
		public int trackingId { get; set; }
		public string searchCode { get; set; }
		public string searchCIUCode { get; set; }
		public int phase { get; set; }
		public int opc { get; set; }
		public int companiaId { get; set; }

		#region Constructor
		public SearchTrackingRequest()
		{
			this.trackingId = 0;
			this.searchCode = String.Empty;
			this.searchCIUCode = String.Empty;
			this.phase = 0;
			this.opc = 0;
			this.companiaId = 0;
		}
		#endregion
	}

	public class SearchMovementRequest
	{
		public string ciu { get; set; }
		public int type { get; set; }
		public string tipoBusqueda { get; set; }

		public SearchMovementRequest()
		{
			this.ciu = String.Empty;
			this.type = 0;
			this.tipoBusqueda = String.Empty;
		}
	}

	public class LogTrackingRequest
	{
		public string ciu { get; set; }
		public string lat { get; set; }
		public string lon { get; set; }
		public string json { get; set; }
		public int tipo { get; set; }

		public LogTrackingRequest()
		{
			this.ciu = String.Empty;
			this.lat = String.Empty;
			this.lon = String.Empty;
			this.json = String.Empty;
			this.tipo = 0;
		}
	}

	public class RecepcionRequest
	{
		public int movimientoId { get; set; }
		public string nombre { get; set; }
		public string apellido { get; set; }
		public string cargo { get; set; }
		public string fecha { get; set; }
		public string lat { get; set; }
		public string lon { get; set; }

		public RecepcionRequest()
		{
			this.movimientoId = 0;
			this.nombre = String.Empty;
			this.apellido = String.Empty;
			this.cargo = String.Empty;
			this.fecha = String.Empty;
			this.lat = String.Empty;
			this.lon = String.Empty;
        }

    }
}
