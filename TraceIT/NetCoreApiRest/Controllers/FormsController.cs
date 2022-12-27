using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using System.Linq;
using System;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;
using Microsoft.AspNetCore.Authorization;
using WSTraceIT.Models.Base.Forms;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using WSTraceIT.Models.Base.Sectors;

namespace WSTraceIT.Controllers
{


    [Route("Forms")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private LoggerD4 log = new LoggerD4("Forms");

        #region save
        /// <summary>
        /// Web Method para guardar Formularios
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("saveForms")]
        [HttpPost]
        [Authorize]
        public IActionResult saveForms([FromBody] saveFormsRequest request)
        {
            log.trace("saveForms");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            saveFormsResponse response = new saveFormsResponse();
            try
            {
                FormsSQL sql = new FormsSQL();
                sql.formularioId = request.formularioId;
                sql.nombre = request.nombre;
                sql.descripcionCorta = request.descripcionCorta;
                sql.usuarioCreadorId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                sql.estatusFormularioId = request.estatusFormularioId;
                sql.sectorId = request.sectorId;
                sql.saveForms();
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
        #endregion

        #region search 
        /// <summary>
        /// Web Method para obtener el detalle de un acopio
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchFormsById")]
        [HttpPost]
        [Authorize]
        public IActionResult searchFormsById([FromBody] searchFormsRequest request)
        {
            log.trace("searchFormsById");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            searchFormsByIdResponse response = new searchFormsByIdResponse();
            try
            {
                FormsSQL sql = new FormsSQL();
                sql.formularioId = request.formularioId;
                response = sql.searchFormsById();
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
        #endregion

        #region update
        /// <summary>
        /// Web Method para actualizar datos de un acopio
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("updateForms")]
        [HttpPost]
        [Authorize]
        public IActionResult updateForms([FromBody] updateFormsRequest request)
        {
            log.trace("updateForms");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            updateFormsResponse response = new updateFormsResponse();
            try
            {
                FormsSQL sql = new FormsSQL();
                sql.formularioId = request.formularioId;
                sql.nombre = request.nombre;
                sql.descripcionCorta = request.descripcionCorta;
                sql.usuarioModificadorId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                sql.estatusFormularioId = request.estatusFormularioId;
                sql.updateForm();

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
        #endregion

        #region delete
        /// <summary>
        /// Web Method para eliminar un acopio y sus productores.
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("deleteForms")]
        [HttpPost]
        [Authorize]
        public IActionResult deleteAcopioProductores([FromBody] deleteFormsRequest request)
        {
            log.trace("deleteForms");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            deleteFormsResponse response = new deleteFormsResponse();
            try
            {
                FormsSQL sql = new FormsSQL();
                sql.formularioId = request.formularioId;
                sql.usuarioModificadorId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                sql.deleteForm();
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

        #endregion

        #region list form
        [Route("searchListForms")]
        [HttpPost]
        [Authorize]
        public IActionResult searchListForms([FromBody] searchFormsRequest request)
        {
            log.trace("searchForm");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            searchListFormResponse response = new searchListFormResponse();
            try
            {
                FormsSQL sql = new FormsSQL();
                sql.sectorId = request.sectorId ;
                response.formsDataList = sql.searchforms();
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
        #endregion
    }



}
