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
     *  Controlador principal de productores
    */

    [Route("Productor")]
    [ApiController]
    public class ProductorController : ControllerBase
    {

        private LoggerD4 log = new LoggerD4("Productor");
        /// <summary>
        /// Web Method para guardar productor
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("saveProductor")]
        [HttpPost]
        [Authorize]
        public IActionResult saveProductor([FromBody] SaveProductorRequest request)
        {
            log.trace("saveProductor");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SaveProductorResponse response = new SaveProductorResponse();
            try{
                ProductorSQL sql = new ProductorSQL();
                sql.activo = request.activo;
                sql.numeroProductor = request.numeroProductor;
                sql.nombreProductor = request.nombreProductor;
                sql.nombreContacto = request.nombreContacto;
                sql.apellidoContacto = request.apellidoContacto;
                sql.telefonoContacto = request.telefonoContacto;
                sql.nombreRancho = request.nombreRancho;
                sql.direccion = request.address;
                sql.latitud = request.latitude;
                sql.longitud = request.longitude;
                sql.usuarioCreador = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                sql.companiaId = request.companiaId;
                sql.acopiosId = request.acopiosId;
                sql.SaveProductor();
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
        /// Web Method para guardar productor
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchListProductor")]
        [HttpPost]
        [Authorize]
        public IActionResult searchListProductor([FromBody] SearchProductorRequest request)
        {
            log.trace("searchListProductor");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            searchListProductorResponse response = new searchListProductorResponse();
            try
            {
                ProductorSQL sql = new ProductorSQL();
                sql.companiaId = request.companiaId;
                sql.nombreProductorNumeroNombreAcopio = request.nombreProductorNumeroNombreAcopio;
                response.searchListProductor =  sql.searchProductores();
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
        /// Web Method para obtener productor mediante id
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchProductorById")]
        [HttpPost]
        [Authorize]
        public IActionResult searchProductorById([FromBody] SearchProductorByIdRequest request)
        {
            log.trace("searchProductorById");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            searchProductorByIdResponse response = new searchProductorByIdResponse();
            try
            {
                ProductorSQL sql = new ProductorSQL();
                sql.productorId = request.productorId;
                response = sql.searchProductorById();
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
        /// Web Method para actualizar productor
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("updateProductorById")]
        [HttpPost]
        [Authorize]
        public IActionResult updateProductorById([FromBody] UpdateProductorByIdRequest request)
        {
            log.trace("updateProductorById");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            UpdateProductorResponse response = new UpdateProductorResponse();
            try
            {
                ProductorSQL sql = new ProductorSQL();
                sql.productorId = request.productorId;
                sql.activo = request.activo;
                sql.numeroProductor = request.numeroProductor;
                sql.nombreProductor = request.nombreProductor;
                sql.nombreRancho = request.nombreRancho;
                sql.direccion = request.address;
                sql.latitud = request.latitude;
                sql.longitud = request.longitude;
                sql.acopiosId = request.acopiosId;
                sql.auxAcopiosId = request.auxAcopiosId;
                sql.usuarioModificador = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                sql.UpdateProductorById();
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
        /// Web Method para eliminar el  productor (Actualizamos el campo Estatus a false esto dejara de listarlo y funcionar) 
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("deleteProductorById")]
        [HttpPost]
        [Authorize]
        public IActionResult deleteProductorById([FromBody] DeleteProductorRequest request)
        {
            log.trace("deleteProductorById");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            UpdateProductorResponse response = new UpdateProductorResponse();
            try
            {
                ProductorSQL sql = new ProductorSQL();
                sql.productorId = request.productorId;
                sql.usuarioModificador = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                sql.DeleteProductorById();
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