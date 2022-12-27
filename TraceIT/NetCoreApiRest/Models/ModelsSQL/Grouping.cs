using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.ModelsSQL
{
	#region Backoffice
	/// <summary>
	/// Clase que contendra todos los datos necesarios de una agrupación
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class GroupingDataSQL
	{
		public int providerId { get; set; }
		public string providerName { get; set; }
		public int groupingId { get; set; }
		public string groupingName { get; set; }
		public string pallet { get; set; }
		public string box { get; set; }
		public int productId { get; set; }
		public string productName { get; set; }
		public int quantity { get; set; }
		public string registerDate { get; set; }
		public string expirationDate { get; set; }
		public int companyId { get; set; }
		public string companyName { get; set; }
		public string nombreEmpacador { get; set; }

		#region Constructor
		public GroupingDataSQL()
		{
			this.providerId = 0;
			this.providerName = String.Empty;
			this.groupingId = 0;
			this.groupingName = String.Empty;
			this.pallet = String.Empty;
			this.box = String.Empty;
			this.productId = 0;
			this.productName = String.Empty;
			this.quantity = 0;
			this.registerDate = String.Empty;
			this.expirationDate = String.Empty;
			this.companyId = 0;
			this.companyName = String.Empty;
			this.nombreEmpacador = String.Empty;
		}
		#endregion
	}
    #endregion
}
