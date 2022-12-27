using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.ModelsSQL
{
	/// <summary>
	/// Clase para mapear los datos de las configuraciones generales desde el SP
	/// Desarrollador: David Martinez
	/// </summary>
	public class GeneralConfigurationDataSQL
	{
		public string configuration { get; set; }
		public string value { get; set; }
	}

	/// <summary>
	/// Clase para mapear la consulta de los datos generales
	/// Desarrollador: David Martinez
	/// </summary>
	public class ConfigurationDropDownSQL
	{
		public List<TraceITListDropDown> users { get; set; }
		public List<TraceITListDropDown> companies { get; set; }
		public List<GeneralConfigurationDataSQL> configurationGenerals { get; set; }

		public ConfigurationDropDownSQL()
		{
			this.companies = new List<TraceITListDropDown>();
			this.users = new List<TraceITListDropDown>();
			this.configurationGenerals = new List<GeneralConfigurationDataSQL>();
		}
	}
}
