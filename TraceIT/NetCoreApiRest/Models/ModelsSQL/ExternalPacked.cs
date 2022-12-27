using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.ModelsSQL
{
	#region Backoffice
	/// <summary>
	/// Clase que contendra todos los datos necesarios de un empacador
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ExternalPackedDataSQL
	{
		public int packedId { get; set; }
		public string packedNumber { get; set; }
		public string packedName { get; set; }
		public string email { get; set; }
		public string password { get; set; }
		public string phone { get; set; }
		public int merma { get; set; }
		public bool status { get; set; }
		public int userId { get; set; }
		public List<int> companiasId { get; set; }

		public string getCompaniasId { get; set; }
		public string compania { get; set; }

		//Obtenemos las companias que se tenían, para si se elimina una borrar el registro (Solo como usuario TraceIt)
		public List<int> auxGetCompany { get; set; }
		public int direccionId { get; set; }

		#region Constructor
		public ExternalPackedDataSQL()
		{
			this.packedId = 0;
			this.packedNumber = String.Empty;
			this.packedName = String.Empty;
			this.email = String.Empty;
			this.password = String.Empty;
			this.phone = String.Empty;
			this.merma = 0;
			this.status = false;
			this.userId = 0;
		}
        #endregion
	}

	/// <summary>
	/// Clase que contendra todos los datos necesarios para un operador de un empacador
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ExternalPackedOperatorDataSQL
	{
		public int operatorId { get; set; }
		public string code { get; set; }
		public string name { get; set; }
		public string image { get; set; }
		public string address { get; set; }
		public int addressId { get; set; }
		public int packedId { get; set; }
		public int companyId { get; set; }

		#region Constructor
		public ExternalPackedOperatorDataSQL()
		{
			this.operatorId = 0;
			this.code = String.Empty;
			this.name = String.Empty;
			this.image = String.Empty;
			this.address = String.Empty;
			this.addressId = 0;
			this.packedId = 0;
			this.companyId = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contendra todos los datos necesarios para una linea de producción de un empacador
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ExternalPackedProdLineDataSQL
	{
		public int lineId { get; set; }
		public string name { get; set; }
		public string address { get; set; }
		public int addressId { get; set; }
		public int packedId { get; set; }
		public int companyId { get; set; }

		#region Constructor
		public ExternalPackedProdLineDataSQL()
		{
			this.lineId = 0;
			this.name = String.Empty;
			this.address = String.Empty;
			this.addressId = 0;
			this.packedId = 0;
			this.companyId = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contendra todos los datos necesarios de un proveedor
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ProductExternalPackedDataSQL
	{
		public int productId { get; set; }
		public string productName { get; set; }
		public int companyId { get; set; }
		public string companyName { get; set; }
		public int packagingId { get; set; }
		public string packagingName { get; set; }
		public string rawMaterial { get; set; }
		public int active { get; set; }
		public int packedId { get; set; }

		#region Constructor
		public ProductExternalPackedDataSQL()
		{
			this.productId = 0;
			this.productName = String.Empty;
			this.companyId = 0;
			this.companyName = String.Empty;
			this.packagingId = 0;
			this.packagingName = String.Empty;
			this.rawMaterial = String.Empty;
			this.active = 0;
			this.packedId = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contendra todos los datos necesarios para la gestión de cajas de tipo agrupador, pallet, caja
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ExtPackedBoxManagDataSQL
	{
		public int groupingId { get; set; }
		public string groupingName { get; set; }
		public int groupingType { get; set; }
		public string pallet { get; set; }
		public string box { get; set; }
		public int productId { get; set; }
		public string productName { get; set; }
		public int quantity { get; set; }
		public int isGroup { get; set; }

		#region Constructor
		public ExtPackedBoxManagDataSQL()
		{
			this.groupingId = 0;
			this.groupingName = String.Empty;
			this.groupingType = 0;
			this.pallet = String.Empty;
			this.box = String.Empty;
			this.productId = 0;
			this.productName = String.Empty;
			this.quantity = 0;
			this.isGroup = 0;
		}
		#endregion
	}

	//No mames si estan bien pendejos
	public class GestionCajasPackedBoxManagDataSQL
	{
		public int groupingId { get; set; }
		public string groupingName { get; set; }
		public int groupingType { get; set; }
		public string pallet { get; set; }
		public string box { get; set; }
		public int productId { get; set; }
		public string productName { get; set; }
		public int quantity { get; set; }
		public int isGroup { get; set; }

		#region Constructor
		public GestionCajasPackedBoxManagDataSQL()
		{
			this.groupingId = 0;
			this.groupingName = String.Empty;
			this.groupingType = 0;
			this.pallet = String.Empty;
			this.box = String.Empty;
			this.productId = 0;
			this.productName = String.Empty;
			this.quantity = 0;
			this.isGroup = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contendra todos los datos necesarios para el detalle de una agrupación
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ExtPackedBoxManagDataDetailSQL
	{
		public int groupingId { get; set; }
		public string groupingName { get; set; }
		public int groupingType { get; set; }
		public string lineName { get; set; }
		public string operatorName { get; set; }
		public int clamsShells { get; set; }
		public string registerDate { get; set; }
		public string range { get; set; }
		public string pallet { get; set; }
		public string box { get; set; }
		public string rangeDetail { get; set; }
		public int productId { get; set; }
		public string productName { get; set; }
		public int quantity { get; set; }
		public int addressId { get; set; }
		public string addressName { get; set; }

		#region Constructor
		public ExtPackedBoxManagDataDetailSQL()
		{
			this.groupingId = 0;
			this.groupingName = String.Empty;
			this.groupingType = 0;
			this.lineName = String.Empty;
			this.operatorName = String.Empty;
			this.clamsShells = 0;
			this.registerDate = String.Empty;
			this.range = String.Empty;
			this.pallet = String.Empty;
			this.box = String.Empty;
			this.rangeDetail = String.Empty;
			this.productId = 0;
			this.productName = String.Empty;
			this.quantity = 0;
			this.addressId = 0;
			this.addressName = String.Empty;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para realizar la búsqueda de reporte de armados
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ExtPackedArmingReporDataSQL
	{
		public int id { get; set; }
		public string pallet { get; set; }
		public string box { get; set; }
		public DateTime dateArm { get; set; }
		public string operatorName { get; set; }
		public string lineName { get; set; }
		public int mermas { get; set; }

		#region Constructor
		public ExtPackedArmingReporDataSQL()
		{

			this.id = 0;
			this.pallet = String.Empty;
			this.box = String.Empty;
			this.dateArm = default(DateTime);
			this.operatorName = String.Empty;
			this.lineName = String.Empty;
			this.mermas = 0;
		}
		#endregion
	}

	public class ExtPackedArmingReporDataCountSQL
	{
		public int total { get; set; }
		public string dateArm { get; set; }
		public string operatorName { get; set; }
		public string lineName { get; set; }

		#region Constructor
		public ExtPackedArmingReporDataCountSQL()
		{
			this.total = 0;
			this.dateArm = String.Empty;
			this.operatorName = String.Empty;
			this.lineName = String.Empty;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para obtener la información de las operaciones a unir
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ExtPackedOperationUnionSQL
	{
		public int operationId { get; set; }
		public int companyId { get; set; }
		public int proveedorId { get; set; }
		public int operatorId { get; set; }
		public int lineId { get; set; }
		public int productId { get; set; }
		public int packagingId { get; set; }
		public string groupingName { get; set; }
		public int totalSize { get; set; }
		public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }
		public string range { get; set; }
		public int unitsScanned { get; set; }
		public int isGroup { get; set; }
		public int nboxesPallet { get; set; }
		public int unitsPerBox { get; set; }

		#region Constructor
		public ExtPackedOperationUnionSQL()
		{
			this.operationId = 0;
			this.companyId = 0;
			this.proveedorId = 0;
			this.operatorId = 0;
			this.lineId = 0;
			this.productId = 0;
			this.packagingId = 0;
			this.groupingName = String.Empty;
			this.totalSize = 0;
			this.startDate = default(DateTime);
			this.endDate = default(DateTime);
			this.range = String.Empty;
			this.unitsScanned = 0;
			this.isGroup = 0;
			this.nboxesPallet = 0;
			this.unitsPerBox = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para obtener el detalle de las operaciones a unir
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ExtPackedOperationDetailUnionSQL
	{
		public int operationId { get; set; }
		public int detailId { get; set; }
		public string pallet { get; set; }
		public string box { get; set; }
		public string line { get; set; }
		public string rangeMin { get; set; }
		public string rangeMax { get; set; }
		public int Merma { get; set; }
		public string EtiquetaID { get; set; }

		#region Constructor
		public ExtPackedOperationDetailUnionSQL()
		{
			this.operationId = 0;
			this.detailId = 0;
			this.pallet = String.Empty;
			this.box = String.Empty;
			this.line = String.Empty;
			this.rangeMin = String.Empty;
			this.rangeMax = String.Empty;
			this.Merma = 0;
			this.EtiquetaID = String.Empty;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para obtener las unidades escaneadas por cada detalle de las operaciones a unir
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ExtPackedOperationDetailScannedUnionSQL
	{
		public int scannedId { get; set; }
		public int detailId { get; set; }
		public int order { get; set; }
		public string code { get; set; }
		public int OperacionId { get; set; }
		public int Merma { get; set; }

		#region Constructor
		public ExtPackedOperationDetailScannedUnionSQL()
		{
			this.scannedId = 0;
			this.detailId = 0;
			this.order = 0;
			this.code = String.Empty;
			this.OperacionId = 0;
			this.Merma = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para generar PDF con código QR y la info de una operación
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ExtPackedOperationInfoPrintSQL
	{
		public int groupingId { get; set; }
		public string groupingName { get; set; }
		public int groupingPId { get; set; }
		public int groupingSId { get; set; }
		public int unitsBox { get; set; }
		public int clamsShells { get; set; }
		public int quantity { get; set; }
		public string range { get; set; }
		public int detailId { get; set; }
		public string pallet { get; set; }
		public string box { get; set; }
		public string minRange { get; set; }
		public string maxRange { get; set; }
		public int scannedId { get; set; }
		public int scannedDetailId { get; set; }
		public string code { get; set; }

		#region Constructor
		public ExtPackedOperationInfoPrintSQL()
		{
			this.groupingId = 0;
			this.groupingName = String.Empty;
			this.groupingPId = 0;
			this.groupingSId = 0;
			this.unitsBox = 0;
			this.clamsShells = 0;
			this.quantity = 0;
			this.range = String.Empty;
			this.detailId = 0;
			this.pallet = String.Empty;
			this.box = String.Empty;
			this.minRange = String.Empty;
			this.maxRange = String.Empty;
			this.scannedId = 0;
			this.scannedDetailId = 0;
			this.code = String.Empty;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contiene los campos para generar PDF con código QR y la info de una operación
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class ExtPrintQRDetailSQL
	{
		public string T { get; set; }
		public string P { get; set; }
		public string I { get; set; }
		public string F { get; set; }
		public string ID { get; set; }

		#region Constructor
		public ExtPrintQRDetailSQL()
		{
			this.T = String.Empty;
			this.P = String.Empty;
			this.I = String.Empty;
			this.F = String.Empty;
			this.ID = String.Empty;
		}
		#endregion
	}

	public class OperationDatase
	{
		public string Company { get; set; }
		public string Product { get; set; }
		public string Packaging { get; set; }
		public string Grouping { get; set; }
		public string Instructions { get; set; }
		public string Operator{ get; set; }
		public string fechaRegistro { get; set; }
		public string Linea { get; set; }
		public int unitsBox { get; set; }
		public string Range { get; set; }
		public string endDate { get; set; }

		public OperationDatase()
		{
			this.Company = String.Empty;
			this.Product = String.Empty;
			this.Packaging = String.Empty;
			this.Grouping = String.Empty;
			this.Instructions = String.Empty;
			this.unitsBox = 0;
			this.Range = String.Empty;
			this.Operator = String.Empty;
			this.fechaRegistro = String.Empty;
			this.Linea = String.Empty;
			this.endDate = String.Empty;
		}
	}

	public class OperationDetailse
	{
		public int DetalleId { get; set; }
		public string Pallet { get; set; }
		public string Caja { get; set; }
		public string Linea { get; set; }
		public string RangoMin { get; set; }
		public string RangoMax { get; set; }
		public string EtiquetaID { get; set; }

		public OperationDetailse()
		{
			this.DetalleId = 0;
			this.Pallet = String.Empty;
			this.Caja = String.Empty;
			this.Linea = String.Empty;
			this.RangoMin = String.Empty;
			this.RangoMax = String.Empty;
			this.EtiquetaID = String.Empty;
        }
	}

	public class DetailsOrderse
	{
		public int detalleId { get; set; }
		public int orden { get; set; }
		public string codigo { get; set; }
		
		public DetailsOrderse()
		{
			this.detalleId = 0;
			this.orden = 0;
			this.codigo = String.Empty;
        }
    }
	#endregion
}
