using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.ModelsSQL
{
	#region Mobile application
	/// <summary>
	/// Clase principal que contiene la información de la familia
	/// Desarrollador: David Martinez
	/// </summary>
	public class InformationFamily
	{
		public FamilySQL FamilyInformation { get; set; }
		public List<TechnicalSpecificationGroupSQL> TechnicalSpecification { get; set; }
		public List<TechnicalSpecificationSQL> TechnicalSpecificationDetails { get; set; }
		public List<LinkSQL> AlternateNormalUsesList { get; set; }
		public List<LinkSQL> InstallationGuideList { get; set; }
		public List<RelatedProductSQL> RelatedProductList { get; set; }
		public List<ServiceCenterSQL> ServiceCenters { get; set; }
		public List<FAQsSQL> FAQsList { get; set; }
		public List<WarrantiesSQL> WarrantiesList { get; set; }
		public List<SocialMediaLinkSQL> SocialMediaLinks { get; set; }
        public List<OriginSQL> originList { get; set; }
		//public string username { get; set; }

		public InformationFamily()
		{
			this.AlternateNormalUsesList = new List<LinkSQL>();
			this.FamilyInformation = new FamilySQL();
			this.FAQsList = new List<FAQsSQL>();
			this.InstallationGuideList = new List<LinkSQL>();
			this.RelatedProductList = new List<RelatedProductSQL>();
			this.TechnicalSpecification = new List<TechnicalSpecificationGroupSQL>();
			this.WarrantiesList = new List<WarrantiesSQL>();
			this.ServiceCenters = new List<ServiceCenterSQL>();
			this.SocialMediaLinks = new List<SocialMediaLinkSQL>();
			//this.username = "";
		}
	}

    /// <summary>
	/// Clase para mapear el origen del producto
	/// Desarrollador: Oscar Ruesga
	/// </summary>
	public class OriginSQL
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Manufacturer { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
		public string Ciu { get; set; } // Nuevo origen, obtenido a partir de CIU
		public string Nombre { get; set; }
		public string Presentacion { get; set; }
		public string Marca { get; set; }
		public string NumSerie { get; set; }
		public string Lote { get; set; }
		public string Caducidad { get; set; }
		public string GTIN { get; set; }
		public string Empresa { get; set; }
		public string Pais { get; set; }
		public string Ciudad { get; set; }
		public string Estado { get; set; }
		public string Planta { get; set; }
		public string LineaProd { get; set; }
		public string Rancho { get; set; }
		public string LineaProduccion { get; set; }
		public string FechaProduccion { get; set; }
		public string CosecheroNombre { get; set; }
		public string CosecheroPuesto { get; set; }
		public string CosecheroDescripcion { get; set; }
		public string CosecheroImagen { get; set; }
		public string Cosecha { get; set; }
		public string Sector { get; set; }

		public OriginSQL()
        {
            this.Address = String.Empty;
            this.Country = String.Empty;
            this.Lat = 0;
            this.Lon = 0;
            this.Manufacturer = String.Empty;
			this.Ciu = String.Empty;
			this.Nombre = String.Empty;
			this.Presentacion = String.Empty;
			this.Marca = String.Empty;
			this.NumSerie = String.Empty;
			this.Lote = String.Empty;
			this.Caducidad = String.Empty;
			this.GTIN = String.Empty;
			this.Empresa = String.Empty;
			this.Pais = String.Empty;
			this.Ciudad = String.Empty;
			this.Estado = String.Empty;
			this.Planta = String.Empty;
			this.LineaProd = String.Empty;
			this.Rancho = String.Empty;
			this.FechaProduccion = String.Empty;
			this.CosecheroNombre = String.Empty;
			this.CosecheroDescripcion = String.Empty;
			this.CosecheroImagen = String.Empty;
			this.Cosecha = String.Empty;
			this.Sector = String.Empty;
		}
    }

    /// <summary>
    /// Clase para guardar la información que hace referencia a la familia
    /// Desarrollador: David Martinez
    /// </summary>
    public class FamilySQL
	{
		public int IdFamily { get; set; }
		public int IdProduct { get; set; }
		public string UDID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string DescriptionEnglish { get; set; }
		public string Image { get; set; }
		public string Model { get; set; }
		public decimal Rating { get; set; }
		public bool AddTicket { get; set; }
		public bool AllowExpiration { get; set; }
		public bool AllowWarranty { get; set; }
		public string SKU { get; set; }
		public string GTIN { get; set; }
		public int CategoryID { get; set; }
		public bool AggregateFamily { get; set; }
		public bool IsRated { get; set; }
		public bool IsWarranty { get; set; }
		public bool IsStolen { get; set; }
		public bool AddProductReference { get; set; }
		public string phoneCompany { get; set; }

		public FamilySQL()
		{
			this.IdFamily = 0;
			this.IdProduct = 0;
			this.UDID = String.Empty;
			this.Name = String.Empty;
			this.Description = String.Empty;
			this.DescriptionEnglish = String.Empty;
			this.Image = String.Empty;
			this.Model = String.Empty;
			this.Rating = 0;
			this.AddTicket = false;
			this.SKU = String.Empty;
			this.GTIN = String.Empty;
			this.CategoryID = 0;
			this.AllowExpiration = false;
			this.AllowWarranty = false;
			this.AggregateFamily = false;
			this.IsRated = false;
			this.IsWarranty = false;
			this.IsStolen = false;
			this.AddProductReference = false;
			this.phoneCompany = String.Empty;

		}
	}

	/// <summary>
	/// Clase para mapear la informacion de las redes sociales
	/// Desarrollador: David Martinez
	/// </summary>
	public class SocialMediaLinkSQL
	{
		public string Image { get; set; }
		public string URL { get; set; }

		public SocialMediaLinkSQL()
		{
			this.Image = String.Empty;
			this.URL = String.Empty;
		}
	}

	/// <summary>
	/// Clase para guardar la información de especificación técnica
	/// Desarrollador: David Martinez
	/// </summary>
	public class TechnicalSpecificationGroupSQL
	{
		public string Title { get; set; }
		public string TitleEnglish { get; set; }
		public int TechnicalSpecificationId { get; set; }

		public TechnicalSpecificationGroupSQL()
		{
			this.Title = String.Empty;
			this.TitleEnglish = String.Empty;
			this.TechnicalSpecificationId = 0;
		}
	}

	/// <summary>
	/// Clase para guardar la informacion de la especificación técnica del detalle
	/// Desarrollador: David Martinez
	/// </summary>
	public class TechnicalSpecificationSQL
	{
		public int TechnicalSpecificationId { get; set; }
		public string Subtitle { get; set; }
		public string SubtitleEnglish { get; set; }
		public string Description { get; set; }
		public string DescriptionEnglish { get; set; }
		public string Image { get; set; }

		public TechnicalSpecificationSQL()
		{
			this.TechnicalSpecificationId = 0;
			this.Subtitle = String.Empty;
			this.SubtitleEnglish = String.Empty;
			this.Description = String.Empty;
			this.DescriptionEnglish = String.Empty;
			this.Image = String.Empty;
		}
	}

	/// <summary>
	/// Clase que guardara la informacion de los links en referencia a las guias de uso y guias de instalación
	/// Desarrollador: David Martinez
	/// </summary>
	public class LinkSQL
	{
		public int IdLink { get; set; }
		public string Title { get; set; }
		public string Image { get; set; }
		public string URL { get; set; }
		public string Author { get; set; }
		public decimal Rate { get; set; }
		public bool IsRated { get; set; }
		public string RecommendedBy { get; set; }
		public string RecommendedByEnglish { get; set; }
		public int Type { get; set; }

		public LinkSQL()
		{
			this.Title = String.Empty;
			this.Image = String.Empty;
			this.IdLink = 0;
			this.URL = String.Empty;
			this.Author = String.Empty;
			this.Rate = 0;
			this.IsRated = false;
			this.RecommendedBy = String.Empty;
			this.RecommendedByEnglish = String.Empty;
			this.Type = 0;
		}
	}

	/// <summary>
	/// Clase que guarda la información de los productos relacionados
	/// Desarrollador: David Martinez
	/// </summary>
	public class RelatedProductSQL
	{
		public string Title { get; set; }
		public string Image { get; set; }
		public string URL { get; set; }

		public RelatedProductSQL()
		{
			this.Title = String.Empty;
			this.Image = String.Empty;
			this.URL = String.Empty;
		}
	}

	/// <summary>
	/// Clase que guardara la información de los centros de servicio
	/// Desarrollador: David Martinez
	/// </summary>
	public class ServiceCenterSQL
	{		
		public string Name { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string Lat { get; set; }
		public string Lon { get; set; }

		public ServiceCenterSQL()
		{
			this.Lat = String.Empty;
			this.Lon = String.Empty;
			this.Name = String.Empty;
			this.Address = String.Empty;
			this.Country = String.Empty;
			this.Phone = String.Empty;
			this.State = String.Empty;
			this.City = String.Empty;
			this.PostalCode = String.Empty;
		}
	}

	/// <summary>
	/// Clase que guardara la información de las preguntas frecuentes
	/// Desarrollador: David Martinez
	/// </summary>
	public class FAQsSQL
	{
		public string Question { get; set; }
		public string QuestionEnglish { get; set; }
		public string Response { get; set; }
		public string ResponseEnglish { get; set; }

		public FAQsSQL()
		{
			this.Question = String.Empty;
			this.QuestionEnglish = String.Empty;
			this.Response = String.Empty;
			this.ResponseEnglish = String.Empty;
		}
	}

	/// <summary>
	/// Clase para guardar la información de las garantias relacionadas a la familia existentes
	/// Desarrollador: David Martinez
	/// </summary>
	public class WarrantiesSQL
	{
		public int Id { get; set; }
		public string PdfUrl { get; set; }
		public string Country { get; set; }
		public int PeriodMonths { get; set; }

		public WarrantiesSQL()
		{
			this.Id = 0;
			this.PdfUrl = String.Empty;
			this.Country = String.Empty;
			this.PeriodMonths = 0;
		}
	}
	#endregion

	#region Backoffice
	/// <summary>
	/// Clase para consultar los archivos de imagenes y pdf's de las familias
	/// Desarrollador: David Martinez
	/// </summary>
	public class ArchivesFamiliesSQL
	{
		public string image { get; set; }
	}

	/// <summary>
	/// Modelo para mapear los datos de la especificacion tecnica general
	/// Desarrollador: David Martinez
	/// </summary>
	public class TecnicalSpecificationDataSQL
	{
		public int specificationTecnicalId { get; set; }
		public string title { get; set; }
		public string titleEnglish { get; set; }
	}

	/// <summary>
	/// Modelo para mapear los datos de la especificacion tecnica del detalle 
	/// Desarrollador: David Martinez
	/// </summary>
	public class TecnicalSpecificationDetailsDataSQL
	{
		public int especificationTecnicalDetailId { get; set; }
		public string subtitle { get; set; }
		public string subtitleEnglish { get; set; }
		public string description { get; set; }
		public string descriptionEnglish { get; set; }
		public string image { get; set; }
		public int specificationTecnicalId { get; set; }
	}

	/// <summary>
	/// Modelo que contendra la informacion de la especificacion tecnica desde la base de datos
	/// Desarrollador: David Martinez
	/// </summary>
	public class TecnicalSpecificationSQL
	{
		public List<TecnicalSpecificationDataSQL> tecnicalSpecificationData { get; set; }
		public List<TecnicalSpecificationDetailsDataSQL> tecnicalSpecificationDetails { get; set; }

		public TecnicalSpecificationSQL()
		{
			this.tecnicalSpecificationDetails = new List<TecnicalSpecificationDetailsDataSQL>();
			this.tecnicalSpecificationData = new List<TecnicalSpecificationDataSQL>();
		}
	}

	/// <summary>
	/// Modelo que contendra los combos para el modulo de familias
	/// Desarrollador: David Martinez
	/// </summary>
	public class FamilyListDropDownSQL
	{
		public List<TraceITListDropDown> companyData { get; set; }
		public List<TraceITListDropDown> categoryData { get; set; }
		public List<TraceITListDropDown> addressData { get; set; }
		public List<TraceITListDropDown> recommendedBy { get; set; }

		public FamilyListDropDownSQL()
		{
			this.addressData = new List<TraceITListDropDown>();
			this.categoryData = new List<TraceITListDropDown>();
			this.companyData = new List<TraceITListDropDown>();
			this.recommendedBy = new List<TraceITListDropDown>();
		}
	}

	/// <summary>
	/// Modelo que contendra los campos de embalajes de una familia
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class PackagingFamilySQL
	{
		public int packagingId { get; set; }
		public string packagingType { get; set; }
		public string readingType { get; set; }
		public int boxLabelType { get; set; }
		public int boxLabelPallet { get; set; }
		public int unitsPerBox { get; set; }
		public int copiesPerBox { get; set; }
		public int linesPerBox { get; set; }
		public decimal grossWeightPerBox { get; set; }
		public string dimensionsWeightPerBox { get; set; }
		public int boxesPerPallet { get; set; }
		public int copiesPerPallet { get; set; }
		public decimal grossWeightPerPallet { get; set; }
		public string dimensionsPerPallet { get; set; }
		public string instructionsWarnings { get; set; }
		public string getEmpacadoresId { get; set; }
		public int companyId	{ get; set; }
	}

	public class productoId
	{
		public int ProductoId { get; set; }
		public string UDID { get; set; }
		public string FechaCaducidad { get; set; }
		public int DireccionId { get; set; }
		public int FamiliaProductoId { get; set; }

	}

	public class familiaProducto
	{
		public int FamiliaProductoId { get; set; }
		public string ImagenUrl { get; set; }
		public string Nombre { get; set; }
		public string Modelo { get; set; }
		public string GTIN { get; set; }
		public string Descripcion { get; set; }
	}
}

	#endregion
