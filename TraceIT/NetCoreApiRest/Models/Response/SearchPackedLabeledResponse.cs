using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class SearchCompanyPackLabComboResponse : TraceITResponse
	{
		public List<PackedLabeledComboSQL> companiescombo { get; set; }

		public SearchCompanyPackLabComboResponse()
		{
			this.companiescombo = new List<PackedLabeledComboSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchProductPackLabComboResponse : TraceITResponse
	{
		public List<PackedLabeledComboSQL> productscombo { get; set; }

		public SearchProductPackLabComboResponse()
		{
			this.productscombo = new List<PackedLabeledComboSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchPackagingPackLabComboResponse : TraceITResponse
	{
		public List<PackedLabeledComboSQL> packagingcombo { get; set; }

		public SearchPackagingPackLabComboResponse()
		{
			this.packagingcombo = new List<PackedLabeledComboSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchComProdPackComboResponse : TraceITResponse
	{
		public List<TraceITListDropDown> companiescombo { get; set; }
		public List<TraceITListDropDown> productscombo { get; set; }
		public List<TraceITListDropDown> packagingcombo { get; set; }

		public SearchComProdPackComboResponse()
		{
			this.companiescombo = new List<TraceITListDropDown>();
			this.productscombo = new List<TraceITListDropDown>();
			this.packagingcombo = new List<TraceITListDropDown>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchCompanyInfoComboResponse : TraceITResponse
	{
		public List<PackedLabeledComboSQL> infocombo { get; set; }

		public SearchCompanyInfoComboResponse()
		{
			this.infocombo = new List<PackedLabeledComboSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchFamilyProductPackLabComboResponse : TraceITResponse
	{
		public List<PackedLabeledComboSQL> familiescombo { get; set; }

		public SearchFamilyProductPackLabComboResponse()
		{
			this.familiescombo = new List<PackedLabeledComboSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchRawMaterialsResponse : TraceITResponse
	{
		public List<RawMaterialsSQL> materialsLst { get; set; }

		public SearchRawMaterialsResponse()
		{
			this.materialsLst = new List<RawMaterialsSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchAddressCompanyResponse : TraceITResponse
	{
		public List<PackedLabeledComboSQL> addressLst { get; set; }

		public SearchAddressCompanyResponse()
		{
			this.addressLst = new List<PackedLabeledComboSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchExisteCIUResponse: TraceITResponse
	{
		public List<ExisteCIUSQL> existeLst { get; set; }

		public SearchExisteCIUResponse()
		{
			this.existeLst = new List<ExisteCIUSQL>();
        }
    }

	public class SearchInfoQRResponse : TraceITResponse
	{
		public List<InfoQRCodeSQL> infoLst { get; set; }

		public SearchInfoQRResponse()
		{
			this.infoLst = new List<InfoQRCodeSQL>();
		}
	}

	public class saveOperationPalletResponse : TraceITResponse
	{
		public bool success { get; set; }

		public saveOperationPalletResponse()
		{
			this.success = false;
			this.messageEng = "";
			this.messageEsp = "";
		}
	}
}
