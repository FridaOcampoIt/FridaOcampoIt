using System;

namespace WSTraceIT.Models.Request
{
	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de direcciones de proveedor
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchAddressProviderRequest
	{
		public string typeCompany { get; set; }
		public int familyId { get; set; }
		public int addressId { get; set; }
		public int empacadorId { get; set; }

		#region Constructor
		public SearchAddressProviderRequest()
		{
			this.typeCompany = String.Empty;
			this.familyId = 0;
			this.addressId = 0;
			this.empacadorId = 0;
		}
		#endregion
	}
}
