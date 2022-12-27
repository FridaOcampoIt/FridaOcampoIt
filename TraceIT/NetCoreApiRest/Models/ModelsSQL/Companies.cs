using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.ModelsSQL
{
	/// <summary>
	/// Modelo para mapear los datos que conforma una compañia
	/// Desarrollador: David Martinez
	/// </summary>
	public class CompanyDataSQL
	{
		public CompanyDataEditionSQL companyDataEdition { get; set; }
		public List<ContactCompaniesDataSQL> contactCompaniesData { get; set; }

		public CompanyDataSQL()
		{
			this.contactCompaniesData = new List<ContactCompaniesDataSQL>();
			this.companyDataEdition = new CompanyDataEditionSQL();
		}
	}

	/// <summary>
	/// Modelo para traer los datos de la compañia en la edicion
	/// Desarrollador: David Martinez
	/// </summary>
	public class CompanyDataEditionSQL
	{
		public int idCompany { get; set; }
		public string name { get; set; }
		public string businessName { get; set; }
		public string email { get; set; }
		public string webSite { get; set; }
		public string phone { get; set; }
		public string country { get; set; }
		public string address { get; set; }
		public bool status { get; set; }
		public string facebook { get; set; }
		public string youtube { get; set; }
		public string linkedin { get; set; }
		public string clientNumber { get; set; }
        public int tipoGiro { get; set; }
        public int estatus { get; set; }

        public CompanyDataEditionSQL()
		{
			this.address = String.Empty;
			this.businessName = String.Empty;
			this.country = String.Empty;
			this.email = String.Empty;
			this.idCompany = 0;
			this.name = String.Empty;
			this.phone = String.Empty;
			this.status = false;
			this.webSite = String.Empty;
			this.facebook = String.Empty;
			this.youtube = String.Empty;
			this.linkedin = String.Empty;
			this.clientNumber = String.Empty;
			this.tipoGiro = 0;
            this.estatus = 0;
        }
	}

	/// <summary>
	/// Modelo para traer los datos de los contactos de la compañia
	/// </summary>
	public class ContactCompaniesDataSQL
	{
		public int idContact { get; set; }
		public string name { get; set; }
		public string phone { get; set; }
		public string email { get; set; }
		public bool defaultContact { get; set; }

		public ContactCompaniesDataSQL()
		{
			this.defaultContact = false;
			this.email = String.Empty;
			this.idContact = 0;
			this.name = String.Empty;
			this.phone = String.Empty;
		}
	}
}
