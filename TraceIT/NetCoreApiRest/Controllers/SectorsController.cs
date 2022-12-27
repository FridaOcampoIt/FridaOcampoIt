
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using System;
using System.Linq;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.Sectors;



namespace WSTraceIT.Controllers
{
    [Route("Sectors")]
    [ApiController]

    public class SectorsController: ControllerBase
    {
        private LoggerD4 log = new LoggerD4("Sectors");


        #region save sectors
        /// <summary>
        /// Web Method para guardar acopio
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("saveSectors")]
        [HttpPost]
        [Authorize]
        public IActionResult saveSectors([FromBody] saveSectorsRequest request)
        {
            log.trace("saveSectors");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            saveSectorsResponse response = new saveSectorsResponse();
            try
            {
                SectorsSQL sql = new SectorsSQL();
                sql.sectorId = request.sectorId;
                sql.nombre = request.nombre;
                sql.descripcionCorta = request.descripcionCorta;
                sql.usuarioCreadorId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                sql.estatusId = request.estatusId;
                sql.SaveUpdateSectors();
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
        /// Web Method para obtener los acopios por compañias
        /// Desarrollador: Hernán Gómez
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("searchSectorsById")]
        [HttpPost]
        [Authorize]
        public IActionResult searchSectorsById([FromBody] searchSectorsRequest request)
        {
            log.trace("searchSectorsById");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            searchSectorsByIdResponse response = new searchSectorsByIdResponse();
            try
            {
                SectorsSQL sql = new SectorsSQL();
                sql.sectorId = request.sectorId;
                response = sql.searchSectorById();

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
        [Route("updateSectors")]
        [HttpPost]
        [Authorize]
        public IActionResult updateSectors([FromBody] updateSectorsRequest request)
        {
            log.trace("updateSectors");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            updateSectorsResponse response = new updateSectorsResponse();
            try
            {
                SectorsSQL sql = new SectorsSQL();
                sql.sectorId = request.sectorId;
                sql.nombre = request.nombre;
                sql.descripcionCorta = request.descripcionCorta;
                sql.estatusId = request.estatusId;
                sql.usuarioModificadorId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                sql.upadateSectors();
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
        [Route("deleteSectors")]
        [HttpPost]
        [Authorize]
        public IActionResult deleteSectors([FromBody] deleteSectorsRequest request)
        {
            log.trace("deleteSectors");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            deleteSectorsResponse response = new deleteSectorsResponse();
            try
            {
                SectorsSQL sql = new SectorsSQL();
                sql.sectorId = request.sectorId;
                sql.usuarioModificadorId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
                sql.deleteSectors();
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

        #region list sectors
        [Route("searchListSectors")]
        [HttpPost]
        [Authorize]
        public IActionResult searchListSectors([FromBody] searchSectorsRequest request)
        {
            log.trace("searchListSectors");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SearchSectorResponse response = new SearchSectorResponse();
            try
            {
                SectorsSQL sql = new SectorsSQL();
                sql.sectorId = request.sectorId;
                sql.nombre = request.nombre;
                response.sectorsDataList = sql.searchSectors();
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
