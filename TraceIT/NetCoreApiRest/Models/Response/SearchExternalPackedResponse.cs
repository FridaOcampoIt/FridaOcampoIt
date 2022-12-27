using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class SearchExternalPackedResponse : TraceITResponse
	{
		public List<ExternalPackedDataSQL> packedList { get; set; }

		public SearchExternalPackedResponse()
		{
			this.packedList = new List<ExternalPackedDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchExtPackedOperatorResponse : TraceITResponse
	{
		public List<ExternalPackedOperatorDataSQL> operatorList { get; set; }

		public SearchExtPackedOperatorResponse()
		{
			this.operatorList = new List<ExternalPackedOperatorDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchExtPackedProdLineResponse : TraceITResponse
	{
		public List<ExternalPackedProdLineDataSQL> prodLineList { get; set; }

		public SearchExtPackedProdLineResponse()
		{
			this.prodLineList = new List<ExternalPackedProdLineDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchProductExternalPackedResponse : TraceITResponse
	{
		public List<ProductExternalPackedDataSQL> productData { get; set; }
		public SearchProductExternalPackedResponse()
		{
			this.productData = new List<ProductExternalPackedDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchExtPackedBoxManagementResponse : TraceITResponse
	{
		public List<GestionCajasPackedBoxManagDataSQL> infoData { get; set; }
		public SearchExtPackedBoxManagementResponse()
		{
			this.infoData = new List<GestionCajasPackedBoxManagDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchExtPackedBoxManagementDetailResponse : TraceITResponse
	{
		public OperationDatase infoOperation { get; set; }
		public List<OperationDetailse> OperationDetails { get; set; }
		public List<DetailsOrderse> detailsOrders { get; set; }

		public SearchExtPackedBoxManagementDetailResponse()
		{
			this.infoOperation = new OperationDatase();
			this.OperationDetails = new List<OperationDetailse>();
			this.detailsOrders = new List<DetailsOrderse>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchExtPackedArmingReportResponse : TraceITResponse
	{
		public List<ExtPackedArmingReporDataCountSQL> infoDataBoxes { get; set; }
		public List<ExtPackedArmingReporDataCountSQL> infoDataPallet { get; set; }
		public List<ExtPackedArmingReporDataCountSQL> infoDataWaste { get; set; }
		public List<ExtPackedArmingReporDataCountSQL> infoDataWasteOperation { get; set; }

		public SearchExtPackedArmingReportResponse()
		{
			this.infoDataBoxes = new List<ExtPackedArmingReporDataCountSQL>();
			this.infoDataPallet = new List<ExtPackedArmingReporDataCountSQL>();
			this.infoDataWaste = new List<ExtPackedArmingReporDataCountSQL>();
			this.infoDataWasteOperation = new List<ExtPackedArmingReporDataCountSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
