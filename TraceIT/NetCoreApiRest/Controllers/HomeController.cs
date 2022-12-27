using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;
using System.Linq;
using WSTraceIT.Models.Base.Home;
using NetCoreApiRest.Utils;
using System.Collections.Generic;

namespace WSTraceIT.Controllers
{
    /*
     *  Autor: Hernán Gómez
     *  Fecha: 06-Abril-2022
     *  Controlador enfocado unicamente a la recolección de información.
     */
    [Route("Home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private LoggerD4 log = new LoggerD4("Home");
        /// <summary>
        /// Web Method para obtener el etiquetado de empacadores
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchEtiquetadoEmpacadores")]
        [HttpPost]
        [Authorize]
        public IActionResult searchEtiquetadoEmpacadores([FromBody] searchHomeDashbroadRequest request)
        {
            log.trace("searchEtiquetadoEmpacadores");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            searchEtiquetaEmabalajeFrutaResponse response = new searchEtiquetaEmabalajeFrutaResponse();
            try
            {
                HomeEtiquetadoEmbalajeFrutaSQL sql = new HomeEtiquetadoEmbalajeFrutaSQL();
                sql.companyId = request.companyId;
                
                sql.fechaInicio = request.fechaInicio;
                sql.fechaFinal = request.fechaFinal;

                response = sql.searchEtiquetadoEmpacadores();
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
        /// Web Method para obtener el etiquetado de empacadores
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchEmpaquesEnviadosReproceso")]
        [HttpPost]
        [Authorize]
        public IActionResult searchEmpaquesEnviadosReproceso([FromBody] searchHomeDashbroadRequest request)
        {
            log.trace("searchEtiquetadoEmpacadores");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            searchEmpaquesEnviadosReprocesoResponse response = new searchEmpaquesEnviadosReprocesoResponse();
            try
            {
                HomeEmpaqueEnviadoReprocesoSQL sql = new HomeEmpaqueEnviadoReprocesoSQL();
                sql.companyId = request.companyId;
                sql.fechaInicio = request.fechaInicio;
                sql.fechaFinal = request.fechaFinal;

                response.listEmpaquesEnviadosReproceso = sql.searchEmpaquesEnviadosReproceso();
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
        /// Web Method para obtener el etiquetado de empacadores
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchFrutaRecibidaReproceso")]
        [HttpPost]
        [Authorize]
        public IActionResult searchFrutaRecibidaReproceso([FromBody] searchHomeDashbroadRequest request)
        {
            log.trace("searchEtiquetadoEmpacadores");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            searchFrutaRecibidaReprocesoResponse response = new searchFrutaRecibidaReprocesoResponse();
            try
            {
                HomeFrutaRecibidaReprocesoSQL sql = new HomeFrutaRecibidaReprocesoSQL();
                sql.companyId = request.companyId;
                sql.fechaInicio = request.fechaInicio;
                sql.fechaFinal = request.fechaFinal;

                response.listFrutaRecibidaReprocesoResponse = sql.searchFrutaRecibidaReproceso();
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
        /// Web Method para obtener el etiquetado de empacadores
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchOperacionEmpacadores")]
        [HttpPost]
        [Authorize]
        public IActionResult searchOperacionEmpacadores([FromBody] searchHomeDashbroadRequest request)
        {
            log.trace("searchEtiquetadoEmpacadores");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            searchOperacionEmpacadorResponse response = new searchOperacionEmpacadorResponse();
            try
            {
                HomeOperacionEmpacadoresSQL sql = new HomeOperacionEmpacadoresSQL();
                sql.companyId = request.companyId;
                sql.fechaInicio = request.fechaInicio;
                sql.fechaFinal = request.fechaFinal;

                response.listsearchOperacionEmpacadoresResponse = sql.searchOperacionEmpacadores();
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
        /// Web Method para obtener el etiquetado de empacadores
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchOperacionEmpacador")]
        [HttpPost]
        [Authorize]
        public IActionResult searchOperacionEmpacador([FromBody] searchHomeDashbroadRequest request)
        {
            log.trace("searchEtiquetadoEmpacadores");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            searchOperacionEmpacadorResponse response = new searchOperacionEmpacadorResponse();
            try
            {
                HomeOperacionEmpacadorSQL sql = new HomeOperacionEmpacadorSQL();
                sql.companyId = request.companyId;
                sql.fechaInicio = request.fechaInicio;
                sql.fechaFinal = request.fechaFinal;

                response.listsearchOperacionEmpacadorResponse = sql.searchOperacionEmpacador();
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
