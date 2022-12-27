using System;

namespace WSTraceIT.Models.Request
{
	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de empacador
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchInternalPackedRequest
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
		
		public int direccionId { get; set; }

		#region Constructor
		public SearchInternalPackedRequest()
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
	/// Clase que contiene los campos para realizar la búsqueda de empacador Interno/Externo
	/// Desarrollador: Hernán Gómez
	/// </summary>
	public class SearchInternalExternalPackedRequest
	{
		public int packedId { get; set; }
		public string packedNumber { get; set; }
		public string packedName { get; set; }
		public int type { get; set; }

		public int companyId { get; set; }

		#region Constructor
		public SearchInternalExternalPackedRequest()
		{
			this.packedId = 0;
			this.packedName = String.Empty;
			this.type = 0;
			this.companyId = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de productos asociados a un empacador
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchProductInternalPackedRequest
	{
		public int packedId { get; set; }
		public int topc { get; set; }

		public string rawMaterial { get; set; }
		public int companyId { get; set; }
		public int productId { get; set; }
		public int packagingId { get; set; }

		#region Constructor
		public SearchProductInternalPackedRequest()
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
	public class SearchBoxManagIntPackedRequest
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
		public SearchBoxManagIntPackedRequest()
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
	public class SearchArmingReportIntPackedRequest
	{
		public int typeCompany { get; set; }
		public int companyId { get; set; }
		public int addressId { get; set; }
		public int productId { get; set; }
		public string searchGeneric { get; set; }
		public string dateStart { get; set; }
		public string dateEnd { get; set; }

		#region Constructor
		public SearchArmingReportIntPackedRequest()
		{
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
}
