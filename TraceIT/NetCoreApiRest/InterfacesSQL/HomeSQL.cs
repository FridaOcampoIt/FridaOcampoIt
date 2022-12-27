using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Base.Home;
using WSTraceIT.Models.Response;
using NetCoreApiRest.Utils;

namespace WSTraceIT.InterfacesSQL
{
	public class HomeEtiquetadoEmbalajeFrutaSQL : DBHelperDapper
	{
		#region Properties
		//Datos Etiquetado Empacador
		public int companyId;
		public string fechaInicio;
		public string fechaFinal;
		//Nombre SP a ejecutar
		public string sp;


		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("HomeSQL");
		#endregion

		#region Constructor
		public HomeEtiquetadoEmbalajeFrutaSQL()
		{
			this.companyId = 0;
			this.fechaInicio = String.Empty;
			this.fechaFinal = String.Empty;
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar el Etiquetado de empacador
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		public searchEtiquetaEmabalajeFrutaResponse searchEtiquetadoEmpacadores()
		{
			log.trace("searchEtiquetadoEmpacadores");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_fechaInicio", fechaInicio, DbType.String);
				parameters.Add("_fechaFinal", fechaFinal, DbType.String);

				searchEtiquetaEmabalajeFrutaResponse response = Consulta<searchEtiquetaEmabalajeFrutaResponse>("spc_consultaEtiquetaEmabalajeFruta", parameters).FirstOrDefault();
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
		#endregion

		#endregion
	}




	public class HomeEmpaqueEnviadoReprocesoSQL : DBHelperDapper
	{
		#region Properties
		//Datos Etiquetado Empacador
		public int companyId;
		public string fechaInicio;
		public string fechaFinal;

		//Nombre SP a ejecutar
		public string sp;


		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("HomeSQL");
		#endregion

		#region Constructor
		public HomeEmpaqueEnviadoReprocesoSQL()
		{
			this.companyId = 0;
			this.fechaInicio = String.Empty;
			this.fechaFinal = String.Empty;
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar empaques enviados a reproceso / productores 
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		public List<searchEmpaquesEnviadosReprocesoResponse> searchEmpaquesEnviadosReproceso()
		{
			log.trace("searchEmpaquesEnviadosReproceso");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_fechaInicio", fechaInicio, DbType.String);
				parameters.Add("_fechaFinal", fechaFinal, DbType.String);

				var response = Consulta<searchEmpaquesEnviadosReprocesoResponse>("spc_consultaEmpaquesEnviadosReproceso", parameters);
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
		#endregion

		#endregion
	}



	public class HomeFrutaRecibidaReprocesoSQL : DBHelperDapper
	{
		#region Properties
		//Datos Etiquetado Empacador
		public int companyId;
		public string fechaInicio;
		public string fechaFinal;

		//Nombre SP a ejecutar
		public string sp;


		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("HomeSQL");
		#endregion

		#region Constructor
		public HomeFrutaRecibidaReprocesoSQL()
		{
			this.companyId = 0;
			this.fechaInicio = String.Empty;
			this.fechaFinal = String.Empty;
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar empaques enviados a reproceso / productores 
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		public List<searchFrutaRecibidaReprocesoResponse> searchFrutaRecibidaReproceso()
		{
			log.trace("searchEmpaquesEnviadosReproceso");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_fechaInicio", fechaInicio, DbType.String);
				parameters.Add("_fechaFinal", fechaFinal, DbType.String);

				var response = Consulta<searchFrutaRecibidaReprocesoResponse>("spc_consultaFrutosRecibidosReproceso", parameters);
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
		#endregion

		#endregion
	}


	public class HomeOperacionEmpacadoresSQL : DBHelperDapper
	{
		#region Properties
		//Datos Etiquetado Empacador
		public int companyId;
		public string fechaInicio;
		public string fechaFinal;

		//Nombre SP a ejecutar
		public string sp;


		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("HomeSQL");
		#endregion

		#region Constructor
		public HomeOperacionEmpacadoresSQL()
		{
			this.companyId = 0;
			this.fechaInicio = String.Empty;
			this.fechaFinal = String.Empty;
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar empaques enviados a reproceso / productores 
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		public List<searchOperacionEmpacadorResponse> searchOperacionEmpacadores()
		{
			log.trace("searchEmpaquesEnviadosReproceso");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_fechaInicio", fechaInicio, DbType.String);
				parameters.Add("_fechaFinal", fechaFinal, DbType.String);

				var response = Consulta<searchOperacionEmpacadorResponse>("spc_consultaOperacionEmpacadores", parameters);
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
		#endregion

		#endregion
	}


	public class HomeOperacionEmpacadorSQL : DBHelperDapper
	{
		#region Properties
		//Datos Etiquetado Empacador
		public int companyId;
		public string fechaInicio;
		public string fechaFinal;

		//Nombre SP a ejecutar
		public string sp;


		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("HomeSQL");
		#endregion

		#region Constructor
		public HomeOperacionEmpacadorSQL()
		{
			this.companyId = 0;
			this.fechaInicio = String.Empty;
			this.fechaFinal = String.Empty;
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar empaques enviados a reproceso / productores 
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <returns></returns>
		public List<searchOperacionEmpacadorResponse> searchOperacionEmpacador()
		{
			log.trace("searchEmpaquesEnviadosReproceso");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companyId", companyId, DbType.Int32);
				parameters.Add("_fechaInicio", fechaInicio, DbType.String);
				parameters.Add("_fechaFinal", fechaFinal, DbType.String);

				var response = Consulta<searchOperacionEmpacadorResponse>("spc_consultaOperacionEmpacador", parameters);
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
		#endregion

		#endregion
	}
}
