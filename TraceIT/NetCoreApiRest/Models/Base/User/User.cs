using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.Base.User
{
	/// <summary>
	/// Clase que contendra la informacion del usuario móvil para su autenticacion
	/// Desarrolllador: David Martinez
	/// </summary>
	public class DataUserMobile
	{
		public int userId { get; set; }
		public string name { get; set; }
		public string lastname { get; set; }
		public string email { get; set; }
		public string gender { get; set; }
		public int age { get; set; }
		public string country { get; set; }
		public string city { get; set; }
		public string postalCode { get; set; }
		public string image { get; set; }
        public int configNotification { get; set; }


        public DataUserMobile()
		{
			this.userId = 0;
			this.email = String.Empty;
			this.lastname = String.Empty;
			this.name = String.Empty;
			this.gender = String.Empty;
			this.age = 0;
			this.country = String.Empty;
			this.city = String.Empty;
			this.postalCode = String.Empty;
			this.image = String.Empty;
            this.configNotification = 1;
		}
	}

	/// <summary>
	/// Clase que contendra el listado de categorias para la carga inicial de la aplicacion
	/// Desarrollador: David Martinez
	/// </summary>
	public class Categories
	{
		public int id { get; set; }
		public string name { get; set; }
		public string image { get; set; }
		public string imageBanner { get; set; }
		public string namePersonalized { get; set; }

		public Categories()
		{
			this.id = 0;
			this.name = String.Empty;
			this.image = String.Empty;
			this.imageBanner = String.Empty;
			this.namePersonalized = String.Empty;
		}
	}

	/// <summary>
	/// Clase que contendra el listado de secciones existentes en el sistema
	/// Desarrollador: David Martinez
	/// </summary>
	public class SectionTypes
	{
		public int id { get; set; }
		public string name { get; set; }

		public SectionTypes()
		{
			this.id = 0;
			this.name = String.Empty;
		}
	}

	/// <summary>
	/// Clase que contendra los tipos de vinculos existentes en el sistema
	/// Desarrollador: David Martinez
	/// </summary>
	public class LinkType
	{
		public int id { get; set; }
		public string name { get; set; }
	}

    /// <summary>
	/// Clase que contendra los paises
	/// Desarrollador: Oscar Ruesga
	/// </summary>
	public class Countries
    {
        public int id { get; set; }
        public string iso { get; set; }
        public string name { get; set; }
    }

    /// <summary>
    /// Clase que contendra las listas que se tendra que consultar por defecto
    /// Desarrollador: David Martinez
    /// </summary>
    public class ListSystem
    {
        public List<Categories> categories { get; set; }
        public List<SectionTypes> sectionTypes { get; set; }
        public List<LinkType> linkTypes { get; set; }
        public List<Countries> countries { get; set; }


		public ListSystem()
		{
			this.categories = new List<Categories>();
			this.sectionTypes = new List<SectionTypes>();
			this.linkTypes = new List<LinkType>();
            this.countries = new List<Countries>();
		}
	}

	/// <summary>
	/// Clase para cargar los productos y familias que tiene agregado un usuario
	/// Desarrollador: David Martinez
	/// </summary>
	public class ProductFamilyUsers
	{
		public int id { get; set; }
		public int familyId { get; set; }
		public int productId { get; set; }
		public int categoryId { get; set; }
		public string name { get; set; }
		public string model { get; set; }
		public string code { get; set; }
		public string expirationDate { get; set; }
		public string image { get; set; }
	}

	/// <summary>
	/// Clase para cargar los datos de los usuarios móviles para su edición
	/// Desarrollador: David Martinez
	/// </summary>
	public class UserMobileData
	{
		public string name { get; set; }
		public string lastName { get; set; }
		public string email { get; set; }
		public int age { get; set; }
		public string gender { get; set; }
		public string country { get; set; }
		public string city { get; set; }
		public string postalCode { get; set; }

		public UserMobileData()
		{
			this.age = 0;
			this.city = String.Empty;
			this.country = String.Empty;
			this.email = String.Empty;
			this.gender = String.Empty;
			this.lastName = String.Empty;
			this.name = String.Empty;
			this.postalCode = String.Empty;
		}
	}

	/// <summary>
	/// Clase para cargar los datos de las garantias registradas por los usuarios móviles
	/// Desarrollador: David Martinez
	/// </summary>
	public class WarrantiesDataUser
	{
		public string folio { get; set; }
		public string dateBuy { get; set; }
		public string placeBuy { get; set; }
		public string photoTicket { get; set; }
		public int daysNotification { get; set; }
		public string expiration { get; set; }
		public int periodMonth { get; set; }
		public string registerName { get; set; }
		public string lastNameRegister { get; set; }
		public string emailRegister { get; set; }
		public bool sendNotification { get; set; }
		public int warrantyId { get; set; }
		public string country { get; set; }
		public string city { get; set; }
		public int age { get; set; }
		public string gender { get; set; }
		public string serialNumber { get; set; }

		public WarrantiesDataUser()
		{
			this.dateBuy = String.Empty;
			this.daysNotification = 0;
			this.emailRegister = String.Empty;
			this.expiration = String.Empty;
			this.folio = String.Empty;
			this.lastNameRegister = String.Empty;
			this.periodMonth = 0;
			this.photoTicket = String.Empty;
			this.placeBuy = String.Empty;
			this.registerName = String.Empty;
			this.sendNotification = false;
			this.serialNumber = String.Empty;
			this.country = String.Empty;
			this.city = String.Empty;
			this.age = 0;
			this.gender = String.Empty;
			this.warrantyId = 0;
		}
	}
}
