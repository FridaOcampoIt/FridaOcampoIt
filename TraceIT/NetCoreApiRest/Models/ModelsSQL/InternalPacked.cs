using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.ModelsSQL
{
	#region Backoffice
	/// <summary>
	/// Clase que contendra todos los datos necesarios de un empacador
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class InternalPackedDataSQL
	{
		public int packedId { get; set; }
		public string packedNumber { get; set; }
		public string packedName { get; set; }
		public int merma { get; set; }
		public string email { get; set; }
		public string password { get; set; }
		public string phone { get; set; }
		public bool status { get; set; }
		public int userId { get; set; }
		public List<int> companiasId { get; set; }

		public string getCompaniasId { get; set; }
		public string compania { get; set; }
		public int empacadorId { get; set; }

		public int direccionId { get; set; }
		//Obtenemos las companias que se tenían, para si se elimina una borrar el registro (Solo como usuario TraceIt)
		public List<int> auxGetCompany { get; set; }
		#region Constructor
		public InternalPackedDataSQL()
		{
			this.packedId = 0;
			this.packedNumber = String.Empty;
			this.packedName = String.Empty;
			this.merma = 0;
			this.email = String.Empty;
			this.password = String.Empty;
			this.phone = String.Empty;
			this.status = false;
			this.userId = 0;
		}
        #endregion
	}

	/// <summary>
	/// Clase que contendra todos los datos necesarios de un empacador
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class InternalExternalPackedDataSQL
	{
		public int packedId { get; set; }
		public string packedNumber { get; set; }
		public string packedName { get; set; }
		public int type { get; set; }

		public int companyId { get; set; }


		#region Constructor
		public InternalExternalPackedDataSQL()
		{
			this.packedId = 0;
			this.packedNumber = String.Empty;
			this.packedName = String.Empty;
			this.companyId = 0;
		}
		#endregion
	}

	/// <summary>
	/// Clase que contendra todos los datos necesarios para un operador de un empacador
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class InternalPackedOperatorDataSQL
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
		public InternalPackedOperatorDataSQL()
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
	public class InternalPackedProdLineDataSQL
	{
		public int lineId { get; set; }
		public string name { get; set; }
		public string address { get; set; }
		public int addressId { get; set; }
		public int packedId { get; set; }
		public int companyId { get; set; }

		#region Constructor
		public InternalPackedProdLineDataSQL()
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
	public class ProductInternalPackedDataSQL
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
		public ProductInternalPackedDataSQL()
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
	public class IntPackedBoxManagDataSQL
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
		public IntPackedBoxManagDataSQL()
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
	public class IntPackedBoxManagDataDetailSQL
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
		public IntPackedBoxManagDataDetailSQL()
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
	public class IntPackedArmingReporDataSQL
	{
		public int id { get; set; }
		public string pallet { get; set; }
		public string box { get; set; }
		public DateTime dateArm { get; set; }
		public string operatorName { get; set; }
		public string lineName { get; set; }

		#region Constructor
		public IntPackedArmingReporDataSQL()
		{

			this.id = 0;
			this.pallet = String.Empty;
			this.box = String.Empty;
			this.dateArm = default(DateTime);
			this.operatorName = String.Empty;
			this.lineName = String.Empty;
		}
		#endregion
	}

	public class IntPackedArmingReporDataCountSQL
	{
		public int total { get; set; }
		public string dateArm { get; set; }
		public string operatorName { get; set; }
		public string lineName { get; set; }

		#region Constructor
		public IntPackedArmingReporDataCountSQL()
		{
			this.total = 0;
			this.dateArm = String.Empty;
			this.operatorName = String.Empty;
			this.lineName = String.Empty;
		}
		#endregion
	}
	#endregion
}
