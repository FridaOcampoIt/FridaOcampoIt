using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.Base.Families
{
	#region Mobile application
	/// <summary>
	/// Clase para mapear el origen del producto
	/// Desarrollador: David Martinez
	/// </summary>
	public class Origin
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

		public Origin()
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
	/// Clase para mapear el listado de la especificacion tecnica de la familia
	/// Desarrollador: David Martinez
	/// </summary>
	public class TechnicalSpecificationGroup
	{
		public string Title { get; set; }
		public string TitleEnglish { get; set; }
		public List<TechnicalSpecification> TechnicalSpecifications { get; set; }

		public TechnicalSpecificationGroup()
		{
			this.TechnicalSpecifications = new List<TechnicalSpecification>();
			this.Title = String.Empty;
			this.TitleEnglish = String.Empty;
		}
	}

	/// <summary>
	/// Clase para mapear la especificacion tecnica de la familia
	/// Desarrollador: David Martinez
	/// </summary>
	public class TechnicalSpecification
	{
		public string Name { get; set; }
		public string NameEnglish { get; set; }
		public string Description { get; set; }
		public string DescriptionEnglish { get; set; }
		public string Image { get; set; }

		public TechnicalSpecification()
		{
			this.Description = String.Empty;
			this.Name = String.Empty;
			this.Image = String.Empty;
		}
	}

	/// <summary>
	/// Clase para mapear los datos del link
	/// Desarrollador: David Martinez
	/// </summary>
	public class Link
	{
		public int IdLink { get; set; }
		public string Title { get; set; }
		public string Image { get; set; }
		public string URL { get; set; }
		public string Author { get; set; }
		public float Rate { get; set; }
		public bool IsRated { get; set; }
		public string RecommendedBy { get; set; }
		public string RecommendedByEnglish { get; set; }
		public int Type { get; set; }

		public Link()
		{
			this.Author = String.Empty;
			this.Image = String.Empty;
			this.Rate = 0;
			this.IsRated = false;
			this.RecommendedBy = String.Empty;
			this.RecommendedByEnglish = String.Empty;
			this.Title = String.Empty;
			this.Type = 0;
			this.URL = String.Empty;
		}
	}

	/// <summary>
	/// Clase para mapear los datos de los centros de servicio de una familia
	/// Desarrollador: David Martinez
	/// </summary>
	public class ServiceCenter
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Country { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public double Lat { get; set; }
		public double Lon { get; set; }

		public ServiceCenter()
		{
			this.Address = String.Empty;
			this.City = String.Empty;
			this.Country = String.Empty;
			this.Name = String.Empty;
			this.Phone = String.Empty;
			this.State = String.Empty;
			this.PostalCode = String.Empty;
			this.Lat = 0;
			this.Lon = 0;
		}
	}

	/// <summary>
	/// Clase para mapear los datos de las preguntas frecuentes
	/// Desarrollador: David Martinez
	/// </summary>
	public class FAQ
	{
		public string Question { get; set; }
		public string QuestionEnglish { get; set; }
		public string Response { get; set; }
		public string ResponseEnglish { get; set; }

		public FAQ()
		{
			this.Question = String.Empty;
			this.Response = String.Empty;
		}
	}

	/// <summary>
	/// Clase para las garantias existentes de una familia
	/// </summary>
	public class Warranty
	{
		public int Id { get; set; }
		public string PdfUrl { get; set; }
		public string Country { get; set; }
		public int PeriodMonths { get; set; }

		public Warranty()
		{
			this.Country = String.Empty;
			this.Id = 0;
			this.PdfUrl = String.Empty;
			this.PeriodMonths = 0;
		}
	}

	/// <summary>
	/// Clase para las redes sociales que tiene una compañia
	/// Desarrollador: David Martinez
	/// </summary>
	public class SocialMediaLinks
	{
		public string Image { get; set; }
		public string URL { get; set; }

		public SocialMediaLinks()
		{
			this.Image = String.Empty;
			this.URL = String.Empty;
		}
	}
	#endregion

	#region Backoffice

	#region Save Family

	/// <summary>
	/// Clase que contendra todos los datos necesarios de la familia
	/// Desarrollador: David Martinez
	/// </summary>
	public class StatusProd
	{
		public int Idprod { get; set; }
		public int st_product { get; set; }
		public int st_familia { get; set; }
		public int st_company { get; set; }

	}


	/// <summary>
	/// Clase que contendra todos los datos necesarios de la familia
	/// Desarrollador: David Martinez
	/// </summary>
	public class FamilyData
	{
		public int familyId { get; set; }
		public string name { get; set; }
		public string model { get; set; }
		public string description { get; set; }
		public string descriptionEnglish { get; set; }
		public string imageBaseFamily { get; set; }
		public string sku { get; set; }
		public string gtin { get; set; }
		public bool status { get; set; }
		public bool warranty { get; set; }
		public bool expiration { get; set; }
		public bool addTicket { get; set; }
		public int category { get; set; }
		public int company { get; set; }
		public int lifeDays { get; set; }
		public bool autoLote { get; set; }
		public bool editLote { get; set; }
		public int consecutiveLote { get; set; }
		public string prefix { get; set; }

		public string colorFamilia	{ get; set; }
		public FamilyData()
        {
			this.autoLote = false;
			this.editLote = false;
			this.consecutiveLote = 0;
			this.prefix = String.Empty;
        }
	}

	/// <summary>
	/// Clase que contendra los datos de las garantias
	/// Desarrollador: David Martinez
	/// </summary>
	public class WarrantyData
	{
		public int warrantyId { get; set; }
		public string country { get; set; }
		public string pdfBase { get; set; }
		public int periodMonth { get; set; }
	}

	/// <summary>
	/// Clase que contendra las preguntas frecuentes
	/// Desarrollador: David Martinez
	/// </summary>
	public class FrequentQuestions
	{
		public int questionId { get; set; }
		public string question { get; set; }
		public string questionEnglish { get; set; }
		public string response { get; set; }
		public string responseEnglish { get; set; }
	}

	/// <summary>
	/// Clase que contendra la especificacion tecnica de la familia
	/// Desarrollador: David Martinez
	/// </summary>
	public class TechnicalSpecificationData
	{
		public int specificationTechnicalId { get; set; }
		public string title { get; set; }
		public string titleEnglish { get; set; }
		public List<TechnicalSpecificationDetailsData> technicalEspecificationDetails { get; set; }
	}

	/// <summary>
	/// Clase que contendra el detalle de la especificación tecnica
	/// Desarrollador: David Martinez
	/// </summary>
	public class TechnicalSpecificationDetailsData
	{
		public int specificationTechnicalDetailId { get; set; }
		public string subtitle { get; set; }
		public string subtitleEnglish { get; set; }
		public string description { get; set; }
		public string descriptionEnglish { get; set; }
		public string imageBase { get; set; }
	}

	/// <summary>
	/// Clase que contendra las guias de uso e instalacion y productos relacionados
	/// Desarrollador: David Martinez
	/// </summary>
	public class linkData
	{
		public int linkId { get; set; }
		public string title { get; set; }
		public string url { get; set; }
		public string thumbailUrl { get; set; }
		public string author { get; set; }
		public bool status { get; set; }
		public int linkTypeId { get; set; }
	}
		
	#endregion

	#region Archives

	/// <summary>
	/// Modelo utilizado para ver la base 64 y el nombre con la que se guardaran los archivos
	/// Desarrollador: David Martinez
	/// </summary>
	public class ArchivesData
	{
		public string base64 { get; set; }
		public string archiveName { get; set; }
		public string archiveDeleteName { get; set; }
        public string tmbName { get; set; }
		public ArchivesData()
		{
			this.archiveDeleteName = String.Empty;
			this.archiveName = String.Empty;
            this.tmbName = String.Empty;
			this.base64 = String.Empty;
		}
	}

	#endregion

	#region Search Family
	public class ProductFamily
	{
		public int familyProductId { get; set; }
		public string name { get; set; }
		public string model { get; set; }
		public string gtin { get; set; }
		public string sku { get; set; }
		public int companyId	{ get; set; }
	}

	/// <summary>
	/// Modelo para mapear los datos de la familia
	/// Desarrollador: David Martinez
	/// </summary>
	public class ProductFamilyData
	{
		public int familyProductId { get; set; }
		public string name { get; set; }
		public string model { get; set; }
		public string description { get; set; }
		public string descriptionEnglish { get; set; }
		public string image { get; set; }
		public string sku { get; set; }
		public string gtin { get; set; }
		public bool status { get; set; }
		public bool warranty { get; set; }
		public bool expiration { get; set; }
		public bool addTicket { get; set; }
		public int category { get; set; }
		public int company { get; set; }
		public int lifeDays { get; set; }
		public bool autoLote { get; set; }
		public bool editLote { get; set; }
		public int consecutiveLote { get; set; }
		public string prefix { get; set; }

		public string colorFamilia	{ get; set; }

		public ProductFamilyData()
		{
			this.addTicket = false;
			this.category = 0;
			this.company = 0;
			this.description = String.Empty;
			this.descriptionEnglish = String.Empty;
			this.expiration = false;
			this.familyProductId = 0;
			this.gtin = String.Empty;
			this.image = String.Empty;
			this.model = String.Empty;
			this.name = String.Empty;
			this.sku = String.Empty;
			this.status = false;
			this.warranty = false;
			this.lifeDays = 0;
			this.autoLote = false;
			this.editLote = false;
			this.consecutiveLote = 0;
			this.colorFamilia = String.Empty;
			this.prefix = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para enlistar los limites de la familia
	/// Desarrollador: Daniel Rodriguez
	/// </summary>
	public class LimitsFamilyData
	{
		public int NPDF { get; set; }
		public int NCharEspec { get; set; }
		public int NCharFAQ { get; set; }
		public int NImg { get; set; }
		public int NVid { get; set; }
		public int NProductosRelacionados { get; set; }

		public LimitsFamilyData() {
			this.NPDF = 0;
			this.NCharEspec = 0;
			this.NCharFAQ = 0;
			this.NImg = 0;
			this.NVid = 0;
			this.NProductosRelacionados = 0;
		}
	}

	/// <summary>
	/// Modelo generico para obtener la cantidad en uso de la familia para comparar contra el limite
	/// Desarrollador: Daniel Rodriguez
	/// </summary>
	public class LimitUsedFamily
	{
		public int totalUsed { get; set; }

		public LimitUsedFamily()
		{
			this.totalUsed = 0;
		}
	}

	/// <summary>
	/// Modelo para enlistar las direcciones relacionada con la familia
	/// Desarrollador: David Martinez
	/// </summary>
	public class DirectionFamilyData
	{
		public int directionId { get; set; }
	}

	/// <summary>
	/// Modelo para tener la estructura de los datos al editar una familia
	/// Desarrollador: David Martinez
	/// </summary>
	public class FamilyDataEdition
	{
		public ProductFamilyData productFamilyData { get; set; }
		public List<DirectionFamilyData> directionFamily { get; set; }
		public LimitsFamilyData limitsFamily { get; set; }
		public LimitUsedFamily specCharUse { get; set; }
		public LimitUsedFamily imageUse { get; set; }

		public FamilyDataEdition()
		{
			this.directionFamily = new List<DirectionFamilyData>();
			this.productFamilyData = new ProductFamilyData();
			this.limitsFamily = new LimitsFamilyData();
			this.specCharUse = new LimitUsedFamily();
			this.imageUse = new LimitUsedFamily();
		}
	}

	/// <summary>
	/// Modelo para mapear la especificacion tecnica
	/// Desarrollador: David Martinez
	/// </summary>
	public class TechnicalSpecificationFamilyData
	{
		public int specificationTecnicalId { get; set; }
		public string title { get; set; }
		public string titleEnglish { get; set; }
		public List<TechnicalSpecificationDetailsFamilyData> technicalSpecificationDetails { get; set; }

		public TechnicalSpecificationFamilyData()
		{
			this.specificationTecnicalId = 0;
			this.technicalSpecificationDetails = new List<TechnicalSpecificationDetailsFamilyData>();
			this.title = String.Empty;
			this.titleEnglish = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para mapear la especificacion tecnica detalle
	/// Desarrollador: David Martinez
	/// </summary>
	public class TechnicalSpecificationDetailsFamilyData
	{
		public int specificationTechnicalDetailId { get; set; }
		public string subtitle { get; set; }
		public string subtitleEnglish { get; set; }
		public string description { get; set; }
		public string descriptionEnglish { get; set; }
		public string image { get; set; }

		public TechnicalSpecificationDetailsFamilyData()
		{
			this.description = String.Empty;
			this.descriptionEnglish = String.Empty;
			this.specificationTechnicalDetailId = 0;
			this.image = String.Empty;
			this.subtitle = String.Empty;
			this.subtitleEnglish = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para mapear los datos de los links de las familias
	/// Desarrollador: David Martinez
	/// </summary>
	public class LinkFamilyData
	{
		public int linkId { get; set; }
		public string title { get; set; }
		public string url { get; set; }
		public string thumbailUrl { get; set; }
		public string author { get; set; }
		public bool status { get; set; }
		public int recommendedById { get; set; }
		public int linkTypeId { get; set; }
		public string recommendedBy { get; set; }

		public LinkFamilyData()
		{
			this.author = String.Empty;
			this.linkId = 0;
			this.linkTypeId = 0;
			this.recommendedById = 0;
			this.status = false;
			this.thumbailUrl = String.Empty;
			this.title = String.Empty;
			this.url = String.Empty;
			this.recommendedBy = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para mapear los datos de las garantias y preguntas frecuentes de la familia
	/// Desarrollador: David Martinez
	/// </summary>
	public class LinksData { 
		public List<LinkFamilyData> linkData { get; set; }
		public LimitsFamilyData limitsFamily { get; set; }

		public LinksData()
		{
			this.linkData = new List<LinkFamilyData>();
			this.limitsFamily = new LimitsFamilyData();
		}
	}

	/// <summary>
	/// Modelo para mapear los datos de las garantias y preguntas frecuentes de la familia
	/// Desarrollador: David Martinez
	/// </summary>
	public class WarrantiesFaqFamily
	{
		public List<WarrantiesFamilyData> warranties { get; set; }
		public List<FrequentQuestionsFamilyData> frequentQuestions { get; set; }
		public LimitsFamilyData limitsFamily { get; set; }

		public WarrantiesFaqFamily()
		{
			this.frequentQuestions = new List<FrequentQuestionsFamilyData>();
			this.warranties = new List<WarrantiesFamilyData>();
			this.limitsFamily = new LimitsFamilyData();
		}
	}

	/// <summary>
	/// Modelo para mapear los datos de la garantia
	/// Desarrollador: David Martinez
	/// </summary>
	public class WarrantiesFamilyData
	{
		public int warrantyId { get; set; }
		public string country { get; set; }
		public string urlPdf { get; set; }
		public int periodMonths { get; set; }

		public WarrantiesFamilyData()
		{
			this.country = String.Empty;
			this.periodMonths = 0;
			this.urlPdf = String.Empty;
			this.warrantyId = 0;
		}
	}


	/// <summary>
	/// Modelo para mapear las preguntas frecuentes
	/// Desarrollador: David Martinez
	/// </summary>
	public class FrequentQuestionsFamilyData
	{
		public int faqId { get; set; }
		public string questionSpanish { get; set; }
		public string responseSpanish { get; set; }
		public string questionEnglish { get; set; }
		public string responseEnglish { get; set; }

		public FrequentQuestionsFamilyData()
		{
			this.faqId = 0;
			this.questionEnglish = String.Empty;
			this.questionSpanish = String.Empty;
			this.responseEnglish = String.Empty;
			this.responseSpanish = String.Empty;
		}
	}

	#endregion

	#region Search Youtube

	/// <summary>
	/// Modelo para mapear los datos que se obtienen del consumo de la api de youtube
	/// Desarrollador: David Martinez
	/// </summary>
	public class YoutubeData
	{
		public string title { get; set; }
		public string videoId { get; set; }
		public string thumbnails { get; set; }
		public string channelTitle { get; set; }
		public string recommendedBy { get; set; }

		public YoutubeData()
		{
			this.channelTitle = String.Empty;
			this.thumbnails = String.Empty;
			this.title = String.Empty;
			this.videoId = String.Empty;
			this.recommendedBy = String.Empty;
		}
	}
	#endregion

	#region Search Packaging Family

	/// <summary>
	/// Modelo para mapear los datos obtenidos de embalajes de una familia
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class PackagingFamily
	{
		public int packagingId { get; set; }
		public string packagingType { get; set; }
		public string readingType { get; set; }
		public int boxLabelType { get; set; }
		public int boxLabelPallet { get; set; }
		public int unitsPerBox { get; set; }
		public int copiesPerBox { get; set; }
		public decimal grossWeightPerBox { get; set; }
		public decimal dimensionsWeightPerBox { get; set; }
		public int boxesPerPallet { get; set; }
		public int copiesPerPallet { get; set; }
		public decimal grossWeightPerPallet { get; set; }
		public decimal dimensionsPerPallet { get; set; }
		public string instructionsWarnings { get; set; }

        #region Constructor
        public PackagingFamily()
        {
			this.packagingId = 0;
			this.packagingType = String.Empty;
			this.readingType = String.Empty;
			this.boxLabelType = 0;
			this.boxLabelPallet = 0;
			this.unitsPerBox = 0;
			this.copiesPerBox = 0;
			this.grossWeightPerBox = 0;
			this.dimensionsWeightPerBox = 0;
			this.boxesPerPallet = 0;
			this.copiesPerPallet = 0;
			this.grossWeightPerPallet = 0;
			this.dimensionsPerPallet = 0;
			this.instructionsWarnings = String.Empty;
		}
        #endregion
    }

    #endregion

#endregion
}
