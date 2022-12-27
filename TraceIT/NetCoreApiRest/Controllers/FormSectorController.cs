using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using System;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.Forms;
using WSTraceIT.Models.Base.FormSector;

namespace WSTraceIT.Controllers
{
    public class FormSectorController
    {
        [Route("FormSector")]
        [ApiController]
        public class FormsController : ControllerBase
        {
            private LoggerD4 log = new LoggerD4("FormSector");

            #region save
            /// <summary>
            /// Web Method para guardar Formulario sector
            /// Desarrollador: Hernán Gómez
            /// </summary>
            /// <param name="request"></param>
            /// <returns></returns>
            [Route("saveFormSector")]
            [HttpPost]
            [Authorize]
            public IActionResult saveFormSector([FromBody] saveFormSectorRequest request)
            {
                log.trace("saveFormSector");
                log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
                saveFormSectorResponse response = new saveFormSectorResponse();
                try
                {
                    FormSectorSQL sql = new FormSectorSQL();
                    sql.formularioSectorId = request.formularioSectorId;
                    sql.sectorId = request.sectorId;
                    sql.formularioId = request.formularioId;
                    
                    sql.SaveFormSector();
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
            /// Web Method para obtener el detalle de formulario sector
            /// Desarrollador: Roberto Ortega
            /// </summary>
            /// <param name="request"></param>
            /// <returns></returns>
            [Route("searchFormSectorById")]
            [HttpPost]
            [Authorize]
            public IActionResult searchFormsById([FromBody] searchFormsRequest request)
            {
                log.trace("searchFormSectorById");
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
            /// Web Method para actualizar formulario sector
            /// Desarrollador: Roberto Ortega
            /// </summary>
            /// <param name="request"></param>
            /// <returns></returns>
            [Route("updateFormSector")]
            [HttpPost]
            [Authorize]
            public IActionResult updateFormSector([FromBody] updateFormSectorRequest request)
            {
                log.trace("updateFormSector");
                log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
                updateFormSectorResponse response = new updateFormSectorResponse();
                try
                {
                    FormSectorSQL sql = new FormSectorSQL();
                    sql.formularioSectorId = request.formularioSectorId;
                    sql.sectorId = request.sectorId;
                    sql.formularioId = request.formularioId;
                    sql.updateFormSector();
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
            /// Web Method para eliminar formulario sector
            /// Desarrollador: Roberto Ortega
            /// </summary>
            /// <param name="request"></param>
            /// <returns></returns>
            [Route("deleteFormSector")]
            [HttpPost]
            [Authorize]
            public IActionResult deleteFormSector([FromBody] deleteFormSectorRequest request)
            {
                log.trace("deleteFormSector");
                log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
                deleteFormSectorResponse response = new deleteFormSectorResponse();
                try
                {
                    FormSectorSQL sql = new FormSectorSQL();
                    sql.formularioSectorId = request.formularioSectorId;
                    sql.deleteFormSector();
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
}
