using System;

namespace WSTraceIT.Models.Request
{
	/// <summary>
	/// Recibe los id de los acopios que tiene el usuario logeado, para recuperar los productores
	/// Desarrollador: Hernán Gómez
	/// </summary>
	public class SearchProductorByAcopioRequest
	{
		public string acopiosIds { get; set; }
		public string busqueda { get; set; }

		#region Constructor
		public SearchProductorByAcopioRequest()
		{
			this.acopiosIds = String.Empty;
			this.busqueda = String.Empty;
		}
		#endregion
	}
}
