using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.Request
{
	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de empacador
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchExternalPackedRequest
	{
		public int packedId { get; set; }
		public string packedName { get; set; }
		public bool opc { get; set; }
		public int operatorId { get; set; }
		public int lineId { get; set; }
		public int addressId { get; set; }
		public int type { get; set; }

		public int companyIdSearch { get; set; }

		public int companyId { get; set; }

		#region Constructor
		public SearchExternalPackedRequest()
		{
			this.packedId = 0;
			this.packedName = String.Empty;
			this.opc = false;
			this.operatorId = 0;
			this.lineId = 0;
			this.addressId = 0;
			this.type = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de productos asociados a un empacador
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchProductExternalPackedRequest
	{
		public int packedId { get; set; }
		public int topc { get; set; }

		public string rawMaterial { get; set; }
		public int companyId { get; set; }
		public int productId { get; set; }
		public int packagingId { get; set; }

		#region Constructor
		public SearchProductExternalPackedRequest()
		{
			this.packedId = 0;
			this.topc = 0;

			this.rawMaterial = String.Empty;
			this.companyId = 0;
			this.productId = 0;
			this.packagingId = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de gestón de cajas
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchBoxManagExtPackedRequest
	{
		public int packedId { get; set; }
		public int addressId { get; set; }
		public int productId { get; set; }
		public int groupingId { get; set; }
		public int pallet { get; set; }
		public int box { get; set; }
		public int typeView { get; set; }
		public DateTime dateStart { get; set; }
		public DateTime dateEnd { get; set; }
		public string searchfield { get; set; }

		public int companiaId { get; set; }
		#region Constructor
		public SearchBoxManagExtPackedRequest()
		{
			this.packedId = 0;
			this.addressId = 0;
			this.productId = 0;
			this.groupingId = 0;
			this.pallet = 0;
			this.box = 0;
			this.typeView = 0;
			this.dateStart = default(DateTime);
			this.dateEnd = default(DateTime);
			this.searchfield = String.Empty;

			this.companiaId = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda del reporte de armados
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchArmingReportExtPackedRequest
	{
		public int packagingId { get; set; }
		public int typeCompany { get; set; }
		public int companyId { get; set; }
		public int addressId { get; set; }
		public int productId { get; set; }
		public string searchGeneric { get; set; }
		public string dateStart { get; set; }
		public string dateEnd { get; set; }

		#region Constructor
		public SearchArmingReportExtPackedRequest()
		{
			this.packagingId = 0;
			this.typeCompany = 0;
			this.companyId = 0;
			this.addressId = 0;
			this.productId = 0;
			this.searchGeneric = String.Empty;
			this.dateStart = String.Empty;
			this.dateEnd = String.Empty;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de operaciones a unir
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchUnionOperationsExtPackedRequest
	{
		public int groupingPId { get; set; }
		public int groupingSId { get; set; }
		public int groupingType { get; set; }
		public List<int> groupings { get; set; }
		public string groupingName { get; set; }

		#region Constructor
		public SearchUnionOperationsExtPackedRequest()
		{
			this.groupingPId = 0;
			this.groupingSId = 0;
			this.groupingType = 0;
			this.groupings = new List<int>();
			this.groupingName = String.Empty;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para generar PDF con código QR y la info de una operación
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchPrintOperationExtPackedRequest
	{
		public int groupingId { get; set; }
		public int groupingType { get; set; }
		public int box { get; set; }
		public int pallet { get; set; }

		#region Constructor
		public SearchPrintOperationExtPackedRequest()
		{
			this.groupingId = 0;
			this.groupingType = 0;
			this.box = 0;
			this.pallet = 0;
		}
		#endregion
	}
}
