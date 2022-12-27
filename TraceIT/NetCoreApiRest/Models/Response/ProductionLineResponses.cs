using System;
using System.Collections.Generic;
using System.Linq;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Production;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class ProductionLineResponses : TraceITResponse
	{
		public bool success { get; set; }

		public ProductionLineResponses() {
			this.success = false;
			this.messageEng = "";
			this.messageEsp = "";
		}
	}

	public class ProductionLineSummaryResponse : TraceITResponse
	{
		public List<OperationDetail> summary { get; set; }

		public ProductionLineSummaryResponse() {
			this.summary = new List<OperationDetail>();
			this.messageEng = "";
			this.messageEsp = "";
		}
	}

	public class ScannedDetailResponse : TraceITResponse { 
		public WasteMaterial details { get; set; }

		public ScannedDetailResponse() {
			details = new WasteMaterial();
			messageEng = "";
			messageEsp = "";
		}
	}

	public class ProductionLineChangeRollResponse : TraceITResponse
	{
		public int operacionId { get; set; }

		public ProductionLineChangeRollResponse()
		{
			operacionId = 0;
			messageEng = "";
			messageEsp = "";
        }
    }

	public class OperationDataResponse : TraceITResponse
	{
		public string startDate { get; set; }
		public string endDate { get; set; }
		public int? idUser { get; set; }
		public string Company { get; set; }
		public string Provider { get; set; }
		public string Operator { get; set; }
		public string Line { get; set; }
		public string Product { get; set; }
		public string Package { get; set; }
		public string grouping { get; set; }
		public int totalUnits { get; set; }
		public string range { get; set; }
		public string scannedCIUs { get; set; }
		public int unitsScanned { get; set; }
		public List<RawMaterial> rawMaterials { get; set; }
		public List<OperationDetail> operation { get; set; }
		public bool isGroup { get; set; }
		public int idOperation { get; set; }
		public string etiquetaId { get; set; }

		public OperationDataResponse()
		{
			startDate = "";
			endDate = "";
			idUser = 0;
			Company = "";
			Provider = "";
			Operator = "";
			Line = "";
			Product = "";
			Package = "";
			grouping = "";
			totalUnits = 0;
			range = "";
			scannedCIUs = "";
			unitsScanned = 0;
			rawMaterials = new List<RawMaterial>();
			operation = new List<OperationDetail>();
			isGroup = false;
			idOperation = 0;
			etiquetaId = String.Empty;
		}
	}

	public class LabelDataCodeResponse : TraceITResponse
	{
		public string company { get; set; }
		public string product { get; set; }
		public string line { get; set; }
		public string ranges { get; set; }
		public string etiquetaId { get; set; }
		public string firstScanned { get; set; }
		public string lastScanned { get; set; }
		public string grouping { get; set; }
		public string Operator { get; set; }
		public int unitBox { get; set; }
		public string pallet { get; set; }
		public string box { get; set; }
		public string startDate { get; set; }
		public string endDate { get; set; }
		

		public LabelDataCodeResponse() {
			company = String.Empty;
			product = String.Empty;
			line = String.Empty;
			ranges = String.Empty;
			etiquetaId = String.Empty;
			firstScanned = String.Empty;
			lastScanned = String.Empty;
			grouping = String.Empty;
			Operator = String.Empty;
			unitBox = 0;
			pallet = String.Empty;
			box = String.Empty;
			startDate = String.Empty;
			endDate = String.Empty;
			

        }
	}

	public class LabelDataResponse : TraceITResponse
	{
		
    }

	public class CompanyWasteResponse
	{
		public int waste { get; set; }

		public CompanyWasteResponse()
		{
			this.waste = 0;
		}
	}
}
