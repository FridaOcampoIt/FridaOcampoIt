using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.Request
{
	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de empacador
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchAgroPackedRequest
	{
		public int producerId { get; set; }
		public string producerNumber { get; set; }

		#region Constructor
		public SearchAgroPackedRequest()
		{
			this.producerId = 0;
			this.producerNumber = String.Empty;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda del reporte de embalaje reproceso
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchReprocessingReportAgroPackedRequest
	{
		public string producerNumber { get; set; }
		public int productId { get; set; }
		public string dateStart { get; set; }
		public string dateEnd { get; set; }

		#region Constructor
		public SearchReprocessingReportAgroPackedRequest()
		{
			this.producerNumber = String.Empty;
			this.productId = 0;
			this.dateStart = String.Empty;
			this.dateEnd = String.Empty;
		}
		#endregion
	}
}
