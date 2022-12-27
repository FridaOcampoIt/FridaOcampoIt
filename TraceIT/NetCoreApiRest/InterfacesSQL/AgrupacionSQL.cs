using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;
using Dapper;
using DWUtils;
using System.Data;
using WSTraceIT.Models.Base.Agrupaciones;
using WSTraceIT.Models.Base;

namespace WSTraceIT.InterfacesSQL
{
    public class AgrupacionSQL : DBHelperDapper
    {
        #region Properties
        //Datos de busquedas
        public string codigo;

        public int fichaId;
        public int tipoFicha;
        public int usuario;
		public int desconocidoId;

		public int movFichaId;

		public string nombre { get; set; }
		public int cantidad { get; set; }
		public string lote { get; set; }
		public string fechaCaducidad { get; set; }
		public string numSerie { get; set; }

		private LoggerD4 log = new LoggerD4("AgrupacionSQL");
        #endregion

        #region Constructor
        public AgrupacionSQL()
        {
            //Datos de busquedas
            this.codigo = String.Empty;

            this.fichaId = 0;
            this.tipoFicha = 0;
            this.usuario = 0;
			this.desconocidoId = 0;

			this.movFichaId = 0;

			this.nombre = String.Empty;
			this.cantidad = 0;
			this.lote = String.Empty;
			this.fechaCaducidad = String.Empty;
			this.numSerie = String.Empty;
        }
        #endregion

        #region Public method


        #region Busqueda de datos pertenecientes a una ficha
        /// <summary>
        ///	Metodo para consultar los datos generales de la selección de un movimiento
        ///	Desarrollador: Javier Ramirez
        /// </summary>
        /// <returns></returns>
        public void SearchFicha()
        {
            log.trace("SearchFicha");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_Codigo", codigo, DbType.Int32);
				parameters.Add("_usuario", usuario, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spc_ConsultaDataFicha", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el sp");

				TransComit();
				CerrarConexion();
            }
            catch (Exception ex)
            {
                log.error("Exception: " + ex.Message);
                CerrarConexion();
                throw ex;
            }
        }

		/// <summary>
		///	Metodo para consultar la longitud de la ficha
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public List<FichaData> SearchFicha2()
		{
			log.trace("SearchFicha2");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_Codigo", codigo, DbType.Int32);
				parameters.Add("_usuario", usuario, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				List<FichaData> resp = Consulta<FichaData>("spc_ConsultaDataFicha", parameters);
				CerrarConexion();

				return resp;
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		#endregion

		#region Guardar ficha temporal
		/// <summary>
		/// Metodo para guardar los datos de la compañia 
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveFichaTemporal()
		{
			log.trace("SaveFichaTemporal");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_fichaID", fichaId, DbType.String);
				parameters.Add("_tipoFicha", tipoFicha, DbType.String);
				parameters.Add("_usuario", usuario, DbType.String);
				parameters.Add("_desconocidoId", desconocidoId, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_GuardarFichasBuscadas", parameters);
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

		#region Busqueda para fichas de agrupaciones seleccionadas
		/// <summary>
		///	Metodo para consultar las datos de las fichas seleccionadas
		///	Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		public FichasSeleccionadasData SearchFichasSeleccionadas()
		{
			log.trace("SearchFichasSeleccionadas");
			try
			{
				FichasSeleccionadasData resp = new FichasSeleccionadasData();
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(TraceITListFichas));
				types.Add(typeof(TraceITListFichas));
				types.Add(typeof(TraceITListFichas));
				types.Add(typeof(TraceITListFichas));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_usuario", usuario, DbType.String);

				var respSQL = ConsultaMultiple("spc_ConsultaDataFichasSeleccionadas", types, parameters);

				resp.fichasSeleccionadasDataList = respSQL[0].Cast<TraceITListFichas>().ToList();
				resp.fichasDesconocidasDataList = respSQL[1].Cast<TraceITListFichas>().ToList();
				resp.fichasUnicasDataList = respSQL[2].Cast<TraceITListFichas>().ToList();
				resp.fichasMovimientosDataList = respSQL[3].Cast<TraceITListFichas>().ToList();

				CerrarConexion();
				return resp;
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}
		#endregion


		#region Eliminar una de las fichas seleccionadas para su reagrupación
		/// <summary>
		/// Metodo para eliminar una de las fichas seleccionadas
		/// Desarrollador: David Martinez
		/// </summary>
		public void DeleteFichaSeleccionada()
		{
			log.trace("DeleteFichaSeleccionada");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_movfichaId", movFichaId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarFichaSeleccionada", parameters);
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

		#region Guardar producto desconocido
		/// <summary>
		/// Metodo para guardar los datos de la compañia 
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveProductoDesconocido()
		{
			log.trace("SaveProductoDesconocido");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_Nombre", nombre, DbType.String);
				parameters.Add("_Cantidad", cantidad, DbType.String);
				parameters.Add("_Lote", lote, DbType.String);
				parameters.Add("_FechaCaducidad", fechaCaducidad, DbType.String);
				parameters.Add("_NumSerie", numSerie, DbType.String);
				parameters.Add("_Codigo", codigo, DbType.String);
				parameters.Add("_usuario", usuario, DbType.String);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_GuardarProductoDesconocido", parameters);
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

		#endregion
	}
}
