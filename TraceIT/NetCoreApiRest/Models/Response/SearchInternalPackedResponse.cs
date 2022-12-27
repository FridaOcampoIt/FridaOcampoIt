using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class SearchInternalPackedResponse : TraceITResponse
	{
		public List<InternalPackedDataSQL> packedList { get; set; }

		public SearchInternalPackedResponse()
		{
			this.packedList = new List<InternalPackedDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchInternalExternalPackedListResponse : TraceITResponse
    {
		public List<InternalExternalPackedDataSQL> packedList { get; set; }
        public SearchInternalExternalPackedListResponse()
        {
			this.packedList = new List<InternalExternalPackedDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
        }
	}

	public class SearchIntPackedOperatorResponse : TraceITResponse
	{
		public List<InternalPackedOperatorDataSQL> operatorList { get; set; }

		public SearchIntPackedOperatorResponse()
		{
			this.operatorList = new List<InternalPackedOperatorDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchIntPackedProdLineResponse : TraceITResponse
	{
		public List<InternalPackedProdLineDataSQL> prodLineList { get; set; }

		public SearchIntPackedProdLineResponse()
		{
			this.prodLineList = new List<InternalPackedProdLineDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchProductInternalPackedResponse : TraceITResponse
	{
		public List<ProductInternalPackedDataSQL> productData { get; set; }
		public SearchProductInternalPackedResponse()
		{
			this.productData = new List<ProductInternalPackedDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchIntPackedBoxManagementResponse : TraceITResponse
	{
		public List<GestionCajasPackedBoxManagDataSQL> infoData { get; set; }
		public SearchIntPackedBoxManagementResponse()
		{
			this.infoData = new List<GestionCajasPackedBoxManagDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchIntPackedBoxManagementDetailResponse : TraceITResponse
	{
		public OperationDatase infoOperation { get; set; }
		public List<OperationDetailse> OperationDetails { get; set; }
		public List<DetailsOrderse> detailsOrders { get; set; }

		public SearchIntPackedBoxManagementDetailResponse()
		{
			this.infoOperation = new OperationDatase();
			this.OperationDetails = new List<OperationDetailse>();
			this.detailsOrders = new List<DetailsOrderse>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchIntPackedArmingReportResponse : TraceITResponse
	{
		public List<IntPackedArmingReporDataCountSQL> infoDataBoxes { get; set; }
		public List<IntPackedArmingReporDataCountSQL> infoDataPallet { get; set; }
		public List<IntPackedArmingReporDataCountSQL> infoDataWaste { get; set; }
		public SearchIntPackedArmingReportResponse()
		{
			this.infoDataBoxes = new List<IntPackedArmingReporDataCountSQL>();
			this.infoDataPallet = new List<IntPackedArmingReporDataCountSQL>();
			this.infoDataWaste = new List<IntPackedArmingReporDataCountSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
