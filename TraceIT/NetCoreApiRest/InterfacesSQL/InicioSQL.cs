using DWUtils;
using NetCoreApiRest.Models.ModelsSQL;
using System;
using System.Collections.Generic;
using NetCoreApiRest.Utils;

namespace NetCoreApiRest.InterfacesSQL
{
	public class InicioSQL : DBHelperDapper
	{
		#region Properties
		private LoggerD4 log = new LoggerD4("InicioSQL");
		#endregion

		#region Constructor
		public InicioSQL()
		{

		}
		#endregion

		#region Public methods
		/// <summary>
		/// Metodo para consultar los permisos del sistema desde base de datos
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public List<Permission> GetPermissionSystem()
		{
			List<Permission> permissions = new List<Permission>();
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				log.debug("Getting the permissions list");
				permissions = Consulta<Permission>("spc_permisos");
				/*foreach(var perm in permissions)
					log.trace("Got id:__" + perm.id + "__, name: __" + perm.name + "__");*/
				CerrarConexion();

				return permissions;
			}
			catch (Exception ex)
			{	
				log.error("Exception: " + ex.Message);	
				CerrarConexion();
				throw ex;
			}
		}
		#endregion
	}
}
