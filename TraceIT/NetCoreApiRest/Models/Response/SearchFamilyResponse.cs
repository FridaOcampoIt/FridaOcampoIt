using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Families;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class SearchFamilyResponse: TraceITResponse
	{
		public int IdProduct { get; set; }
		public int IdFamily { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string DescriptionEnglish { get; set; }
		public string Image { get; set; }
		public string Model { get; set; }
		public float Rating { get; set; }
		public bool AddTicket { get; set; }
		public bool AllowExpiration { get; set; }
		public bool AllowWarranty { get; set; }
		public string SKU { get; set; }
		public string GTIN { get; set; }
		public string QRCode { get; set; }
		public string UDID { get; set; }
		public int CategoryID { get; set; }
		public bool AggregateFamily { get; set; }
		public bool IsRated { get; set; }
		public bool IsWarranty { get; set; }
		public bool IsStolen { get; set; }
		public bool AddProductReference { get; set; }
		public string phoneCompany { get; set; }
		public int est_company { get; set; }
		public int est_familia { get; set; }
		public int est_prod { get; set; }
		public int IdProduct2 { get; set; }
		public Origin Origin { get; set; }
		public List<TechnicalSpecificationGroup> TechnicalSpecifications { get; set; }
		public List<Link> AlternateNormalUses { get; set; }
		public List<Link> InstallationGuide { get; set; }
		public List<Link> RelatedProduct { get; set; }
		public List<ServiceCenter> ServiceCenters { get; set; }
		public List<FAQ> FAQs { get; set; }
		public List<Warranty> Warranties { get; set; }
		public List<SocialMediaLinks> SocialMediaLinks { get; set; }
        public List<alertas> listAlertas { get; set; }

		public SearchFamilyResponse()
		{
			this.AddTicket = false;
			this.AllowExpiration = false;
			this.AllowWarranty = false;
			this.AddProductReference = false;
			this.AlternateNormalUses = new List<Link>();
			this.CategoryID = 0;
			this.Description = String.Empty;
			this.DescriptionEnglish = String.Empty;
			this.FAQs = new List<FAQ>();
			this.GTIN = String.Empty;
			this.IdFamily = 0;
			this.IdProduct = 0;
			this.Image = String.Empty;
			this.InstallationGuide = new List<Link>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.Model = String.Empty;
			this.Name = String.Empty;
			this.Origin = new Origin(); ;
			this.QRCode = String.Empty;
			this.AggregateFamily = false;
			this.IsRated = false;
			this.IsWarranty = false;
			this.IsStolen = false;
			this.Rating = 0;
			this.RelatedProduct = new List<Link>();
			this.ServiceCenters = new List<ServiceCenter>();
			this.SKU = String.Empty;
			this.TechnicalSpecifications = new List<TechnicalSpecificationGroup>();
			this.UDID = String.Empty;
			this.Warranties = new List<Warranty>();
			this.SocialMediaLinks = new List<SocialMediaLinks>();
			this.phoneCompany = String.Empty;
			this.est_company = 0;
			this.est_familia = 0;
			this.est_prod = 0;
			this.IdProduct2 = 0;
            this.listAlertas = new List<alertas>();
	}
	}
}
