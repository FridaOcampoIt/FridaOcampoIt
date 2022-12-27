using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;
using System.IO;

namespace WSTraceIT.Controllers
{
    [Route("Robbery")]
    [ApiController]
    public class RobberyContoller : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("RobberyContoller");
        /// <summary>
        /// Web Method para guardar acopio
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("guardarReporteRobo")]
        [HttpPost]
        [Authorize]
        public IActionResult GuardarReporteRobo([FromBody] Robo request)
        {
            log.trace("guardarReporteRobo");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SaveRoboResponse response = new SaveRoboResponse();
            try
            {
                RobberySQL sql = new RobberySQL();
                sql.tituloAlertaES = request.tituloAlertaES;
                sql.descripcionES = request.descripcionES;
                sql.tituloAlertaEN = request.tituloAlertaEN;
                sql.descripcionEN = request.descripcionEN;
                sql.notificacionMovilCiu = request.notificacionMovilCiu;
                sql.notificacionTracking = request.notificacionTracking;
                sql.usuarioCreadorId = request.usuarioCreadorId;
                sql.companiaId = request.companiaId;
                sql.usuarioSolicitoId = request.usuarioSolicitoId;
                sql.tipoAlertaId = request.tipoAlertaId;
                sql.nombreArchivo = request.nombreArchivo;
                sql.tipoReporteId = request.tipoReporteId;
                sql.familiaId = request.familiaId;
                sql.codigoAlerta = request.codigoAlerta;
                sql.agrupacionId = request.agrupacionId;
                sql.GuardarReporteRobo();
            }
            catch (Exception ex)
            {
                log.error("Exception: " + ex.Message);
                response.messageEng = "An error occurred: " + ex.Message;
                response.messageEsp = "Ocurrio un error: " + ex.Message;
            }

            log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

            return Ok(response);
        }


        /// <summary>
        /// Web Method para obtener los reportes generales y/o filtrados
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("buscarReportesRobo")]
        [HttpPost]
        [Authorize]
        public IActionResult BuscarReportesRobo([FromBody] SearchRoboRequest request)
        {
            log.trace("buscarReportesRobo");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            ListaRobo response = new ListaRobo();
            try
            {
                RobberySQL sql = new RobberySQL();
                response.listaRobo = sql.BuscarReportesRobo();
            }
            catch (Exception ex)
            {
                log.error("Exception: " + ex.Message);
                response.messageEng = "An error occurred: " + ex.Message;
                response.messageEsp = "Ocurrio un error: " + ex.Message;
            }

            log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

            return Ok(response);
        }




        [Route("saveDocsRobery")]
        [HttpPost]
        [Authorize]
        public IActionResult saveDocsRobery()
        {
            log.trace("saveDocsRobery");
            saveDocResponse response = new saveDocResponse();
            try
            {
                var httpRequest = HttpContext.Request;

                MovimientosSQL sql = new MovimientosSQL();
                sql.doc = httpRequest.Form["doc"].ToString().Trim();
                sql.nombreDoc = httpRequest.Form["nombreDoc"].ToString().Trim();


                // Guardar archivo
                if (httpRequest.Form.Files.Count > 0)
                {
                    if (((httpRequest.Form.Files[0].Length / 1024) / 1024) > 10)
                    {
                        throw new Exception("El archivo debe ser menor a 10 MB");
                    }
                    string fileName = httpRequest.Form.Files[0].FileName;
                    string filepath = ConfigurationSite._cofiguration["Paths:pathFilesAlertas"] + "/";
                    string fullPath = Path.Combine(filepath, fileName);

                    if (!Directory.Exists(filepath))
                        Directory.CreateDirectory(filepath);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        httpRequest.Form.Files[0].CopyTo(stream);
                    }
                }
                response.Accion = 1;
            }
            catch (Exception ex)
            {

                log.error("Exception: " + ex.Message);
                response.messageEng = "An error occurred: " + ex.Message;
                response.messageEsp = "Ocurrio un error: " + ex.Message;
                response.Accion = 0;
            }
            

            return Ok(response);
        }


        /// <summary>
        /// Web Method para la consulta de los UDID con sus respectivos productos
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("buscarUDIDProducto")]
        [HttpPost]
        [Authorize]
        public IActionResult BuscarUDIDProducto([FromBody]Robo request)
        {
            ListFamiliasProductosRes responseSQL = new ListFamiliasProductosRes();
            try
            {
                RobberySQL sql = new RobberySQL();
                responseSQL = sql.BuscarUDIDProducto(request);
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
                responseSQL.messageEng = "An error occurred: " + ex.Message;
                responseSQL.messageEsp = "Ocurrio un error: " + ex.Message;
            }
            return Ok(responseSQL);
        }

        /// <summary>
        /// Web Method para la consulta del detalle del robo
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("roboDetalle")]
        [HttpPost]
        [Authorize]
        public IActionResult RoboDetalle([FromBody]Robo request)
        {
            Robo responseSQL = new Robo();
            try
            {
                RobberySQL sql = new RobberySQL();
                sql.roboId = request.roboId;
                responseSQL = sql.RoboDetalle();
            }
            catch (Exception ex)
            { log.error("Exception: " + ex.Message); 
                responseSQL.saveRoboResponse.messageEng = "An error occurred: " + ex.Message;
                responseSQL.saveRoboResponse.messageEsp = "Ocurrio un error: " + ex.Message;
            }
            return Ok(responseSQL);
        }

        [Route("obtenerProductoReportado")]
        [HttpPost]
        [Authorize]
        public IActionResult ObtenerProductoReportado([FromBody]int productoId)
        {
            log.trace("ObtenerProductoReportado");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(productoId));
            RobberyRegistryInfo responseSQL = new RobberyRegistryInfo();
            try
            {
                RobberySQL sql = new RobberySQL();
                responseSQL = sql.ObtenerProductoReportado(productoId);
                responseSQL.messageEng = "";
                responseSQL.messageEsp = "";
            }
            catch (Exception ex)
            {
                log.error("Exception: " + ex.Message);
                responseSQL.messageEng = "An error occurred: " + ex.Message;
                responseSQL.messageEsp = "Ocurrio un error: " + ex.Message;
            }
            return Ok(responseSQL);
        }


        [Route("eliminarAlertaRobo")]
        [HttpPost]
        [Authorize]
        public IActionResult eliminarAlertaRobo([FromBody] Robo request)
        {
            log.trace("eliminarAlertaRobo");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            eliminarAlertaResponse response = new eliminarAlertaResponse();
            try
            {
                RobberySQL sql = new RobberySQL();
                sql.roboId = request.roboId;
                sql.EliminarReporteRobo();
            }
            catch (Exception ex)
            {
                log.error("Exception: " + ex.Message);
                response.messageEng = "An error occurred: " + ex.Message;
                response.messageEsp = "Ocurrio un error: " + ex.Message;
            }
            log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
            return Ok(response);
        }


    }
}
