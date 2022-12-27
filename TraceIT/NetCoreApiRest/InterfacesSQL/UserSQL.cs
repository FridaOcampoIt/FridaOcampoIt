using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.User;
using NetCoreApiRest.Utils;

namespace WSTraceIT.InterfacesSQL
{
	public class UserSQL: DBHelperDapper
	{
		#region Propiedades
		public int idUser;
		public string name;
		public string lastName;
		public string email;
		public string password;
		public string position;
		public int companyId;
		public int rolId;
		public int profile;
		public int option;
		public string recoveryCode;
		public int type;
		public int idProvider;
		public int isOrigin;
		public List<int> acopiosIds;
		public List<int> auxAcopiosIds;
		private LoggerD4 log = new LoggerD4("UserSQL");
		#endregion

		#region Constructor
		public UserSQL()
		{
			this.idUser = 0;
			this.name = String.Empty;
			this.lastName = String.Empty;
			this.email = String.Empty;
			this.password = String.Empty;
			this.position = String.Empty;
			this.companyId = 0;
			this.rolId = 0;
			this.profile = 0;
			this.option = 0;
			this.recoveryCode = "";
			this.type = 0;
			this.idProvider = 0;
			this.isOrigin = 0;
		}
		#endregion

		#region Metodo Publicos
		/// <summary>
		/// Metodo para guardar un usuario del BO
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveUser()
		{
			log.trace("SaveUser");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_lastName", lastName, DbType.String);
				parameters.Add("_email", email, DbType.String);
				parameters.Add("_password", password, DbType.String);
				parameters.Add("_position", position, DbType.String);
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_rolId", rolId, DbType.Int32);
				parameters.Add("_profileId", profile, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarUsuarios", parameters);
				int response = parameters.Get<int>("_response");
				if(response > 0)
                {
					//Verificamos si tiene acopios asignados nuestro usuario
					if (acopiosIds != null)
					{
						foreach (int acopio in acopiosIds) //Recorremos todos los registros recibidos en la variable acopiosIDs
						{
							string query = $"INSERT INTO rel_053_usuarioacopio (FK_AcopioId, FK_UsuarioId) VALUES({acopio},{response});";
							ConsultaCommand<string>(query);
						}
					}
				}
				if (response == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (response == -1)
					throw new Exception("Ya existe un usuario con el mismo email");

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
		/// Metodo para ejecutar el sp para la edicion del usuario
		/// Desarrollador: David Martinez
		/// </summary>
		public void EditUser()
		{
			log.trace("EditUser");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_idUser", idUser, DbType.Int32);
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_lastName", lastName, DbType.String);
				parameters.Add("_email", email, DbType.String);
				parameters.Add("_password", password, DbType.String);
				parameters.Add("_position", position, DbType.String);
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_rolId", rolId, DbType.Int32);
				parameters.Add("_profileId", profile, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_edicionUsuarios", parameters);
				int response = parameters.Get<int>("_response");


				if (acopiosIds != null || auxAcopiosIds != null)
				{
                    if (auxAcopiosIds != null)
                    {
						//Nos regresa un array con los acopios que ya no se encuentren listadas en la petición
						var viejos = auxAcopiosIds.Except(acopiosIds != null ? acopiosIds : new List<int>());
						//Eliminamos los registros de la tabla pivote (Eliminados por usuario TraceIt)
						foreach (int viejo in viejos)
						{
							string query = $"DELETE FROM rel_053_usuarioacopio  WHERE FK_AcopioId = {viejo} and FK_UsuarioId = {idUser};";
							ConsultaCommand<string>(query);
						}
                    }
					if (acopiosIds != null)
					{
						//Nos regresa un array con los acopios nuevas que se agregaron en la petición
						var nuevos = acopiosIds.Except(auxAcopiosIds != null ? auxAcopiosIds : new List<int>());
						foreach (int nuevo in nuevos)
						{
							//Creamos los registros para las nuevas compañias que seran ligadas (Solo usuario TraceIt)
							string query = $"INSERT INTO rel_053_usuarioacopio (FK_AcopioId, FK_UsuarioId) VALUES({ nuevo },{idUser});";
							ConsultaCommand<string>(query);
						}
					}
				}

				if (response == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (response == -1)
					throw new Exception("Ya existe un usuario con el mismo email");

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
		/// Metodo para ejecutar el sp para la eliminación del usuario
		/// Desarrollador: David Martinez
		/// </summary>
		public void DeleteUser()
		{
			log.trace("DeleteUser");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_idUser", idUser, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarUsuarios", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (response == -1)
					throw new Exception("No se puede eliminar el usuario porque tiene seguimientos relacionados");
				else if (response == -2)
					throw new Exception("No se puede eliminar el usuario porque tiene reportes de robo relacionados");

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
		/// Metodo para ejecutar el sp para la consulta de los usuarios
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public List<DataUserBackOffice> searchUser()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_rolId", rolId, DbType.Int32);

				List<DataUserBackOffice> response = Consulta<DataUserBackOffice>("spc_consultaUsuarios", parameters);
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
		/// Metodo para ejecutar el sp para la consulta de los usuarios por compañía
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		public List<UserByCompanyId> searchUserByCompanyId()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companyId", companyId, DbType.Int32);

				List<UserByCompanyId> response = Consulta<UserByCompanyId>("spc_consultaUsuariosByCompanyId", parameters);
				CerrarConexion();

				return response;
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}
		/// <summary>
		/// Metodo para ejecutar el sp para la consulta de los datos del usuario
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public DataUserBackOfficeDatas searchUserData()
		{
			log.trace("searchUserData");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_idUser", idUser, DbType.Int32);

				DataUserBackOfficeDatas resp = Consulta<DataUserBackOfficeDatas>("spc_consultaUsuariosDatos", parameters).FirstOrDefault();
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
		/// Metodo para consultar los combos para el modulo de usuarios
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public UserDropDown searchUserDropDown()
		{
			log.trace("searchUserDropDown");
			UserDropDown response = new UserDropDown();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_company", companyId, DbType.Int32);
				parameters.Add("_user", idUser, DbType.Int32);
				parameters.Add("_option", option, DbType.Int32);

				if(option == 1 || option == 3)
				{
					List<Type> types = new List<Type>();
					types.Add(typeof(TraceITListDropDown));
					types.Add(typeof(TraceITListDropDown));
					types.Add(typeof(TraceITListDropDown));

					var responseSQL = ConsultaMultiple("spc_consultaCombosUsuarios", types, parameters);

					response.rols = responseSQL[0].Cast<TraceITListDropDown>().ToList();
					response.companies = responseSQL[1].Cast<TraceITListDropDown>().ToList();
					response.profiles = responseSQL[2].Cast<TraceITListDropDown>().ToList();
				}
				else
				{
					response.profiles = Consulta<TraceITListDropDown>("spc_consultaCombosUsuarios", parameters);
				}

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
		/// Metodo para realizar el metodo del login para BO
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public UserLoginData loginUser()
		{
			log.trace("loginUser");
			try
			{
				UserLoginData response = new UserLoginData();

				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(UserDataLogin));
				types.Add(typeof(UserPermissionLogin));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_email", email, DbType.String);
				parameters.Add("_password", password, DbType.String);
				parameters.Add("_provider", idProvider, DbType.Int32);
				parameters.Add("_isOrigin", isOrigin, DbType.Int32);

				var responseSQL = ConsultaMultiple("spc_loginWeb", types, parameters);

				response.userData = responseSQL[0].Cast<UserDataLogin>().FirstOrDefault();
				response.userPermissions = responseSQL[1].Cast<UserPermissionLogin>().ToList();

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
		/// Metodo para guardar y actualizar el código de recuperación
		/// Desarrollador: David Martinez
		/// </summary>
		public void saveCodeRecovery()
		{
			log.trace("saveCodeRecovery");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_email", email, DbType.String);
				parameters.Add("_recoveryCode", recoveryCode, DbType.String);
				parameters.Add("_password", password, DbType.String);
				parameters.Add("_option", type, DbType.Int32);
				parameters.Add("_userType", 2, DbType.Int32);//for web user
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
				parameters = EjecutarSPOutPut("spu_edicionCodigoRecuperacion", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Ocurrio un error ejecutar el sp");
				else if (response == -1)
					throw new Exception("Usuario inexistente");
				else if (response == -2)
					throw new Exception("Código de recuperación incorrecto");

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

		#endregion
	}
}
