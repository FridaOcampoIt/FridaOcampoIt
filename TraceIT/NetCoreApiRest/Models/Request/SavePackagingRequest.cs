using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.Request
{
	/// <summary>
	/// Clase que contiene los campos para realizar el guardado del embalaje de una familia
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SavePackagingRequest
	{
		public int packagingId { get; set; }
		public string packagingType { get; set; }
		public string readingType { get; set; }
		public int boxLabelType { get; set; }
		public int boxLabelPallet { get; set; }
		public int unitsPerBox { get; set; }
		public int copiesPerBox { get; set; }
		public int linesPerBox { get; set; }
		public decimal grossWeightPerBox { get; set; }
		public string dimensionsWeightPerBox { get; set; }
		public int boxesPerPallet { get; set; }
		public int copiesPerPallet { get; set; }
		public decimal grossWeightPerPallet { get; set; }
		public string dimensionsPerPallet { get; set; }
		public string instructionsWarnings { get; set; }
		public int familyId { get; set; }
		public List<int> empacadoresId { get; set; }
		public List<int> auxEmpacadoresId { get; set; }

		#region Constructor
		public SavePackagingRequest()
		{
			this.packagingId = 0;
			this.packagingType = String.Empty;
			this.readingType = String.Empty;
			this.boxLabelType = 0;
			this.boxLabelPallet = 0;
			this.unitsPerBox = 0;
			this.copiesPerBox = 0;
			this.linesPerBox = 0;
			this.grossWeightPerBox = 0;
			this.dimensionsWeightPerBox = "";
			this.boxesPerPallet = 0;
			this.copiesPerPallet = 0;
			this.grossWeightPerPallet = 0;
			this.dimensionsPerPallet = "";
			this.instructionsWarnings = String.Empty;
			this.familyId = 0;
		}
		#endregion
	}
}
