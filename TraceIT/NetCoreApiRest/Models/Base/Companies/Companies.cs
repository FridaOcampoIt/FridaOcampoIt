using System;

namespace WSTraceIT.Models.Base.Companies
{
	/// <summary>
	/// Modelo para traer los datos de las compañias en la consulta
	/// Desarrollador: David Martinez
	/// </summary>
	public class CompaniesData
	{
		public int idCompany { get; set; }
		public string name { get; set; }
		public string businessName { get; set; }
		public string phone { get; set; }

		public CompaniesData()
		{
			this.businessName = String.Empty;
			this.idCompany = 0;
			this.name = String.Empty;
			this.phone = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para traer los datos de la compañia en la edicion
	/// Desarrollador: David Martinez
	/// </summary>
	public class CompanyDataEdition
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
		public string youtube { get; set;}
		public string linkedin { get; set; }
		public string clientNumber { get; set; }
        public int tipoGiro { get; set; }

        public CompanyDataEdition()
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
			this.facebook = "";
			this.youtube = "";
			this.linkedin = "";
  		this.tipoGiro = 0;	
        }
	}

	/// <summary>
	/// Modelo para guardar y traer los datos de los contactos de la compañia
	/// Desarrollador: David Martinez
	/// </summary>
	public class ContactCompaniesData
	{
		public int idContactFirst { get; set; }
		public string contactNameFirst { get; set; }
		public string contactPhoneFirst { get; set; }
		public string contactEmailFirst { get; set; }
		public bool defaultFirst { get; set; }
		public int idContactSecond { get; set; }
		public string contactNameSecond { get; set; }
		public string contactPhoneSecond { get; set; }
		public string contactEmailSecond { get; set; }
		public bool defaultSecond { get; set; }

		public ContactCompaniesData()
		{
			this.contactEmailFirst = String.Empty;
			this.contactEmailSecond = String.Empty;
			this.contactNameFirst = String.Empty;
			this.contactNameSecond = String.Empty;
			this.contactPhoneFirst = String.Empty;
			this.contactPhoneSecond = String.Empty;
			this.defaultFirst = false;
			this.defaultSecond = false;
			this.idContactFirst = 0;
			this.idContactSecond = 0;
		}
	}
}
