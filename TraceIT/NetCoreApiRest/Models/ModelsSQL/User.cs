using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base.User;

namespace WSTraceIT.Models.ModelsSQL
{
	/// <summary>
	/// Clase para recuperar el nombre del archivo del usuario
	/// Desarrollador: David Martinez
	/// </summary>
	public class UserImageSQL
	{
		public string nameImage { get; set; }

		public UserImageSQL()
		{
			this.nameImage = String.Empty;
		}
	}

	/// <summary>
	/// Clase que contendra el listado de categorias para la carga inicial de la aplicacion
	/// Desarrollador: David Martinez
	/// </summary>
	public class CategoriesSQL
	{
		public int id { get; set; }
		public string name { get; set; }
		public string nameEnglish { get; set; }
		public string image { get; set; }
		public string imageBanner { get; set; }
		public string namePersonalized { get; set; }
		public string namePersonalizedEnglish { get; set; }

		public CategoriesSQL()
		{
			this.id = 0;
			this.image = String.Empty;
			this.imageBanner = String.Empty;
			this.name = String.Empty;
			this.nameEnglish = String.Empty;
			this.namePersonalized = String.Empty;
			this.namePersonalizedEnglish = String.Empty;
		}
	}

	/// <summary>
	/// Clase que contendra el listado de secciones existentes en el sistema
	/// Desarrollador: David Martinez
	/// </summary>
	public class SectionTypesSQL
	{
		public int id { get; set; }
		public string name { get; set; }
		public string nameEnglish { get; set; }

		public SectionTypesSQL()
		{
			this.id = 0;
			this.name = String.Empty;
			this.nameEnglish = String.Empty;
		}
	}

	/// <summary>
	/// Clase que contendra las listas que se tendra que consultar por defecto
	/// Desarrollador: David Martinez
	/// </summary>
	public class ListSystemSQL
	{
		public List<CategoriesSQL> categoriesSQL { get; set; }
		public List<SectionTypesSQL> sectionTypesSQL { get; set; }
		public List<LinkType> linkTypes { get; set; }
        public List<Countries> countries { get; set; }

		public ListSystemSQL()
		{
			this.categoriesSQL = new List<CategoriesSQL>();
			this.sectionTypesSQL = new List<SectionTypesSQL>();
			this.linkTypes = new List<LinkType>();
            this.countries = new List<Countries>();
		}
	}

	/// <summary>
	/// Clase que contendra el listado de los correos para ser enviados
	/// Desarrollador: David Martinez
	/// </summary>
	public class EmailUserSQL
	{
		public string email { get; set; }
	}

	/// <summary>
	/// Clase que recuperará el id de la compañia
	/// Desarrollador: David Martinez
	/// </summary>
	public class CompanyIdSQL
	{
		public int id { get; set; }
		public string qrCode { get; set; }
	}

	/// <summary>
	/// Clase para regresar los datos necesarios al registrar la garantia
	/// Desarrollador: David Martinez
	/// </summary>
	public class WarrantiesResponseSQL
	{
		public List<string> correos { get; set; }
		public string qrCode { get; set; }
		public int productId { get; set; }

		public WarrantiesResponseSQL()
		{
			this.correos = new List<string>();
			this.productId = 0;
			this.qrCode = String.Empty;
		}
	}

    /// <summary>
	/// Clase que contendra los datos para la notificación push
	/// Desarrollador: Oscar Ruesga
	/// </summary>
	public class NotificationData
    {
        public string Vencimiento { get; set; }
        public string Modelo { get; set; }
        public string ImagenUrl { get; set; }
        public string TokenPush { get; set; }
        public string TipoGarantia { get; set; }
        public int dias { get; set; }
        public int idioma { get; set; }
    }
}
