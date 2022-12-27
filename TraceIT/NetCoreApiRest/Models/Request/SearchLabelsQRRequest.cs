using System;

namespace WSTraceIT.Models.Request
{
	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de etiquetas qr
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchLabelsQRRequest
	{
		public int labelId { get; set; }
		public string name { get; set; }
		public string code { get; set; }
		public int opc { get; set; }

		#region Constructor
		public SearchLabelsQRRequest()
		{
			this.labelId = 0;
			this.name = String.Empty;
			this.code = String.Empty;
			this.opc = 0;
		}
		#endregion
	}
}
