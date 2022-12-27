using System;

namespace WSTraceIT.Models.Request
{
	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de embalajes de una familia
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchPackagingRequest
	{
		public int familyId { get; set; }
		public int packagingId { get; set; }

		#region Constructor
		public SearchPackagingRequest()
		{
			this.familyId = 0;
			this.packagingId = 0;
		}
		#endregion
	}
}
