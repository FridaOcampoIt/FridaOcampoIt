using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using System;
using System.Linq;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.CategoriesForms;
using WSTraceIT.Models.Base.Forms;
using WSTraceIT.Models.Base.FormSector;

namespace WSTraceIT.Controllers
{
    public class CategoriesFormController 
    {
            [Route("categoriesForm")]
            [ApiController]
            public class FormsController : ControllerBase
            {
                private LoggerD4 log = new LoggerD4("categoriesForm");

                #region save
                /// <summary>
                /// Web Method para guardar Formulario sector
                /// Desarrollador: Hernán Gómez
                /// </summary>
                /// <param name="request"></param>
                /// <returns></returns>
                [Route("saveCategoriesForm")]
                [HttpPost]
                [Authorize]
                public IActionResult saveCategoriesForm([FromBody] saveCategoriesFormsRequest request)
                {
                    log.trace("saveCategoriesForm");
                    log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
                    saveCategoriesFormsResponse response = new saveCategoriesFormsResponse();
                    try
                    {
                        CategoriesFormSQL sql = new CategoriesFormSQL();
        
                     sql.categoriaFormularioId = request.categoriaFormularioId;
                     sql.formularioId = request.formularioId;
                     sql.nombreCategoria = request.nombreCategoria;
                     sql.usuarioCreadorId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                
                    sql.SaveCategoriesForm();
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
                [Route("searchCategoriesFormsById")]
                [HttpPost]
                [Authorize]
                public IActionResult searchCategoriesFormsById([FromBody] searchCategoriesFormsByIdRequest request)
                {
                    log.trace("searchFormSectorById");
                    log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
                    searchCategoriesFormsByIdResponse response = new searchCategoriesFormsByIdResponse();
                    try
                    {
                        CategoriesFormSQL sql = new CategoriesFormSQL();
                        sql.categoriaFormularioId = request.categoriaFormularioId;
                        response = sql.searchCategoriesFormById();
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
                [Route("updateCategoriesForms")]
                [HttpPost]
                [Authorize]
                public IActionResult updateCategoriesForms([FromBody] updateCategoriesFormsRequest request)
                {
                    log.trace("updateCategoriesForms");
                    log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
                    updateCategoriesFormsResponse response = new updateCategoriesFormsResponse();
                    try
                    {
                        CategoriesFormSQL sql = new CategoriesFormSQL();
                        sql.categoriaFormularioId = request.categoriaFormularioId;
                        sql.formularioId = request.formularioId;
                        sql.nombreCategoria = request.nombreCategoria;
                        sql.usuarioCreadorId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                        sql.fechaCreacion = request.fechaCreacion;
                        sql.usuarioModificadorId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                        sql.fechaModificacion = request.fechaModificacion;
                        sql.estatusCategoriaId = request.estatusCategoriaId;

                        sql.upadateCategoriesForm();
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
