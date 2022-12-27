using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.Configuration;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;

namespace WSTraceIT.Controllers
{
    [Route("Tracking")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("TrackingController");

		/// <summary>
		/// Web Method que sirve para realizar la búsqueda de los rastreos de acuerdo a las opciones seleccionadas en el filtro 
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchTracking")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchTracking([FromBody] SearchTrackingRequest request)
		{
			log.trace("SearchTracking");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchTrackingResponse response = new SearchTrackingResponse();
			try {
				TrackingSQL sql = new TrackingSQL();
				sql.searchCode = request.searchCode;
				sql.searchCIUCode = request.searchCIUCode;
				sql.phase = request.phase;
                //var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
                //sql.userId = Convert.ToInt32(idUser);

                if (request.companiaId != 0)
                    if (!sql.mismaCompania(request.searchCode, request.companiaId))
                    {
						throw new Exception("El ciu pertenece a otra compañia");
                    }

                if (request.searchCode.Trim().Length < 14)
                {

					response.trackingList = sql.SearchTrackingList();
                } else 
				{
					response.trackingList = sql.SearchTackingListBox();
                }
			} catch (Exception ex) {
				log.error("There was an error " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
		}

		/// <summary>
		/// Web Method que sirve para obtener la información de un envento (movimiento) de rastreo
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("GetTrackingInfo")]
		[HttpPost]
		[Authorize]
		public IActionResult GetTrackingInfo([FromBody] SearchTrackingRequest request)
		{
			log.trace("GetTrackingInfo");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			GetTrackingInfoResponse response = new GetTrackingInfoResponse();
			try
			{
				TrackingSQL sql = new TrackingSQL();
				sql.trackingId = request.trackingId;
				sql.opc = request.opc;
				//var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
				//sql.userId = Convert.ToInt32(idUser);

				sql.GetTrackingInfo();

				if(request.opc == 1) {
					response.eventInfo = sql.eventInfo;
				}
				if (request.opc == 2) {
					response.eventSenderInfo = sql.eventSenderInfo;
				}
				if (request.opc == 3) {
					response.eventLegalInfo = sql.eventLegalInfo;
				}
				if (request.opc == 4) {
					response.eventRecipientInfo = sql.eventRecipientInfo;
				}
				if (request.opc == 5) {
					response.eventProductsInfo = sql.eventProductsInfo;
				}
				if (request.opc == 6) {
					response.eventInfo = sql.eventInfo;
					response.eventSenderInfo = sql.eventSenderInfo;
					response.eventLegalInfo = sql.eventLegalInfo;
					response.eventRecipientInfo = sql.eventRecipientInfo;
					response.eventProductsInfo = sql.eventProductsInfo;
					response.eventLegalDocsInfo = sql.eventLegalDocsInfo;

					response.eventTransportInfo = sql.eventTransportInfo;
					response.eventProdDetailInfo = sql.eventProdDetailInfo;
					response.eventTotalProdInfo = sql.eventTotalProdInfo;
					response.eventTotalPalletInfo = sql.eventTotalPalletInfo;
					response.eventTotalBoxInfo = sql.eventTotalBoxInfo;
					response.eventTotalQuantityInfo = sql.eventTotalQuantityInfo;
					response.eventTotalWeightInfo = sql.eventTotalWeightInfo;
					response.eventDateMinInfo = sql.eventDateMinInfo;
				}
			}
			catch (Exception ex)
			{
				log.error("There was an error " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
		}

		[Route("SearchLastMovement")]
		[HttpPost]
		public IActionResult searchLastMovement([FromBody] SearchMovementRequest request)
		{
			log.trace("SearchLastMovement");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchMovementResponse response = new SearchMovementResponse();
			try
            {
				TrackingSQL sql = new TrackingSQL();
				sql.searchMovementId(request.ciu, request.type);
				response.moveId = sql.movementID;
            }
            catch (Exception ex)
            {

				response.messageEsp = ex.Message;
				response.messageEng = ex.Message;
            }
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
        }

		[Route("SearchDocs")]
		[HttpPost]
		public IActionResult searchDocuments([FromBody] SearchMovementRequest request)
		{
			log.trace("SearchDocs");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			searchDocsResponse response = new searchDocsResponse();
            try
            {
				TrackingSQL sql = new TrackingSQL();
				sql.searchDocs(request.ciu, request.type, request.tipoBusqueda);
				response.docsFamilia = sql.documentosFamilias;
				response.docsStock = sql.documentoStocks;
				response.alertas = sql.listaAlertas;
				response.infoFamilia = sql.infoFamilia;

                if (response.alertas.Count > 0)
                {
                    foreach (var alerta in response.alertas)
                    {
						alerta.NombreArchivo = alerta.NombreArchivo == "" ? "" : $"AlertFiles/{alerta.NombreArchivo}";
                    }
                }

				response.messageEng = "";
				response.messageEsp = "";
            }
            catch (Exception ex)
            {

				response.messageEng = ex.Message;
				response.messageEsp = ex.Message;
                
            }

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
		}

		[Route("RegistroLog")]
		[HttpPost]
		public IActionResult RegistroLog([FromBody] LogTrackingRequest request){

			log.trace("Log Tracking");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			LogTrackingResponse response = new LogTrackingResponse();
			try
            {
				TrackingSQL sql = new TrackingSQL();
				sql.ciu = request.ciu;
				sql.lat = request.lat;
				sql.lon = request.lon;
				sql.json = request.json;
				sql.tipo = request.tipo;

				sql.registroLog();

			}
            catch (Exception ex)
            {
				response.messageEng = ex.Message;
				response.messageEsp = ex.Message;
            }

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
        }

		[Route("RegistroRecepcion")]
		[HttpPost]
		public IActionResult RegistroRecepcion([FromBody] RecepcionRequest request)
		{
			log.trace("Log RegistroRecepcion");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			LogTrackingResponse response = new LogTrackingResponse();
			try
            {
				TrackingSQL sql = new TrackingSQL();
				sql.movementID = request.movimientoId;
				sql.nombre = request.nombre;
				sql.apellido = request.apellido;
				sql.cargo = request.cargo;
				sql.fecha = request.fecha;
				sql.lat = request.lat;
				sql.lon = request.lon;

				sql.recepcionTracking();

			}
            catch (Exception ex)
            {
				response.messageEng = ex.Message;
				response.messageEsp = ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
        }
	}
}