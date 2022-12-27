using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.ModelsSQL
{
	#region Backoffice
	/// <summary>
	/// Clase que contendra todos los datos necesarios de un empacador
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class AgroPackedDataSQL
	{
		public int producerId { get; set; }
		public string producerNumber { get; set; }
		public string producerName { get; set; }
		public string ranch { get; set; }
		public string address { get; set; }

		#region Constructor
		public AgroPackedDataSQL()
		{
			this.producerId = 0;
			this.producerNumber = String.Empty;
			this.producerName = String.Empty;
			this.ranch = String.Empty;
			this.address = String.Empty;
		}
        #endregion
	}

	/// <summary>
	/// Clase que contendra todos los datos necesarios para las gráficas del reporte de embalaje reproceso
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class AgroPackedPackagingReporDataCountSQL
	{
		public int total { get; set; }
		public string dateMov { get; set; }
		public int id { get; set; }

		#region Constructor
		public AgroPackedPackagingReporDataCountSQL()
		{
			this.total = 0;
			this.dateMov = String.Empty;
			this.id = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos a mapear para la búsqueda de reporte de embalaje reproceso
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class AgroPackedPackagingReproReporDataSQL
	{
		public int id { get; set; }
		public int parentId { get; set; }
		public int typeMov { get; set; }
		public DateTime dateMov { get; set; }
		public int quantity { get; set; }
		public int received { get; set; }
		public int waste { get; set; }

		#region Constructor
		public AgroPackedPackagingReproReporDataSQL()
		{

			this.id = 0;
			this.parentId = 0;
			this.typeMov = 0;
			this.dateMov = default(DateTime);
			this.quantity = 0;
			this.received = 0;
			this.waste = 0;
		}
		#endregion
	}
	#endregion
}
