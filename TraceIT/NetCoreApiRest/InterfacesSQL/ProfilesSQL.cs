using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Profiles;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;

namespace WSTraceIT.InterfacesSQL
{
	public class ProfilesSQL: DBHelperDapper
	{
		#region Propiedades
		public string name;
		public int company;
		public int profileId;

		public List<int> permission;
		private LoggerD4 log = new LoggerD4("ProfilesSQL");

		#endregion

		#region Constructor
		public ProfilesSQL()
		{
			this.name = String.Empty;
			this.company = 0;
			this.profileId = 0;
			this.permission = new List<int>();
		}
		#endregion

		#region Metodos publicos
		/// <summary>
		/// Metodo para guardar el perfil
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveProfile()
		{
			log.trace("SaveProfile");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_companyId", company, DbType.Int32);
				parameters.Add("_option", 1, DbType.Int32);
				parameters.Add("_permissionId", 0, DbType.Int32);
				parameters.Add("_profileId", profileId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarPerfiles", parameters);
				profileId = parameters.Get<int>("_response");

				if (profileId == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (profileId == -1)
					throw new Exception("Ya existe un perfil con el mismo nombre");

				foreach (int per in permission)
				{
					parameters = new DynamicParameters();
					parameters.Add("_name", name, DbType.String);
					parameters.Add("_companyId", company, DbType.Int32);
					parameters.Add("_option", 2, DbType.Int32);
					parameters.Add("_permissionId", per, DbType.Int32);
					parameters.Add("_profileId", profileId, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_guardarPerfiles", parameters);
					int response = parameters.Get<int>("_response");

					if (response == 0)
						throw new Exception("Error al ejecutar el SP");
					else if (response == -2)
						throw new Exception("Algunos permisos no se le pueden asignar a un perfil de compañia");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
TransRollback();
				CerrarConexion();
				throw ex;
			}
		} 

		/// <summary>
		/// Metodo para editar el perfil
		/// Desarrollador: David Martinez
		/// </summary>
		public void EditProfile()
		{
			log.trace("EditProfile");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_profileId", profileId, DbType.Int32);
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_companyId", company, DbType.Int32);				
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_edicionPerfiles", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (response == -1)
					throw new Exception("Ya existe un perfil con el mismo nombre");

				foreach (int per in permission)
				{
					parameters = new DynamicParameters();
					parameters.Add("_name", name, DbType.String);
					parameters.Add("_companyId", company, DbType.Int32);
					parameters.Add("_option", 2, DbType.Int32);
					parameters.Add("_permissionId", per, DbType.Int32);
					parameters.Add("_profileId", profileId, DbType.Int32);
					parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

					parameters = EjecutarSPOutPut("spi_guardarPerfiles", parameters);
					response = parameters.Get<int>("_response");

					if (response == 0)
						throw new Exception("Error al ejecutar el SP");
					else if (response == -2)
						throw new Exception("Algunos permisos no se le pueden asignar a un perfil de compañia");
				}

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para eliminar el perfil
		/// Desarrollador: David Martinez
		/// </summary>
		public void DeleteProfile()
		{
			log.trace("DeleteProfile");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_profileId", profileId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarPerfiles", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (response == -1)
					throw new Exception("Existen usuarios asignados al perfil");

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar los perfiles
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public List<Profiles> SearchProfiles()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_companyId", company, DbType.Int32);

				List<Profiles> resp = Consulta<Profiles>("spc_consultaPerfiles", parameters);
				CerrarConexion();

				return resp;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar los permisos del sistema y los combos que se necesitaran
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public DropDownProfilesPermissionSQL SearchDropDownPermission()
		{
			log.trace("SearchDropDownPermission");
			DropDownProfilesPermissionSQL response = new DropDownProfilesPermissionSQL();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(PermissionSQL));
				types.Add(typeof(TraceITListDropDown));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companyId", company, DbType.Int32);

				var respSQL = ConsultaMultiple("spc_consultaCombosPerfiles", types, parameters);				

				response.permissions = respSQL[0].Cast<PermissionSQL>().ToList();
				response.companyList = respSQL[1].Cast<TraceITListDropDown>().ToList();
				CerrarConexion();

				return response;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para devolver los datos de los perfiles para su edicion
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public PermissionProfileData SearchProfileData()
		{
			log.trace("SearchProfileData");
			PermissionProfileData response = new PermissionProfileData();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_profileId", profileId, DbType.Int32);

				List<Type> types = new List<Type>();
				types.Add(typeof(ProfilesData));
				types.Add(typeof(PermissionData));

				var respSQL = ConsultaMultiple("spc_consultaPerfilesDatos", types, parameters);

				response.profileData = respSQL[0].Cast<ProfilesData>().FirstOrDefault();
				response.permissions = respSQL[1].Cast<PermissionData>().ToList();
				CerrarConexion();

				return response;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
CerrarConexion();
				throw ex;
			}
		}
		#endregion
	}
}
