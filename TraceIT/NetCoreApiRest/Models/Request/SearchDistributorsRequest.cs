using System;

namespace WSTraceIT.Models.Request
{
	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de distribuidores
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchDistributorsRequest
	{
		public int distributorId { get; set; }
		public string distributor { get; set; }
		public string businessName { get; set; }
		public bool opc { get; set; }
		public int topc { get; set; }

		public string rawMaterial { get; set; }
		public int companyId { get; set; }
		public int productId { get; set; }
		public int packagingId { get; set; }

		#region Constructor
		public SearchDistributorsRequest()
		{
			this.distributorId = 0;
			this.distributor = String.Empty;
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
