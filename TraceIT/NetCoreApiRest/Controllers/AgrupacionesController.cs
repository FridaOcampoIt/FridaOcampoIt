using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreApiRest.Utils;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WSTraceIT.Controllers
{

    [Route("Agrupaciones")]
    [ApiController]
    public class AgrupacionesController : ControllerBase
    {
        LoggerD4 log = new LoggerD4("AgrupacionesController");

        #region Busqueda para fichas de agrupacion
        /// <summary>
        /// Web Method para buscar un movimiento general
        /// Desarrollador: Javier Ramirez
        /// </summary>
        /// <param nombreAgrupacion="request"></param>
        /// <returns></returns>
        [Route("searchDataFicha")]
        [HttpPost]
        /*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
        [Authorize]
        public IActionResult searchDataFicha([FromBody] SearchDataFichaRequest request)
        {
            log.trace("searchDataFicha");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SearchDataFichaResponse response = new SearchDataFichaResponse();
            try
            {
                AgrupacionSQL sql = new AgrupacionSQL();
                sql.codigo = request.codigo;
                sql.usuario = request.usuario;

                sql.SearchFicha();
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
        /// Web Method para buscar un movimiento general
        /// Desarrollador: Javier Ramirez
        /// </summary>
        /// <param nombreAgrupacion="request"></param>
        /// <returns></returns>
        [Route("searchDataFicha2")]
        [HttpPost]
        /*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
        [Authorize]
        public IActionResult searchDataFicha2([FromBody] SearchDataFichaRequest request)
        {
            log.trace("searchDataFicha2");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SearchDataFichaResponse2 response = new SearchDataFichaResponse2();
            try
            {
                AgrupacionSQL sql = new AgrupacionSQL();
                sql.codigo = request.codigo;
                sql.usuario = request.usuario;

                response.fichasDataList = sql.SearchFicha2();
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

        #region Insersion de datos a tabla temporal de fichas seleccionadas
        /// <summary>
		/// Web Method para guardar los productos
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveFichaTemp")]
        [HttpPost]
        [Authorize]
        public IActionResult saveFichaTemp([FromBody] SaveFichaTempRequest request)
        {
            log.trace("saveFichaTemp");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SaveFichaTempResponse response = new SaveFichaTempResponse();
            try
            {
                AgrupacionSQL sql = new AgrupacionSQL();
                sql.fichaId = request.fichaId;
                sql.tipoFicha = request.tipoFicha;
                sql.usuario = request.usuario;
                sql.desconocidoId = request.desconocidoId;

                sql.SaveFichaTemporal();
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

        #region Busqueda de todas las fichas seleciconadas para reagruparlas
        /// <summary>
        /// Web Method para buscar las fichas ya selecionadas
        /// Desarrollador: Javier Ramirez
        /// </summary>
        /// <param nombreAgrupacion="request"></param>
        /// <returns></returns>
        [Route("searchDataFichasSeleccionadas")]
        [HttpPost]
        /*[Authorize(Policy = ConstantsPermission.moduloMovimientos)]*/
        [Authorize]
        public IActionResult searchDataFichasSeleccionadas([FromBody] SearchDataFichasSeleccionadasRequest request)
        {
            log.trace("searchDataFichasSeleccionadas");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SearchDataFichasSeleccionadasResponse response = new SearchDataFichasSeleccionadasResponse();
            try
            {
                AgrupacionSQL sql = new AgrupacionSQL();
                sql.usuario = request.usuario;

                FichasSeleccionadasData respSQL = sql.SearchFichasSeleccionadas();
                response.fichasSeleccionadasDataList = respSQL.fichasSeleccionadasDataList;
                response.fichasDesconocidasDataList = respSQL.fichasDesconocidasDataList;
                response.fichasUnicasDataList = respSQL.fichasUnicasDataList;
                response.fichasMovimientosDataList = respSQL.fichasMovimientosDataList;

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

        /// <summary>
        /// Web Method para buscar los tipos de movimientos y productos para el combo del catalogo movimientos
        /// Desarrollador: Javier Ramirez
        /// </summary>
        /// <param nombreAgrupacion="request"></param>
        /// <returns></returns>

        #region Eliminar una de las fichas seleccionadas para ser reagrupadas
        /// <summary>
		/// Web Method para eliminar una ficha seleccionada
		/// Desarrollador: Javier Ramirez
		/// </summary>
		/// <returns></returns>
		[Route("deleteFichaSeleccionada")]
        [HttpPost]
        [Authorize]
        public IActionResult deleteFichaSeleccionada([FromBody] DeleteFichaSeleccionadasRequest request)
        {
            log.trace("deleteFichaSeleccionada");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            DeleteFichaSeleccionadaResponse response = new DeleteFichaSeleccionadaResponse();
            try
            {
                AgrupacionSQL sql = new AgrupacionSQL();
                sql.movFichaId = request.movfichaId;

                sql.DeleteFichaSeleccionada();
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

        #region Insersion de datos de producto desconocido
        /// <summary>
        /// Web Method para guardar un producto desconocido
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("saveProdDesc")]
        [HttpPost]
        [Authorize]
        public IActionResult saveProdDesc([FromBody] SaveProdDescRequest request)
        {
            log.trace("saveProdDesc");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            SaveProdDescResponse response = new SaveProdDescResponse();
            try
            {
                AgrupacionSQL sql = new AgrupacionSQL();
                sql.nombre = request.nombre;
                sql.lote = request.lote;
                sql.cantidad = request.cantidad;
                sql.fechaCaducidad = request.fechaCaducidad;
                sql.numSerie = request.numSerie;
                sql.codigo = request.codigo;
                sql.usuario = request.usuario;

                sql.SaveProductoDesconocido();
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
