using System;

namespace WSTraceIT.Models.Request
{
	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de proveedores
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchProvidersRequest
	{
		public int providerId { get; set; }
		public string provider { get; set; }
		public string businessName { get; set; }
		public bool opc { get; set; }
		public int topc { get; set; }

		public string rawMaterial { get; set; }
		public int companyId { get; set; }
		public int productId { get; set; }
		public int packagingId { get; set; }

		#region Constructor
		public SearchProvidersRequest()
		{
			this.providerId = 0;
			this.provider = String.Empty;
			this.businessName = String.Empty;
			this.opc = false;
			this.topc = 0;

			this.rawMaterial = String.Empty;
			this.companyId = 0;
			this.productId = 0;
			this.packagingId = 0;
		}
		#endregion
	}
}
