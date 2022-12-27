using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.ModelsSQL
{
	#region Backoffice
	/// <summary>
	/// Clase que contendra todos los datos necesarios de una etiqueta qr
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class LabelsQRDataSQL
	{
		public int labelId { get; set; }
		public string name { get; set; }
		public int orientation { get; set; }
		public bool grouper { get; set; }
		public int nChildren { get; set; }
		public int topPrimary { get; set; }
		public int topSecondary { get; set; }
		public int rightPrimary { get; set; }
		public int rightSecondary { get; set; }
		public int bottomPrimary { get; set; }
		public int bottomSecondary { get; set; }
		public int leftPrimary { get; set; }
		public int leftSecondary { get; set; }
		public int companyId { get; set; }

		#region Constructor
		public LabelsQRDataSQL()
		{
			this.labelId = 0;
			this.name = String.Empty;
			this.orientation = 0;
			this.grouper = false;
			this.nChildren = 0;
			this.topPrimary = 0;
			this.topSecondary = 0;
			this.rightPrimary = 0;
			this.rightSecondary = 0;
			this.bottomPrimary = 0;
			this.bottomSecondary = 0;
			this.leftPrimary = 0;
			this.leftSecondary = 0;
			this.companyId = 0;
		}
        #endregion
	}

	/// <summary>
	/// Clase que contendra todos los datos necesarios de una etiqueta qr para los combos
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class LabelsQRDataComboSQL
	{
		public int id { get; set; }
		public string data { get; set; }

		#region Constructor
		public LabelsQRDataComboSQL()
		{
			this.id = 0;
			this.data = String.Empty;
		}
		#endregion
	}
	#endregion
}
