using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;

namespace WSTraceIT.InterfacesSQL
{
	public class GroupsSQL: DBHelperDapper
	{
		#region Properties
		public int companyId;
		public int productId;
		public string searchGeneric;
		public DateTime? dateStart;
		public DateTime? dateEnd;

		//Datos del filtro para Empaques y Etiquetas
		public List<GroupDataSQL> groups;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("GroupsSQL");
		#endregion

		#region Constructor
		public GroupsSQL()
		{
			this.companyId = 0;
			this.productId = 0;
			this.searchGeneric = String.Empty;
			this.dateStart = default(DateTime);
			this.dateEnd = default(DateTime);

			this.groups = new List<GroupDataSQL>();
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar las agrupaciones realizadas en la Linea de Producción
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<GroupDataSQL> SearchGroupsPackedLabeled()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_productId", productId, DbType.Int32);
				parameters.Add("_searchGeneric", searchGeneric, DbType.String);
				parameters.Add("_dateStart", dateStart, DbType.DateTime);
				parameters.Add("_dateEnd", dateEnd, DbType.DateTime);

				List<GroupDataSQL> response = Consulta<GroupDataSQL>("spc_consultaAgrupacionesEmpaqEtiq", parameters);
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

		#endregion
	}
}
