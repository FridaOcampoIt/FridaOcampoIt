using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSTraceIT.Models.Base.Production
{
	public class ProductionLine
	{
		public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }
		public int idUser { get; set; }
		public int idCompany { get; set; }
		public int idProvider { get; set; }
		public int idOperator { get; set; }
		public int idLine { get; set; }
		public int idProduct { get; set; }
		public int idPackage { get; set; }
		public string grouping { get; set; }
		public int totalUnits { get; set; }
		public List<RawMaterial> rawMaterials { get; set; }
		public List<OperationDetail> operation { get; set; }

		public ProductionLine()
		{
			startDate = new DateTime();
			endDate = new DateTime();
			idUser = 0;
			idCompany = 0;
			idProvider = 0;
			idOperator = 0;
			idLine = 0;
			idProduct = 0;
			idPackage = 0;
			grouping = "";
			totalUnits = 0;
			rawMaterials = new List<RawMaterial>();
			operation = new List<OperationDetail>();
		}	
	}

	public class RawMaterial
	{
		public string provider { get; set; }
		public string product { get; set; }
		public string lot { get; set; }

		public RawMaterial()
		{
			provider = "";
			product = "";
			lot = "";
		}
	}

	public class WasteMaterial
	{
		public string grouping { get; set; }
		public string pallet { get; set; }
		public string box { get; set; }
		public string ciu { get; set; }

		public WasteMaterial()
		{
			grouping = "";
			pallet = "";
			box = "";
			ciu = "";
		}
	}

	public class OperationDetail
	{
		public string grouping { get; set; }
		public string pallet { get; set; }
		public string box { get; set; }
		public string range { get; set; }
		public string line { get; set; }
		public string operatorName { get; set; }
		public int  merma { get; set; }
		public string etiquetaID { get; set; }
		public List<string> scanned { get; set; }
		public List<string> codigosQR { get; set; }


		public OperationDetail()
		{
			grouping = "";
			pallet = "";
			box = "";
			range = "";
			operatorName = "";
			merma = 0;
			etiquetaID = "";
			scanned = new List<string>();
			codigosQR = new List<string>();
		}
	}

	public class CambiosRollo
	{
		public string ultimo { get; set; }
		public string primero { get; set; }
		public string caja { get; set; }
		public string pallet { get; set; }
		public int iUltimo { get; set; }
		public int iPrimero { get; set; }
		public int iPackId { get; set; }

		public CambiosRollo()
		{
			this.ultimo = "";
			this.primero = "";
			caja = "";
			pallet = "";
			iUltimo = 0;
			iPrimero = 0;
			iPackId = 0;
		}
	}
}
