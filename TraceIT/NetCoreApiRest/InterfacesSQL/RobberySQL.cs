using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;

namespace WSTraceIT.InterfacesSQL
{
    public class RobberySQL : DBHelperDapper
    {
        #region properties
        public int roboId; 
        public string tituloAlertaES;
        public string descripcionES;
        public string tituloAlertaEN;
        public string descripcionEN;
        public bool notificacionMovilCiu;
        public bool notificacionTracking;
        public int usuarioCreadorId;
        public int companiaId;
        public int usuarioSolicitoId;
        public int tipoAlertaId;
        public string nombreArchivo;
        public int tipoReporteId;
        public int familiaId;
        public string codigoAlerta;
        public string companiaFamilia;
        public bool agrupacionId;
        private LoggerD4 log = new LoggerD4("RobberySQL");
        #endregion
       
        #region Consturctor
        public RobberySQL()
        {
            this.roboId = 0;
            this.tituloAlertaES = String.Empty;
            this.descripcionES = String.Empty;
            this.tituloAlertaEN = String.Empty;
            this.descripcionEN = String.Empty;
            this.notificacionMovilCiu = false;
            this.notificacionTracking = false;
            this.usuarioCreadorId = 0;
            this.companiaId = 0;
            this.usuarioSolicitoId = 0;
            this.tipoAlertaId = 0;
            this.nombreArchivo = String.Empty;
            this.tipoReporteId = 0;
            this.familiaId = 0;
            this.codigoAlerta = String.Empty;
            this.agrupacionId = false;
            this.companiaFamilia = String.Empty;
        }
        #endregion
        /// <summary>
        /// Metodo para ejecutar el sp para guardar el reporte de robo
        /// Desarrollador: Oscar Ruesga -> Hernán Gómez
        /// </summary>
        /// <returns></returns>
        public void GuardarReporteRobo()
        {
            log.trace("GuardarReporteRobo");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_tituloAlertaES", tituloAlertaES, DbType.String);
                parameters.Add("_descripcionES", descripcionES, DbType.String);
                parameters.Add("_tituloAlertaEN", tituloAlertaEN, DbType.String);
                parameters.Add("_descripcionEN", descripcionEN, DbType.String);
                parameters.Add("_notificacionMovilCiu", notificacionMovilCiu, DbType.Boolean);
                parameters.Add("_notificacionTracking", notificacionTracking, DbType.Boolean);
                parameters.Add("_usuarioCreadorId", usuarioCreadorId, DbType.Int32);
                parameters.Add("_companiaId", companiaId, DbType.Int32);
                parameters.Add("_usuarioSolicitoId", usuarioSolicitoId, DbType.Int32);
                parameters.Add("_tipoAlerta", tipoAlertaId, DbType.Int32);
                parameters.Add("_nombreArchivo", nombreArchivo, DbType.String);
                parameters.Add("_tipoReporte", tipoReporteId, DbType.Int32);
                parameters.Add("_familiaId", familiaId, DbType.Int32);
                parameters.Add("_codigoAlerta", codigoAlerta, DbType.String);
                parameters.Add("_agrupacionId", agrupacionId, DbType.Boolean);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spi_guardarReporteRobo", parameters);
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



        #region Public method
        /// <summary>
        /// Metodo para consultar los reportes dados de alta!
        /// Desarrollador: Hernán Gómez
        /// </summary>
        public List<Robo> BuscarReportesRobo()
        {
            log.trace("searchReportesRobo");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();

                List<Robo> resp = Consulta<Robo>("spc_reportesRobo", parameters);
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

        /// <summary>
        /// Metodo para ejecutar el sp para la consulta de los UDID con sus respectivos productos
        /// Desarrollador: Oscar Ruesga
        /// </summary>
        /// <returns></returns>
        public ListFamiliasProductosRes BuscarUDIDProducto(Robo param)
        {
            log.trace("BuscarUDIDProducto");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(param));
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                List<Type> types = new List<Type>();
                types.Add(typeof(FamiliasProductosSelector));
                types.Add(typeof(FamiliasProductosSelector));

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_companiaId", param.companiaId, DbType.Int32);
                parameters.Add("_familiaId", param.familiaId, DbType.Int32);
                var respSQL = ConsultaMultiple("spc_familiasProductosSelector", types, parameters);
                ListFamiliasProductosRes response = new ListFamiliasProductosRes();
                List<FamiliasProductosSelector> udids = respSQL[0].Cast<FamiliasProductosSelector>().ToList();
                List<FamiliasProductosSelector> prod = respSQL[1].Cast<FamiliasProductosSelector>().ToList();

                foreach (FamiliasProductosSelector uUdid in udids)
                {
                    List<ProductosRes> productos = new List<ProductosRes>();
                    foreach (FamiliasProductosSelector lUdid in prod)
                    {
                        if (uUdid.udid == lUdid.udid)
                        {
                            ProductosRes producto = new ProductosRes();
                            producto.ProductoId = lUdid.productoId;
                            producto.ciu = lUdid.ciu;
                            productos.Add(producto);
                        }
                    }
                    FamiliasProductosSelectorRes famres = new FamiliasProductosSelectorRes();
                    famres.UDID = uUdid.udid;
                    famres.productos = productos;
                    response.ListaUdidsCius.Add(famres);
                }

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
        /// Metodo para ejecutar el sp para la consulta del detalle de los robos
        /// Desarrollador: Oscar Ruesga
        /// </summary>
        /// <returns></returns>
        public Robo RoboDetalle()
        {
            log.trace("RoboDetalle");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_idRobo", roboId, DbType.Int32);
                Robo respSQL = Consulta<Robo>("spc_detalleReportesRobo",  parameters).FirstOrDefault();
                CerrarConexion();

                return respSQL;
            }
            catch (Exception ex)
            {
                log.error("Exception: " + ex.Message);
                CerrarConexion();
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para ejecutar el sp para la consulta de las solicitudes de etiqueta
        /// Desarrollador: Oscar Ruesga
        /// </summary>
        /// <returns></returns>
        public RobberyRegistryInfo ObtenerProductoReportado(int productoId)
        {
            log.trace("ObtenerProductoReportado");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(productoId));
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                RobberyRegistryInfo res = new RobberyRegistryInfo();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_productoId", productoId, DbType.Int32);
                List<RobberyRegistryInfo> response = new List<RobberyRegistryInfo>();
                response = Consulta<RobberyRegistryInfo>("spc_ObtenerProductoReportado", parameters);
                if (response.Count > 0)
                {
                    res = response[0];
                }
                else
                {
                    res = new RobberyRegistryInfo();
                }
                CerrarConexion();

                return res;
            }
            catch (Exception ex)
            {
                log.error("Exception: " + ex.Message);
                CerrarConexion();
                throw ex;
            }
        }

        /*
         * Autor: Hernán Gómez
         * Metodo para desactivar alertas.
         */
        public void EliminarReporteRobo()
        {
            log.trace("EliminarReporteRobo");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(roboId));
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_alertaId", roboId, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                parameters = EjecutarSPOutPut("spd_eliminarAlertaRobo", parameters);

                int response = parameters.Get<int>("_response");
                if (response == 0)
                    throw new Exception("Error al ejecutar el SP");
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

    }
}
