using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class SearchLabelsQRResponse : TraceITResponse
	{
		public List<LabelsQRDataSQL> labelsqr { get; set; }

		public SearchLabelsQRResponse()
		{
			this.labelsqr = new List<LabelsQRDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class DataLabelQRCodeResponse : TraceITResponse
	{
		public int id { get; set; }
		public string basedata { get; set; }

		public DataLabelQRCodeResponse()
		{
			this.id = 0;
			this.basedata = String.Empty;
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchLabelsQRComboResponse : TraceITResponse
	{
		public List<LabelsQRDataComboSQL> labelsqrcombo { get; set; }

		public SearchLabelsQRComboResponse()
		{
			this.labelsqrcombo = new List<LabelsQRDataComboSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}
