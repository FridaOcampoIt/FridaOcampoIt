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
    public class LabelSQL : DBHelperDapper
    {
		private LoggerD4 log = new LoggerD4("LabelSQL");
        #region Metodos publicos
        /// <summary>
        /// Metodo para ejecutar el sp para la consulta de las solicitudes de etiqueta
        /// Desarrollador: Oscar Ruesga
        /// </summary>
        /// <returns></returns>
        public ListaSolicitudEtiquetas BuscarSolicitudEtiquetas(SolicitudesEtiquetas param)
        {
            log.trace("BuscarSolicitudEtiquetas");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(param));
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_company", param.companiaId, DbType.Int32);
                parameters.Add("_folio", param.folio, DbType.String);
                ListaSolicitudEtiquetas response = new ListaSolicitudEtiquetas();
                response.listaSolicitudEtiquetas = Consulta<SolicitudesEtiquetas>("spc_solicitudesEtiquetas", parameters);
                CerrarConexion();

                return response;
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
CerrarConexion();
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para ejecutar el sp para la consulta de las compañias
        /// Desarrollador: Oscar Ruesga
        /// </summary>
        /// <returns></returns>
        public ListaCompania BuscarCompanias(int param)
        {
            log.trace("BuscarCompanias");
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_company", param, DbType.Int32);
                ListaCompania response = new ListaCompania();
                response.listaCompanias = Consulta<Companias>("spc_compania", parameters);
                CerrarConexion();

                return response;
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
CerrarConexion();
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para ejecutar el sp para la consulta del detalle de la solicitud
        /// Desarrollador: Oscar Ruesga
        /// </summary>
        /// <returns></returns>
        public ListaSolicitudEtiquetas DetalleSolicitud(SolicitudesEtiquetas param)
        {
            log.trace("DetalleSolicitud");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(param));
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_solicitudId", param.solicitudId, DbType.Int32);
                ListaSolicitudEtiquetas response = new ListaSolicitudEtiquetas();
                response.listaSolicitudEtiquetas = Consulta<SolicitudesEtiquetas>("spc_detalleSolicitud", parameters);
                CerrarConexion();

                return response;
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
CerrarConexion();
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para ejecutar el sp para guardar la bitacora
        /// Desarrollador: Oscar Ruesga
        /// </summary>
        /// <returns></returns>
        public void GuardarBitacoraSolicitud(Bitacora param)
        {
            log.trace("GuardarBitacoraSolicitud");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(param));
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_descripcion", param.descripcion, DbType.String);
                parameters.Add("_solicitudId", param.solicitudId, DbType.Int32);
                parameters.Add("_usuarioId", param.usuarioId, DbType.Int32);
                parameters.Add("_statusSolicitudId", param.estatusSolicitudId, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spi_bitacoraSolicitud", parameters);
                int response = parameters.Get<int>("_response");

                if (response == 0)
                    throw new Exception("Error al ejecutar el SP");
                else if (response == -1)
                    throw new Exception("Error al guardar");

                TransComit();
                CerrarConexion();
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
TransRollback();
                CerrarConexion();
                throw ex;
            }
        }


        /// <summary>
        /// Metodo para ejecutar el sp para la consulta del historial de bitacora
        /// Desarrollador: Oscar Ruesga
        /// </summary>
        /// <returns></returns>
        public ListaBitacoras HistorialBitacora(Bitacora param)
        {
            log.trace("HistorialBitacora");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(param));
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_solicitudId", param.solicitudId, DbType.Int32);
                ListaBitacoras response = new ListaBitacoras();
                response.listaBitacoras = Consulta<Bitacora>("spc_historialBitacora", parameters);
                CerrarConexion();

                return response;
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
CerrarConexion();
                throw ex;
            }
        }


        /// <summary>
        /// Metodo para ejecutar el sp para actualizar el status de la solicitud
        /// Desarrollador: Oscar Ruesga
        /// </summary>
        /// <returns></returns>
        public void ActualizarEstatusSolicitud(SolicitudesEtiquetas param)
        {
            log.trace("ActualizarEstatusSolicitud");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(param));
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_solicitudId", param.solicitudId, DbType.Int32);
                parameters.Add("_statusSolicitudId", param.estatusSolicitudId, DbType.Int32);
                parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                parameters = EjecutarSPOutPut("spu_estatusSolicitud", parameters);
                int response = parameters.Get<int>("_response");

                if (response == 0)
                    throw new Exception("Error al ejecutar el SP");
                else if (response == -1)
                    throw new Exception("Error al guardar");

                TransComit();
                CerrarConexion();
            }catch (Exception ex){
				log.error("Exception: " + ex.Message); 
				TransRollback();
                CerrarConexion();
                throw ex;
            }
        }

        /// <summary>
		/// Metodo para guardar los productos
		/// Desarrollador: Oscar Ruesga
		/// </summary>
		public string[] GuardarSolicitudArchivo(ListaSolicitudEtiquetas lista)
        {
            log.trace("GuardarSolicitudArchivo");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(lista));
			string[] resp = new string[2];
            //List<string> QR = new List<string>();
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();
				int userId = 0;
                
                string query = "set @consecutivo = (SELECT LPAD(((SELECT cast(Folio as SIGNED) FROM tra_002_solicitudqr ORDER by cast(Folio as SIGNED) DESC Limit 1) + 1), 5, '0'));";
                query += "insert into tra_002_solicitudqr (Folio, Cantidad, Timestamp, FamiliaProductoId, UsuarioId,StatusSolicitudId, DireccionId, UDID, Caducidad) values ";
                foreach (var solicitud in lista.listaSolicitudEtiquetas)
                {
                    string fechastr = solicitud.caducidad.ToString();
                    DateTime fecha = new DateTime();
                    if(fechastr != null)
                    {
                        fecha = DateTime.Parse(fechastr);
                        fechastr = fecha.ToString("yyyy-MM-dd");
                    }
					userId = solicitud.usuarioId;
                    query += "(@consecutivo,'" + solicitud.cantidad + "', date_format(now(), '%d/%m/%Y %h:%i:%s %p'), '" + solicitud.familiaId + "','" + solicitud.usuarioId + "','1','" + solicitud.direccionId + "','" + solicitud.udid + "','" + fechastr + "'),";
                }

                //ConsultaCommand<string>("LOAD DATA INFILE '"+ path + filename + ".tmp.txt" + "' INTO TABLE cat_004_producto FIELDS TERMINATED BY ',' (UDID,FechaCaducidad,CodigoQR,FamiliaProductoId,TipoProductoId,DireccionId)");
                query = query.TrimEnd(',');
                query += "; select @consecutivo;";
                var folios = ConsultaCommand<string>(query);
				var username = ConsultaCommand<string>("SELECT CONCAT(Nombre,' ', Apellido) FROM seg_001_usuario WHERE PK_UsuarioId = "+userId+";");
				TransComit();
                CerrarConexion();

                //System.IO.File.Delete(path + filename + ".tmp.txt");//delete the file after import
                resp[0] =  folios.Count > 0 ? folios[0]:"0";
				resp[1] = username[0];
				return resp;
            }
            catch (Exception ex)
            {
				log.error("Exception: " + ex.Message); 
				TransRollback();
                CerrarConexion();
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para ejecutar el sp para guardar solicitudes
        /// Desarrollador: Oscar Ruesga
        /// </summary>
        /// <returns></returns>
        public string[] GuardarSolicitud(SolicitudesEtiquetas param)
        {
            log.trace("GuardarSolicitud");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(param));
            string[] response = new string[2];
            try
            {
                CrearConexion(TipoConexion.Mysql);
                AbrirConexion();
                CrearTransaccion();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("_familiaId", param.familiaId, DbType.Int32);
                parameters.Add("_usuarioId", param.usuarioId, DbType.Int32);
                parameters.Add("_statusId", param.estatusSolicitudId, DbType.Int32);
                parameters.Add("_origenId", param.direccionId, DbType.Int32);
                parameters.Add("_udid", param.udid, DbType.String);
                parameters.Add("_caducidad", param.caducidad, DbType.Date);
                parameters.Add("_cantidad", param.cantidad, DbType.Int32);
                parameters.Add("_response", dbType: DbType.String, direction: ParameterDirection.InputOutput);
				parameters.Add("_userName", dbType: DbType.String, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_crearSolicitudes", parameters);
                response[0] = parameters.Get<string>("_response");
				response[1] = parameters.Get<string>("_userName");

				if (response == null)
                    throw new Exception("Error al ejecutar el SP");
                else if (response[0] == "-1")
                    throw new Exception("Error al guardar");

                TransComit();
                CerrarConexion();
            }
            catch (Exception ex){
				log.error("Exception: " + ex.Message); 
				TransRollback();
                CerrarConexion();
                throw ex;
            }
            return response;
        }

        #endregion
    }
}
