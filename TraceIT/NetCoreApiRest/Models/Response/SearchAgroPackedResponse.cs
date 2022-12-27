using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class SearchAgroPackedResponse : TraceITResponse
	{
		public List<AgroPackedDataSQL> producerList { get; set; }

		public SearchAgroPackedResponse()
		{
			this.producerList = new List<AgroPackedDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchAgroPackedPackagingReproReportResponse : TraceITResponse
	{
		public List<AgroPackedPackagingReporDataCountSQL> infoDataShipment { get; set; }
		public List<AgroPackedPackagingReporDataCountSQL> infoDataReceived { get; set; }
		public List<AgroPackedPackagingReporDataCountSQL> infoDataWaste { get; set; }
		public List<AgroPackedPackagingReporDataCountSQL> infoDataWasteReport { get; set; }

		public SearchAgroPackedPackagingReproReportResponse()
		{
			this.infoDataShipment = new List<AgroPackedPackagingReporDataCountSQL>();
			this.infoDataReceived = new List<AgroPackedPackagingReporDataCountSQL>();
			this.infoDataWaste = new List<AgroPackedPackagingReporDataCountSQL>();
			this.infoDataWasteReport = new List<AgroPackedPackagingReporDataCountSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
