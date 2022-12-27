using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Address;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;
using WSTraceIT.Models.Response;

namespace WSTraceIT.InterfacesSQL
{
	public class AcopioSQL: DBHelperDapper
	{
		#region Properties
		public int acopioId;
		public Boolean activo;
		public Boolean estatus;
		public int numeroAcopio;
		public string nombreAcopio;
		public int paisId;
		public int estadoId;
		public string ciudad;
		public int codigoPostal;
		public string direccion;
		public string latitud;
		public string longitud;
		public int usuarioCreador;
		public int companiaId;
		public DateTime fechaCreacion;
		public DateTime fechaModificacion;
		public int usuarioModificador;
		public string nombreNumeroAcopio;
		public string acopiosIds;
		public string busqueda;
		private LoggerD4 log = new LoggerD4("AcopioSQL");
		#endregion

		#region Constructor
		public AcopioSQL()
		{
			this.acopioId = 0;
			this.estatus = true;
			this.activo = true;
			this.numeroAcopio = 0;
			this.nombreAcopio = String.Empty;
			this.paisId = 0;
			this.estadoId = 0;
			this.ciudad = String.Empty;
			this.codigoPostal = 0;
			this.direccion = String.Empty;
			this.latitud = String.Empty;
			this.longitud = String.Empty;
			this.usuarioCreador = 0;
			this.companiaId = 0;
			this.fechaCreacion = DateTime.Now;
			this.fechaModificacion = DateTime.Now;
			this.usuarioModificador = 0;
			this.nombreNumeroAcopio = String.Empty;
			this.acopiosIds = String.Empty;
			this.busqueda = String.Empty;
		}
		#endregion

		#region Public method
		/// <summary>
		/// Metodo para guardar los acopios
		/// Desarrollador: Hernán Gómez
		/// </summary>
		public void SaveAcopio()
		{
			log.trace("SaveAcopio");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters(); 
				parameters.Add("_activo", activo, DbType.Boolean);
				parameters.Add("_numeroAcopio", numeroAcopio, DbType.Int32);
				parameters.Add("_nombreAcopio", nombreAcopio, DbType.String);
				parameters.Add("_paisId", paisId, DbType.Int32);
				parameters.Add("_estadoId", estadoId, DbType.Int32);
				parameters.Add("_ciudad", ciudad, DbType.String);
				parameters.Add("_codigoPostal", codigoPostal, DbType.Int32);
				parameters.Add("_direccion", direccion, DbType.String);
				parameters.Add("_latitud", latitud, DbType.String);
				parameters.Add("_longitud", longitud, DbType.String);
				parameters.Add("_usuarioCreador", usuarioCreador, DbType.Int32);
				parameters.Add("_companiaId", companiaId, DbType.Int64);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarAcopio", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el sp");
				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{	
				log.error("Exception: " + ex.Message);	
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}
		#endregion



		#region Public method
		/// <summary>
		/// Metodo para consultar los acopios por compañia
		/// Desarrollador: Hernán Gómez
		/// </summary>
		public List<searchAcopioResponse> searchListAcopios()
		{
			log.trace("searchListAcopios");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_companiaId", companiaId, DbType.Int64);
				parameters.Add("_nombreNumeroAcopio", nombreNumeroAcopio, DbType.String);

				List<searchAcopioResponse> resp = Consulta<searchAcopioResponse>("spc_consultaAcopioCompaniaByIdAndNameNumber", parameters);
				return resp;
				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}
		#endregion


		#region Public method
		/// <summary>
		/// Metodo para consultar el acopio por id
		/// Desarrollador: Hernán Gómez
		/// </summary>
		public searchAcopioByIdResponse searchAcopioById()
		{
			log.trace("searchAcopioById");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_acopioId", acopioId, DbType.Int64);

				searchAcopioByIdResponse resp = Consulta<searchAcopioByIdResponse>("spc_consultaAcopioById", parameters).FirstOrDefault();
				return resp;
				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}
		#endregion


		#region Public method
		/// <summary>
		/// Metodo para guardar los acopios
		/// Desarrollador: Hernán Gómez
		/// </summary>
		public void UpadateAcopio()
		{
			log.trace("UpadateAcopio");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_acopioId", acopioId, DbType.Int32);
				parameters.Add("_activo", activo, DbType.Boolean);
				parameters.Add("_numeroAcopio", numeroAcopio, DbType.Int32);
				parameters.Add("_nombreAcopio", nombreAcopio, DbType.String);
				parameters.Add("_paisId", paisId, DbType.Int32);
				parameters.Add("_estadoId", estadoId, DbType.Int32);
				parameters.Add("_ciudad", ciudad, DbType.String);
				parameters.Add("_codigoPostal", codigoPostal, DbType.Int32);
				parameters.Add("_direccion", direccion, DbType.String);
				parameters.Add("_latitud", latitud, DbType.String);
				parameters.Add("_longitud", longitud, DbType.String);
				parameters.Add("_usuarioModificador", usuarioModificador, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_updateAcopio", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el sp");
				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}
		#endregion

		#region Public method
		/// <summary>
		/// Metodo para eliminar el acopio, (involucra productores asociados al acopio)
		/// Desarrollador: Hernán Gómez
		/// </summary>
		public void deleteAcopioProductor()
		{
			log.trace("deleteAcopioProductor");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_acopioId", acopioId, DbType.Int32);
				parameters.Add("_usuarioModificador", usuarioModificador, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminaAcopioProductor", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el sp");
				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}
		#endregion



		#region Public method
		/// <summary>
		/// Metodo para obtener el listado de productores acorde al acopio.
		/// Desarrollador: Hernán Gómez
		/// </summary>
		public List<SearchProductorByAcopioResponse> getProductoresByAcopio()
		{
			log.trace("getProductoresByAcopio");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_arrayacopio", acopiosIds, DbType.String);
				parameters.Add("_busqueda", busqueda, DbType.String);
				List< SearchProductorByAcopioResponse> resp = Consulta<SearchProductorByAcopioResponse>("spc_listaAcopiosUsuario", parameters);
				return resp;
				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}
		#endregion

	}
}
