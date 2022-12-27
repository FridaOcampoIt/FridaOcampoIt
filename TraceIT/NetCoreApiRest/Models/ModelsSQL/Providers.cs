using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.ModelsSQL
{
	#region Backoffice
	/// <summary>
	/// Clase que contendra todos los datos necesarios de un proveedor
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ProvidersDataSQL
	{
		public int providerId { get; set; }
		public string providerNumber { get; set; }
		public string providerName { get; set; }
		public string businessName { get; set; }
		public string email { get; set; }
		public string phone { get; set; }
		public bool status { get; set; }
		public int userId { get; set; }

		#region Constructor
		public ProvidersDataSQL()
		{
			this.providerId = 0;
			this.providerNumber = String.Empty;
			this.providerName = String.Empty;
			this.businessName = String.Empty;
			this.email = String.Empty;
			this.phone = String.Empty;
			this.status = false;
			this.userId = 0;
		}
        #endregion
	}

	/// <summary>
	/// Clase que contendra todos los datos necesarios de un proveedor
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ProductProviderDataSQL
	{
		public int productId { get; set; }
		public string productName { get; set; }
		public int companyId { get; set; }
		public string companyName { get; set; }
		public int packagingId { get; set; }
		public string packagingName { get; set; }
		public string rawMaterial { get; set; }
		public int active { get; set; }
		public int providerId { get; set; }

		#region Constructor
		public ProductProviderDataSQL()
		{
			this.productId = 0;
			this.productName = String.Empty;
			this.companyId = 0;
			this.companyName = String.Empty;
			this.packagingId = 0;
			this.packagingName = String.Empty;
			this.rawMaterial = String.Empty;
			this.active = 0;
			this.providerId = 0;
		}
		#endregion
	}
	#endregion
}
