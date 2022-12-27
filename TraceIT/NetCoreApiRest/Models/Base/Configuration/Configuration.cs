using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.Base.Configuration
{
	/// <summary>
	/// Clase para mapear los datos de los DropDown para el modulo
	/// Desarrollador: David Martinez
	/// </summary>
	public class GeneralConfigurationDropDown
	{
		public List<TraceITListDropDown> userTraceIT { get; set; }
		public List<TraceITListDropDown> companyTraceIT { get; set; }
		public List<GeneralConfigurationData> generalConfigurations { get; set; }

		public GeneralConfigurationDropDown()
		{
			this.generalConfigurations = new List<GeneralConfigurationData>();
			this.userTraceIT = new List<TraceITListDropDown>();
			this.companyTraceIT = new List<TraceITListDropDown>();
		}
	}

	/// <summary>
	///	Clase para mapear las configuraciones generales
	///	Desarrollador: David Martinez
	/// </summary>
	public class GeneralConfigurationData
	{
		public string configuration { get; set; }
		public List<string> value { get; set; }

		public GeneralConfigurationData()
		{
			this.configuration = String.Empty;
			this.value = new List<string>();
		}
	}

	/// <summary>
	/// Clase para mapear las configuraciones de las compañias
	/// Desarrollador: David Martinez
	/// </summary>
	public class CompanyConfigurationData
	{
		public int nUseGuides { get; set; }
		public int nInstalationGuides { get; set; }
		public int nRelatedProduct { get; set; }
		public string notifyComments { get; set; }
		public string notifyWarranty { get; set; }
		public string notifyStolen { get; set; }
		public int nPDF {get; set; }
		public int nImg { get; set; }
		public int nCharEspec { get; set; }
		public int nCharFAQ { get; set; }
		public int nVid { get; set; }
	}

	/// <summary>
	/// Clase para mapear los datos de las configuraciones generales
	/// Desarrollador: David Martinez
	/// </summary>
	public class SaveGeneralConfiguration
	{
		public string configuration { get; set; }
		public string value { get; set; }
	}
}
