using System;
using System.Collections.Generic;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Request
{
	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de agrupaciones
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchPackedLabeledRequest
	{
		public int companyId { get; set; }
		public int productId { get; set; }
		public string searchGeneric { get; set; }
		public string dateStart { get; set; }
		public string dateEnd { get; set; }
		public bool chkDistributor { get; set; }
		public int opc { get; set; }
		public int empacadorId { get; set; }

		#region Constructor
		public SearchPackedLabeledRequest()
		{
			this.companyId = 0;
			this.productId = 0;
			this.searchGeneric = String.Empty;
			this.dateStart = String.Empty;
			this.dateEnd = String.Empty;
			this.chkDistributor = false;
			this.opc = 0;
			this.empacadorId = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de agrupaciones
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchPackedLabeledCombosRequest
	{
		public int id { get; set; }
		public int id2 { get; set; }
		public string typeCompany { get; set; }

		#region Constructor
		public SearchPackedLabeledCombosRequest()
		{
            this.id = 0;
			this.id2 = 0;
			this.typeCompany = "";
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda operadores y lineas de producción de una compañia
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchCompanyInfoCombosRequest
	{
		public int id { get; set; }
		public int opc { get; set; }

		#region Constructor
		public SearchCompanyInfoCombosRequest()
		{
			this.id = 0;
			this.opc = 0;
		}
		#endregion
	}

	public class SearchCompanyInfoCombosEmbalajeRequest
	{
		public int id { get; set; }
		public int opc { get; set; }
		public int familiaId { get; set; }
		public int providerId { get; set; }

		#region Constructor
		public SearchCompanyInfoCombosEmbalajeRequest()
		{
			this.id = 0;
			this.opc = 0;
			this.familiaId = 0;
			this.providerId = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de materias primas de un proveedor
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchRawMaterialsRequest
	{
		public int companyId { get; set; }
		public int productId { get; set; }
		public int packagingId { get; set; }
		public int providerId { get; set; }

		#region Constructor
		public SearchRawMaterialsRequest()
		{
			this.companyId = 0;
			this.productId = 0;
			this.packagingId = 0;
			this.providerId = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase para recibir si existe un ciu escaneado
	/// Desarrollador: Omar Larrion
	/// </summary>
	public class SearchExisteCIURequest
	{
		public string ciu { get; set; }

        #region Constructor
        public SearchExisteCIURequest()
		{
			this.ciu = "";
        }
		#endregion
    }

	/// <summary>
	/// Clase para realizar la busqueda de cajas e info de la operación de un pallet / caja
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchQrCodeRequest
	{
		public string ciuI { get; set; }
		public string ciuF { get; set; }
		public int init { get; set; }
		public int productId { get; set; }
		public int packId { get; set; }

		#region Constructor
		public SearchQrCodeRequest()
		{
			this.ciuI = String.Empty;
			this.ciuF = String.Empty;
			this.init = 0;
			this.productId = 0;
			this.packId = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase para mapear los datos de una nueva operación
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class SearchOperationPallet
	{
		public List<InfoQRCodeSQL> pallets { get; set; }

		#region Constructor
		public SearchOperationPallet()
		{
			this.pallets = new List<InfoQRCodeSQL>();
		}
		#endregion
	}
}
