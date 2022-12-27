using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.Base.User
{
	/// <summary>
	/// Modelo para mapear los datos de los usuarios
	/// Desarrollador: David Martinez
	/// </summary>
	public class DataUserBackOffice
	{
		public string name { get; set; }
		public string email { get; set; }
		public string profile { get; set; }
		public string rol { get; set; }
		public string company { get; set; }
		public int idUser { get; set; }

		public DataUserBackOffice()
		{
			this.company = String.Empty;
			this.email = String.Empty;
			this.idUser = 0;
			this.name = String.Empty;
			this.profile = String.Empty;
			this.rol = String.Empty;
			
		}
	}


	public class UserByCompanyId 
	{
		public int id { get; set; }
		public string nombre { get; set; }
		public string apellido { get; set; }
		public string email { get; set; }
		public int companyId { get; set; }
		public UserByCompanyId()
		{
			this.id = 0;
			this.nombre = String.Empty;
			this.apellido = String.Empty;
			this.email = String.Empty;
			this.companyId = 0;
		}

	}

	/// <summary>
	/// Modelo para mapear los datos de los usuarios para su edición
	/// Desarrollador: David Martinez
	/// </summary>
	public class DataUserBackOfficeDatas
	{
		public int idUser { get; set; }
		public string name { get; set; }
		public string lastName { get; set; }
		public string email { get; set; }
		public string position { get; set; }
		public int company { get; set; }
		public int rol { get; set; }
		public int profile { get; set; }

		public string acopiosIds		{ get; set; }

		public DataUserBackOfficeDatas()
		{
			this.company = 0;
			this.email = String.Empty;
			this.idUser = 0;
			this.lastName = String.Empty;
			this.name = String.Empty;
			this.position = String.Empty;
			this.profile = 0;
			this.rol = 0;
			this.acopiosIds = String.Empty;
		}
	}

	/// <summary>
	/// Clase para mapear los combos para el modulo de usuarios
	/// Desarrollador: David Martinez
	/// </summary>
	public class UserDropDown
	{
		public List<TraceITListDropDown> rols { get; set; }
		public List<TraceITListDropDown> companies { get; set; }
		public List<TraceITListDropDown> profiles { get; set; }

		public UserDropDown()
		{
			this.companies = new List<TraceITListDropDown>();
			this.profiles = new List<TraceITListDropDown>();
			this.rols = new List<TraceITListDropDown>();
		}
	}

	/// <summary>
	/// Clase para mapear los datos del usuario para el login
	/// Desarrollador: David Martinez
	/// </summary>
	public class UserDataLogin
	{
		public int idUser { get; set; }
		public string name { get; set; }
		public string companyName { get; set; }
		public int company { get; set; }
		public int isType { get; set; }
		public int origen { get; set; }
		public int Merma { get; set; }
		public int eUser { get; set; }
		public string acopiosIds { get; set; }

		public UserDataLogin()
		{
			this.name = String.Empty;
			this.companyName = String.Empty;
			this.idUser = 0;
			this.company = 0;
			this.isType = 0;
			this.origen = 0;
			this.Merma = 0;
			this.eUser = 0;
		}
	}

	/// <summary>
	/// Clase para mapear todos los datos del usuario para el login
	/// Desarrollador: David Martinez
	/// </summary>
	public class UserLoginData
	{
		public UserDataLogin userData { get; set; }
		public List<UserPermissionLogin> userPermissions { get; set; }

		public UserLoginData()
		{
			this.userPermissions = new List<UserPermissionLogin>();
			this.userData = new UserDataLogin();
		}
	}

	/// <summary>
	/// Clase para mapear los permisos del usuario para el login
	/// Desarrollador: David Martinez
	/// </summary>
	public class UserPermissionLogin
	{
		public int permissionId { get; set; }
		public string namePermission { get; set; }

		public UserPermissionLogin()
		{
			this.permissionId = 0;
			this.namePermission = String.Empty;
		}
	}
}
