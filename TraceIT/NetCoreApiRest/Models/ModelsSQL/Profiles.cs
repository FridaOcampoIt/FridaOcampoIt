using System.Collections.Generic;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.ModelsSQL
{
	/// <summary>
	/// Modelo para traerse los permisos del sistema
	/// Desarrollador: David Martinez
	/// </summary>
	public class PermissionSQL
	{
		public int idPermission { get; set; }
		public string name { get; set; }
		public bool isFather { get; set; }
		public int fatherPermissionId { get; set; }
		public bool forUserCompany { get; set; }
	}

	/// <summary>
	/// Modelo para mapear los combos que se utilizaran en el modulo
	/// Desarrollador: David Martinez
	/// </summary>
	public class DropDownProfilesPermissionSQL
	{
		public List<PermissionSQL> permissions { get; set; }
		public List<TraceITListDropDown> companyList { get; set; }

		public DropDownProfilesPermissionSQL()
		{
			this.permissions = new List<PermissionSQL>();
			this.companyList = new List<TraceITListDropDown>();
		}
	}
}
