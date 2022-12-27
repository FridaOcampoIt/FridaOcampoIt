using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using WSTraceIT.Interfaces;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.Families;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Controllers
{
    [Route("AgroPacked")]
    [ApiController]
    public class AgroPackedController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("AgroPackedController");

		/// <summary>
		/// Web Method que sirve para realizar la búsqueda de Empacadores Agro
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchAgroPacked")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchAgroPacked([FromBody] SearchAgroPackedRequest request)
		{
			log.trace("SearchExternalPacked");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchAgroPackedResponse response = new SearchAgroPackedResponse();
			try
			{
				AgroPackedSQL sql = new AgroPackedSQL();
				sql.producerNumber = request.producerNumber.Trim();
				sql.companyId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdCompany").Value);// Compañía id del usuario logueado

				response.producerList = sql.SearchProducerList();
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

		/// <summary>
		/// Web Method para obtener el reporte de embalaje reproceso
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchReportPackagingReprocessing")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchReportPackagingReprocessing([FromBody] SearchReprocessingReportAgroPackedRequest request)
		{
			log.trace("SearchReportPackagingReprocessing");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchAgroPackedPackagingReproReportResponse response = new SearchAgroPackedPackagingReproReportResponse();
			try
			{
				AgroPackedSQL sql = new AgroPackedSQL();
				sql.producerNumber = request.producerNumber;
				sql.companyId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdCompany").Value); // compañia id del usuario logueado;
				sql.productId = request.productId;
				sql.dateStart = request.dateStart;
				sql.dateEnd = request.dateEnd;

				response = sql.SearchReprocessingPackagingReport();
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