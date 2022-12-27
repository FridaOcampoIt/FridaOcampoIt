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
	public class GroupingSQL: DBHelperDapper
	{
		#region Properties
		public int companyId;
		public int productId;
		public string searchGeneric;
		public string dateStart;
		public string dateEnd;
		public bool chkDistributor;
		public int usuarioId;
		public int opc;
		public int empacadorId;
		//Datos del filtro para Empaques y Etiquetas
		public List<GroupingDataSQL> groupingList;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("GroupingSQL");
		#endregion

		#region Constructor
		public GroupingSQL()
		{
			this.companyId = 0;
			this.productId = 0;
			this.searchGeneric = String.Empty;
			this.dateStart = String.Empty;
			this.dateEnd = String.Empty;
			this.chkDistributor = false;
			this.usuarioId = 0;
			this.opc = 0;
			this.empacadorId = 0;
			this.groupingList = new List<GroupingDataSQL>();
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar las agrupaciones realizadas en la Linea de Producción
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<GroupingDataSQL> SearchGroupsPackedLabeled()
		{
			log.trace("SearchGroupsPackedLabeledSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_productId", productId, DbType.Int32);
				// parameters.Add("_searchGeneric", searchGeneric, DbType.String);
				parameters.Add("_dateStart", dateStart == "" ? null : dateStart, DbType.String);
				parameters.Add("_dateEnd", dateEnd == "" ? null : dateEnd, DbType.String);
				parameters.Add("_empacadorId", empacadorId, DbType.Int32);
				// parameters.Add("_chkDistributor", chkDistributor, DbType.Boolean);
				// parameters.Add("_usuarioId", usuarioId, DbType.Int32);
				parameters.Add("_opc", opc, DbType.Int32);

				List<GroupingDataSQL> response = Consulta<GroupingDataSQL>("spc_consultaAgrupacionesEmpaqEtiq", parameters);
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
