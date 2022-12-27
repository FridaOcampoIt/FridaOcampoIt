using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.ModelsSQL
{
	#region Backoffice
	/// <summary>
	/// Clase que contendra todos los datos necesarios de un distribuidor
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class DistributorsDataSQL
	{
		public int distributorId { get; set; }
		public string distributorNumber { get; set; }
		public string distributorName { get; set; }
		public string businessName { get; set; }
		public string email { get; set; }
		public string phone { get; set; }
		public bool status { get; set; }

		#region Constructor
		public DistributorsDataSQL()
		{
			this.distributorId = 0;
			this.distributorNumber = String.Empty;
			this.distributorName = String.Empty;
			this.businessName = String.Empty;
			this.email = String.Empty;
			this.phone = String.Empty;
			this.status = false;
		}
        #endregion
	}

	/// <summary>
	/// Clase que contendra todos los datos necesarios de un distribuidor
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ProductDistributorDataSQL
	{
		public int productId { get; set; }
		public string productName { get; set; }
		public int companyId { get; set; }
		public string companyName { get; set; }
		public int packagingId { get; set; }
		public string packagingName { get; set; }
		public string rawMaterial { get; set; }
		public int active { get; set; }
		public int distributorId { get; set; }

		#region Constructor
		public ProductDistributorDataSQL()
		{
			this.productId = 0;
			this.productName = String.Empty;
			this.companyId = 0;
			this.companyName = String.Empty;
			this.packagingId = 0;
			this.packagingName = String.Empty;
			this.rawMaterial = String.Empty;
			this.active = 0;
			this.distributorId = 0;
		}
		#endregion
	}
	#endregion
}
