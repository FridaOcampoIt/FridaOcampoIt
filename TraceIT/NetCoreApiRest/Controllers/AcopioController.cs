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
     *  Controlador enfocado para las operaciónes de acopios
    */

    [Route("Acopio")]
    [ApiController]
    public class AcopioController : ControllerBase
    {


        private LoggerD4 log = new LoggerD4("Acopio");

        /// <summary>
        /// Web Method para recuperar información general de acopios en la visa de proovedores
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchInformationAcopio")]
        [HttpPost]
        [Authorize]
        public IActionResult searchInformationAcopioByCompanyId([FromBody] SearchMovimientoByCompanyIdRequest request)
        {
            log.trace("searchMovimientoAcopioByCompanyId");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SearchMovimientoResponse response = new SearchMovimientoResponse();
            try
            {
                MovimientosSQL sql = new MovimientosSQL();
                sql.producto = request.producto;
                sql.fechaIngresoDe = request.datestart == "" ? null : request.datestart;
                sql.fechaIngresoHasta = request.dateEnd == "" ? null : request.dateEnd;
                sql.company = request.companiaId;

                response.movimientosDataList = sql.SearchMovimientosByCompaniaId();
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
        /// Web Method para guardar acopio
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("saveAcopio")]
        [HttpPost]
        [Authorize]
        public IActionResult saveAcopio([FromBody] SaveAcopioRequest request)
        {
            log.trace("saveAcopio");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SaveAcopioResponse response = new SaveAcopioResponse();
            try{
                AcopioSQL sql = new AcopioSQL();
                sql.activo = request.activo;
                sql.numeroAcopio = request.numeroAcopio;
                sql.nombreAcopio = request.nombreAcopio;
                sql.paisId = request.paisId;
                sql.estadoId = request.estadoId;
                sql.ciudad = request.ciudad;
                sql.codigoPostal = request.codigoPostal;
                sql.direccion = request.address;
                sql.latitud = request.latitude;
                sql.longitud = request.longitude;
                sql.usuarioCreador = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                sql.companiaId = request.companiaId;
                sql.SaveAcopio();
            }catch (Exception ex)
            {
                log.error("Exception: " + ex.Message);
                response.messageEng = "An error occurred: " + ex.Message;
                response.messageEsp = "Ocurrio un error: " + ex.Message;
            }

            log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

            return Ok(response);
        }



        /// <summary>
        /// Web Method para obtener los acopios por compañias
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchListAcopios")]
        [HttpPost]
        [Authorize]
        public IActionResult searchListAcopios([FromBody] SearchAcopioRequest request)
        {
            log.trace("searchAcopio");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            searchListAcopioResponse response = new searchListAcopioResponse();
            try
            {
                AcopioSQL sql = new AcopioSQL();
                sql.companiaId = request.companiaId;
                sql.nombreNumeroAcopio = request.nombreNumeroAcopio;
                response.searchListAcopio = sql.searchListAcopios();
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
        /// Web Method para obtener el detalle de un acopio
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchAcopioById")]
        [HttpPost]
        [Authorize]
        public IActionResult searchAcopioById([FromBody] SearchAcopioByIdRequest request)
        {
            log.trace("searchAcopio");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            searchAcopioByIdResponse response = new searchAcopioByIdResponse();
            try
            {
                AcopioSQL sql = new AcopioSQL();
                sql.acopioId = request.acopioId;
                response = sql.searchAcopioById();
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
        /// Web Method para actualizar datos de un acopio
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("updateAcopio")]
        [HttpPost]
        [Authorize]
        public IActionResult updateAcopio([FromBody] UpdateAcopioRequest request)
        {
            log.trace("updateAcopio");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            UpdateAcopioResponse response = new UpdateAcopioResponse();
            try
            {
                AcopioSQL sql = new AcopioSQL();
                sql.acopioId = request.acopioId;
                sql.activo = request.activo;
                sql.numeroAcopio = request.numeroAcopio;
                sql.nombreAcopio = request.nombreAcopio;
                sql.paisId = request.paisId;
                sql.estadoId = request.estadoId;
                sql.ciudad = request.ciudad;
                sql.codigoPostal = request.codigoPostal;
                sql.direccion = request.address;
                sql.latitud = request.latitude;
                sql.longitud = request.longitude;
                sql.usuarioModificador = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                sql.UpadateAcopio();
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
        /// Web Method para eliminar un acopio y sus productores.
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("deleteAcopioProductores")]
        [HttpPost]
        [Authorize]
        public IActionResult deleteAcopioProductores([FromBody] DeleteAcopioProductoresRequest request)
        {
            log.trace("deleteAcopioProductores");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            DeleteAcopioProductorResponse response = new DeleteAcopioProductorResponse();
            try
            {
                AcopioSQL sql = new AcopioSQL();
                sql.acopioId = request.acopioId;
                sql.usuarioModificador = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                sql.deleteAcopioProductor();
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
        /// Web Method para obtener el listado de productores por acopio's
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("getProductoresByAcopio")]
        [HttpPost]
        [Authorize]
        public IActionResult getProductoresByAcopio([FromBody] SearchProductorByAcopioRequest request)
        {
            log.trace("getProductoresByAcopio");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            searchListProductorByAcopioResponse response = new searchListProductorByAcopioResponse();
            try
            {
                AcopioSQL sql = new AcopioSQL();
                sql.acopiosIds = request.acopiosIds;
                sql.busqueda = request.busqueda;
                response.searchListProductorByAcopio = sql.getProductoresByAcopio();
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