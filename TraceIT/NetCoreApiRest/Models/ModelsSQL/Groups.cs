using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.ModelsSQL
{
	#region Backoffice
	/// <summary>
	/// Clase que contendra todos los datos necesarios de una agrupación
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class GroupDataSQL
	{
		public int groupId { get; set; }
		public string name { get; set; }
		public string pallet { get; set; }
		public string box { get; set; }
		public int productId { get; set; }
		public int quantity { get; set; }
		public DateTime registerDate { get; set; }
		public DateTime expirationDate { get; set; }
		public int companyId { get; set; }

		#region Constructor
		public GroupDataSQL()
		{
			this.groupId = 0;
			this.name = String.Empty;
			this.pallet = String.Empty;
			this.box = String.Empty;
			this.productId = 0;
			this.quantity = 0;
			this.registerDate = default(DateTime);
			this.expirationDate = default(DateTime);
			this.companyId = 0;
		}
		#endregion
	}
    #endregion
}
