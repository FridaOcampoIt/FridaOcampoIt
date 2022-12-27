using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base.Production;

namespace WSTraceIT.Models.Request
{
	/*
	 *	Here will be all the request models for this module
	 */
	public class ProductionLineRequests
	{

	}

	public class OperationDataRequest {
		public string localId { get; set; }
		public DateTime? startDate { get; set; }
		public DateTime? endDate { get; set; }
		public int? idUser { get; set; }
		public int idCompany { get; set; }
		public int idProvider { get; set; }
		public int idOperator { get; set; }
		public int idLine { get; set; }
		public int idProduct { get; set; }
		public int idPackage { get; set; }
		public string grouping { get; set; }
		public int totalUnits { get; set; }
		public string range { get; set; }
		public string scannedCIUs { get; set; }
		public int unitsScanned { get; set; }
		public int unitsPerBox { get; set; }
		public List<RawMaterial> rawMaterials { get; set; }
		public List<OperationDetail> operation { get; set; }
		public bool isGroup { get; set; }
		public int idOperation { get; set; }
		public List<CambiosRollo> cambiosRollo { get; set; }
		public List<string> codigosQR { get; set; }

		public OperationDataRequest()
		{
			localId = String.Empty;
			startDate = null;
			endDate = null;
			idUser = 0;
			idCompany = 0;
			idProvider = 0;
			idOperator = 0;
			idLine = 0;
			idProduct = 0;
			idPackage = 0;
			grouping = "";
			totalUnits = 0;
			range = "";
			scannedCIUs = "";
			unitsScanned = 0;
			unitsPerBox = 0;
			rawMaterials = new List<RawMaterial>();
			operation = new List<OperationDetail>();
			codigosQR = new List<string>();
			isGroup = false;
			idOperation = 0;
		}
	}

	public class OperationDaySummaryRequest {
		public DateTime startDate { get; set; }
		public int idCompany { get; set; }
		public int idProvider { get; set; }

		public OperationDaySummaryRequest() {
			startDate = new DateTime();
			idCompany = 0;
			idProvider = 0;
		}
	}

	public class ScannedDetailsRequest {
		public int idCompany { get; set; }
		public string code { get; set; }

		public ScannedDetailsRequest() {
			idCompany = 0;
			code = "";
		}
	}

	public class SaveWastageRequest{
		public int idCompany { get; set; }
		public List<WasteMaterial> wasteMaterial { get; set; }

		public SaveWastageRequest()
		{
			idCompany = 0;
			wasteMaterial = new List<WasteMaterial>();
		}
	}

	public class SaveRollChangeRequest{

		public int operacionId { get; set; }
		public string pallet { get; set; }
		public string caja { get; set; }
		public string ciufin { get; set; }
		public string ciuin { get; set; }

		public SaveRollChangeRequest()
		{
			this.operacionId = 0;
			this.pallet = "";
			this.caja = "";
			this.ciufin = "";
			this.ciufin = "";
        }
    }

	public class OperationRequest
	{
		public string ciu { get; set; }
		public string type { get; set; }
		public string pallet { get; set; }

		public OperationRequest()
		{
			ciu = "";
			type = "";
			pallet = "";
        }
    }

	public class OperationCodeRequest
	{
		public string code { get; set; }
		public string type { get; set; }

		public OperationCodeRequest()
		{
			this.code = "";
			this.type = "";
        }
	}

	public class LabelDataRequest
	{
		public string ciu { get; set; }
		public string tipo { get; set; }

		public LabelDataRequest()
		{
			this.ciu = "";
			this.tipo = "";
        }
    }

	public class CompanyWasteRequest
	{
		public int companyId { get; set; }

		public CompanyWasteRequest()
		{
			this.companyId = 0;
		}
	}
}
