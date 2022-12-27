using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.Base.Profiles
{
	/// <summary>
	/// Modelo para consultar los perfiles
	/// Desarrollador: David Martinez
	/// </summary>
	public class Profiles
	{
		public int profileId { get; set; }
		public string name { get; set; }
		public string company { get; set; }
	}

	/// <summary>
	/// Modelo para consultar los datos de un perfil
	/// Desarrollador: David Martinez
	/// </summary>
	public class ProfilesData
	{
		public int profileId { get; set; }
		public string name { get; set; }
		public int company { get; set; }

		public ProfilesData()
		{
			this.profileId = 0;
			this.name = String.Empty;
			this.company = 0;
		}
	}	

	/// <summary>
	/// Modelo para traerse la consulta de los datos de un perfil para su edición
	/// Desarrollador: David Martinez
	/// </summary>
	public class PermissionProfileData
	{
		public ProfilesData profileData { get; set; }
		public List<PermissionData> permissions { get; set; }

		public PermissionProfileData()
		{
			this.permissions = new List<PermissionData>();
			this.profileData = new ProfilesData();
		}
	}

	/// <summary>
	/// Modelo para mapear los permisos que contiene un perfil
	/// Dosarrollador: David Martinez
	/// </summary>
	public class PermissionData
	{
		public int permission { get; set; }
	}

	/// <summary>
	/// Clase para mapear los permisos del sistema
	/// Desarrollador: David Martinez
	/// </summary>
	public class DropDownProfilesPermission
	{
		public List<PermissionEstructure> permissions { get; set; }
		public List<TraceITListDropDown> companyList { get; set; }

		public DropDownProfilesPermission()
		{
			this.companyList = new List<TraceITListDropDown>();
			this.permissions = new List<PermissionEstructure>();
		}
	}

	/// <summary>
	/// Clase para consultar la estructura de los permisos padres e hijos
	/// Desarrollador: David Martinez
	/// </summary>
	public class PermissionEstructure
	{
		public int idPermission { get; set; }
		public string name { get; set; }
		public List<PermissionChildren> permission { get; set; }

		public PermissionEstructure()
		{
			this.idPermission = 0;
			this.name = String.Empty;
			this.permission = new List<PermissionChildren>();
		}
	}

	/// <summary>
	/// Clase para traerse los permisos hijos
	/// Desarrollador: David Martinez
	/// </summary>
	public class PermissionChildren
	{
		public int idPermission { get; set; }
		public string name { get; set; }
		public bool check { get; set; }
		public bool isForCompany { get; set; }

		public PermissionChildren()
		{
			this.name = String.Empty;
			this.idPermission = 0;
			this.check = false;
			this.isForCompany = false;
		}
	}
}
