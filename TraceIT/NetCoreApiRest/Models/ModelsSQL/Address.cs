using System.Collections.Generic;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.ModelsSQL
{
	/// <summary>
	/// Modelo utilizado para regresar los datos de los combos de Direcciones
	/// </summary>
	public class AddressListDropDownSQL
	{
		public List<TraceITListDropDown> companyData { get; set; }
		public List<TraceITListDropDown> addressTypeData { get; set; }
		public List<TraceITListDropDown> familyData { get; set; }

		public AddressListDropDownSQL()
		{
			this.addressTypeData = new List<TraceITListDropDown>();
			this.companyData = new List<TraceITListDropDown>();
			this.familyData = new List<TraceITListDropDown>();
		}
	}
}
