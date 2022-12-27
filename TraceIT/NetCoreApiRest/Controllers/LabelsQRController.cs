using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using WSTraceIT.Interfaces;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.Configuration;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;

namespace WSTraceIT.Controllers
{
    [Route("LabelsQR")]
    [ApiController]
    public class LabelsQRController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("LabelsQRController");

		/// <summary>
		/// Web Method que sirve para realizar la búsqueda etiquetas qr
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchLabels")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchLabels([FromBody] SearchLabelsQRRequest request)
		{
			log.trace("SearchLabels");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchLabelsQRResponse response = new SearchLabelsQRResponse();
			try {
				LabelsQRSQL sql = new LabelsQRSQL();
				sql.name = request.name;
				sql.opc = request.opc;
				//var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
				//sql.userId = Convert.ToInt32(idUser);

				response.labelsqr = sql.SearchLabelsQR();
			} catch (Exception ex) {
				log.error("There was an error " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method que sirve para obtener la lista de etiquetas qr para combos
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchLabelsCombo")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchLabelsCombo([FromBody] SearchLabelsQRRequest request)
		{
			log.trace("SearchLabelsCombo");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchLabelsQRComboResponse response = new SearchLabelsQRComboResponse();
			try
			{
				LabelsQRSQL sql = new LabelsQRSQL();
				sql.name = request.name;
				sql.opc = request.opc;
				//var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
				//sql.userId = Convert.ToInt32(idUser);

				response.labelsqrcombo = sql.SearchLabelsQRCombo();
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
		/// Web Method que sirve para obtener los datos de una etiqueta qr
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchLabelsData")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchLabelsData([FromBody] SearchLabelsQRRequest request)
		{
			log.trace("SearchLabelsData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchLabelsQRResponse response = new SearchLabelsQRResponse();
			try {
				LabelsQRSQL sql = new LabelsQRSQL();
				sql.labelId = request.labelId;
				
				response.labelsqr = sql.SearchLabelQRData();
			} catch (Exception ex) {
				log.error("There was an error " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method que sirve para guardar / actualizar una etiqueta qr
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SaveLabelQR")]
		[HttpPost]
		[Authorize]
		public IActionResult SaveLabelQR([FromBody] SaveLabelsQRRequest request)
		{
			log.trace("SaveLabelQR");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveLabelsQRResponse response = new SaveLabelsQRResponse();
			try {
				if (request.labelqrData.name.Trim() == "") {
					response.messageEng = "The field name is required";
					response.messageEsp = "El campo nombre es obligatorio";
				} else {
					LabelsQRSQL sql = new LabelsQRSQL();
					sql.labelId = request.labelqrData.labelId;
					sql.name = request.labelqrData.name;
					sql.orientation = request.labelqrData.orientation;
					sql.grouper = request.labelqrData.grouper;
					sql.nChildren = request.labelqrData.nChildren;
					sql.topPrimary = request.labelqrData.topPrimary;
					sql.topSecondary = request.labelqrData.topSecondary;
					sql.rightPrimary = request.labelqrData.rightPrimary;
					sql.rightSecondary = request.labelqrData.rightSecondary;
					sql.bottomPrimary = request.labelqrData.bottomPrimary;
					sql.bottomSecondary = request.labelqrData.bottomSecondary;
					sql.leftPrimary = request.labelqrData.leftPrimary;
					sql.leftSecondary = request.labelqrData.leftSecondary;
					sql.companyId = request.labelqrData.companyId;
					//sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);// id del usuario logueado
					sql.sp = sql.labelId == 0 ? "spi_guardarEtiquetaQR" : "spu_edicionEtiquetaQR";

					sql.SaveLabelQR();
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
		/// Web Method para eliminar una etiqueta qr
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteLabelQR")]
		[HttpPost]
		[Authorize]
		public IActionResult deleteLabelQR([FromBody] SearchLabelsQRRequest request)
		{
			log.trace("deleteLabelQR");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveLabelsQRResponse response = new SaveLabelsQRResponse();
			try
			{
				LabelsQRSQL sql = new LabelsQRSQL();
				sql.labelId = request.labelId;

				sql.DeleteLabelQR();
			} catch (Exception ex) {
				log.error("Exception: " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method que sirve para obtener el base64 de una etiqueta qr
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("GetLabelImageData")]
		[HttpPost]
		[Authorize]
		public IActionResult GetLabelImageData([FromBody] SearchLabelsQRRequest request)
		{
			log.trace("GetLabelImageData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchLabelsQRResponse labeldata = new SearchLabelsQRResponse();
			DataLabelQRCodeResponse response = new DataLabelQRCodeResponse();
			try
			{
				LabelsQRSQL sql = new LabelsQRSQL();
				sql.labelId = request.labelId;

				labeldata.labelsqr = sql.SearchLabelQRData();

				QRLabel qrlabel = new QRLabel();
				qrlabel.code = (request.code != "" ? request.code : (labeldata.labelsqr.Count > 0 ? labeldata.labelsqr.First().name : ""));
				qrlabel.Height = 250;
				qrlabel.Width = 250;
				//response.basedata = qrlabel.SimpleDataMatrix();
				response.basedata = qrlabel.SimpleQRCode();
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
	}
}