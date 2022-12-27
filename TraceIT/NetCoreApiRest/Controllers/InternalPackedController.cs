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
using System.Collections.Generic;

namespace WSTraceIT.Controllers
{
    [Route("InternalPacked")]
    [ApiController]
    public class InternalPackedController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("InternalPackedController");

		/// <summary>
		/// Web Method que sirve para realizar la búsqueda de Empacadores 
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchInternalPacked")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchInternalPacked([FromBody] SearchInternalPackedRequest request)
		{
			log.trace("SearchInternalPacked");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchInternalPackedResponse response = new SearchInternalPackedResponse();
			try
			{
				InternalPackedSQL sql = new InternalPackedSQL();
				//Nobre de emacador
				sql.packedName = request.packedName;
				
				//Aparentemente solo para verificar que lleva filtos, (No es necesario este valor) 
				//sql.opc = request.opc;

				//Tipo de empacador (Interno/Externo)  1 = Externo / 2 = Interno (No somos adivinos) 
				sql.typeUsuario = request.type;

				//El valor del usuario que lo registro, ya no es necesario
				//var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
				
				//Mandamos la compañia que se recibio del front (Y por coicidente, usamos el usuario que esta logeado) 
				var companiaId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdCompany").Value);

				//Se usa esta valor, para traer el dato esclusivo de empacadores de una compañia
				sql.companyId = companiaId;

				//Agregamos un valor compañia para busqueda
				sql.companyIdSearch = request.companyIdSearch;

				//Mandamos el usuario logeado 
				var usuarioId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
				sql.userId = usuarioId;

				//validar si el usuario es administrador o consultor
				//si el usuario es administrador, se manda el id del usuario logueado
				//si el usuario es consultor, se manda un 0 como idusuario logueado
				//sql.userId = Convert.ToInt32(idUser);  //Valor de verdad no necesario

				response.packedList = sql.SearchPackedList();
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
		/// Web Method que sirve para realizar la búsqueda de Empacadores por compañia, para ligarlo a familia
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchInternalExternalPacked")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchInternalExternalPacked([FromBody] SearchInternalExternalPackedRequest request)
		{
			log.trace("SearchInternalPacked");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchInternalExternalPackedListResponse response = new SearchInternalExternalPackedListResponse();
			try
			{
				InternalExternalPackedSQL sql = new InternalExternalPackedSQL();
				//idDecompañia
				sql.companyId = request.companyId;

				response.packedList = sql.SearchInternalExternalPackedList();
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
		/// Web Method que sirve para obtener los datos de un Empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchPackedData")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchPackedData([FromBody] SearchInternalPackedRequest request)
		{
			log.trace("SearchInternalPackedData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchInternalPackedResponse response = new SearchInternalPackedResponse();
			try
			{
				InternalPackedSQL sql = new InternalPackedSQL();
				sql.packedId = request.packedId;
				sql.companyId = request.companyId;
				response.packedList = sql.SearchPackedData();
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
		/// Web Method que sirve para guardar / actualizar un Empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SavePacked")]
		[HttpPost]
		[Authorize]
		public IActionResult SavePacked([FromBody] SaveInternalPackedRequest request)
		{
			log.trace("SaveInternalPacked");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveInternalPackedResponse response = new SaveInternalPackedResponse();
			try
			{
				if (request.packedData.packedNumber.Trim() == "" || request.packedData.packedName.Trim() == "") {
					response.messageEng = "Some fields are required";
					response.messageEsp = "Algunos campos son obligatorios";
				} else if (request.packedData.packedId == 0 && request.packedData.password.Trim() == "") {
					response.messageEng = "The password field is required";
					response.messageEsp = "El campo contraseña es requerido";
				} else {
					InternalPackedSQL sql = new InternalPackedSQL();
					sql.packedId = request.packedData.packedId;
					sql.packedNumber = request.packedData.packedNumber;
					sql.packedName = request.packedData.packedName;
					sql.merma = request.packedData.merma;
					sql.email = request.packedData.email;
					sql.password = request.packedData.password;
					sql.phone = request.packedData.phone;
					sql.status = request.packedData.status;
					var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
					sql.userId = Convert.ToInt32(idUser);
					sql.companyId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdCompany").Value); // compañia id del usuario logueado
					sql.companiasId = request.packedData.companiasId;
					sql.auxGetCompany = request.packedData.auxGetCompany;
					sql.sp = sql.packedId == 0 ? "spi_guardarEmpacadorEmpaqEtiq" : "spu_edicionEmpacadorEmpaqEtiq";
					sql.SavePacked();
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

		/// <summary>
		/// Web Method para eliminar un Empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deletePacked")]
		[HttpPost]
		[Authorize]
		public IActionResult deletePacked([FromBody] SearchInternalPackedRequest request)
		{
			log.trace("deleteInternalPacked");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteInternalPackedResponse response = new DeleteInternalPackedResponse();
			try
			{
				InternalPackedSQL sql = new InternalPackedSQL();
				sql.packedId = request.packedId;

				sql.DeletePacked();
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
		/// Web Method que sirve para realizar la búsqueda de Operadores de un Empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchPackedOperators")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchPackedOperators([FromBody] SearchInternalPackedRequest request)
		{
			log.trace("SearchPackedOperators");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchIntPackedOperatorResponse response = new SearchIntPackedOperatorResponse();
			try {
				InternalPackedOperatorSQL sql = new InternalPackedOperatorSQL();
				sql.packedId = request.packedId;
				sql.addressId = request.addressId;
				sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IsType").Value) == 0 ?
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value) :
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "eUser").Value);

				//response.operatorList = sql.SearchPackedOperatorsList();
				response.operatorList = (from tsd in sql.SearchPackedOperatorsList()
										 select new InternalPackedOperatorDataSQL
										 {
											 operatorId = tsd.operatorId,
											 code = tsd.code,
											 name = tsd.name,
											 image = tsd.image == null || tsd.image == "" ? "" : ConfigurationSite._cofiguration["Paths:urlImagesOperatorsEmpEtq"] + "2/" + tsd.image,
											 address = tsd.address,
											 addressId = tsd.addressId,
											 packedId = tsd.packedId
										 }).ToList();
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
		/// Web Method que sirve para obtener los datos para un Operador de un Empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchPackedOperatorsData")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchPackedOperatorsData([FromBody] SearchInternalPackedRequest request)
		{
			log.trace("SearchPackedOperatorsData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchIntPackedOperatorResponse response = new SearchIntPackedOperatorResponse();
			try
			{
				InternalPackedOperatorSQL sql = new InternalPackedOperatorSQL();
				sql.operatorId = request.operatorId;
				sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);

				response.operatorList = sql.SearchPackedOperatorData();
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
		/// Web Method que sirve para guardar / actualizar un Operador de un Empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SavePackedOperator")]
		[HttpPost]
		[Authorize]
		public IActionResult SavePackedOperator([FromBody] SaveInternalPackedOperatorRequest request)
		{
			log.trace("SavePackedOperator");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveInternalPackedResponse response = new SaveInternalPackedResponse();
			ArchivesData archives = new ArchivesData();
			try
			{
				if (request.operatorData.code.Trim() == "") {
					response.messageEng = "The code field is required";
					response.messageEsp = "El campo código es requerido";
				} else if (request.operatorData.name.Trim() == "") {
					response.messageEng = "The name field is required";
					response.messageEsp = "El campo nombre es requerido";
				} else {
					InternalPackedOperatorSQL sql = new InternalPackedOperatorSQL();
					sql.operatorId = request.operatorData.operatorId;
					sql.code = request.operatorData.code.Trim();
					sql.name = request.operatorData.name.Trim();
					sql.addressId = request.operatorData.addressId;
					sql.packedId = request.operatorData.packedId;
					sql.companyId = request.operatorData.companyId;
					sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IsType").Value) == 0 ?
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value) :
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "eUser").Value);// id del usuario logueado
					sql.sp = sql.operatorId == 0 ? "spi_guardarOperadorEmpaqEtiq" : "spu_edicionOperadorEmpaqEtiq";

					if (request.operatorData.image != null && request.operatorData.image != "") {
						//Se guarda la imagen de la familia del producto
						archives.archiveName = DateTime.Now.ToString("ddMMyyyyHHmmss") + ".png";
						archives.base64 = request.operatorData.image;
						request.operatorData.image = archives.archiveName;
					}
					sql.image = request.operatorData.image;

					sql.SavePackedOperator();

					if (archives.base64 != "" && archives.base64 != null) {
						ProcesarImagen image = new ProcesarImagen();
						image.SaveTmbn(archives.base64, archives.archiveName, ConfigurationSite._cofiguration["Paths:pathImagesOperatorsEmpEtq"] + "2/");
					}
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

		/// <summary>
		/// Web Method para eliminar un Operador de empacador interno
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deletePackedOperator")]
		[HttpPost]
		[Authorize]
		public IActionResult deletePackedOperator([FromBody] SearchInternalPackedRequest request)
		{
			log.trace("deletePackedOperator");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteInternalPackedResponse response = new DeleteInternalPackedResponse();
			try
			{
				InternalPackedOperatorSQL sql = new InternalPackedOperatorSQL();
				sql.operatorId = request.operatorId;
				sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IsType").Value) == 0 ?
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value) :
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "eUser").Value);// id del usuario logueado

				sql.DeletePackedOperator();
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
		/// Web Method que sirve para realizar la búsqueda de Lineas de producción de un Empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchPackedProdLines")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchPackedProdLines([FromBody] SearchInternalPackedRequest request)
		{
			log.trace("SearchPackedProdLines");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchIntPackedProdLineResponse response = new SearchIntPackedProdLineResponse();
			try {
				InternalPackedProductionLineSQL sql = new InternalPackedProductionLineSQL();
				sql.packedId = request.packedId;
				sql.addressId = request.addressId;
				sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IsType").Value) == 0 ?
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value) :
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "eUser").Value);

				response.prodLineList = sql.SearchPackedProdLinesList();
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
		/// Web Method que sirve para obtener los datos para una Linea de producción de un Empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchPackedProdLinesData")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchPackedProdLinesData([FromBody] SearchInternalPackedRequest request)
		{
			log.trace("SearchPackedProdLinesData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchIntPackedProdLineResponse response = new SearchIntPackedProdLineResponse();
			try
			{
				InternalPackedProductionLineSQL sql = new InternalPackedProductionLineSQL();
				sql.lineId = request.lineId;
				sql.packedId = request.packedId;
				sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);

				response.prodLineList = sql.SearchPackedProdLineData();
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
		/// Web Method que sirve para guardar / actualizar una Linea de producción de un Empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SavePackedProdLine")]
		[HttpPost]
		[Authorize]
		public IActionResult SavePackedProdLine([FromBody] SaveInternalPackedProdLineRequest request)
		{
			log.trace("SavePackedProdLine");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveInternalPackedResponse response = new SaveInternalPackedResponse();
			try
			{
				if (request.prodLineData.name.Trim() == "") {
					response.messageEng = "The name field is required";
					response.messageEsp = "El campo nombre es requerido";
				} else {
					InternalPackedProductionLineSQL sql = new InternalPackedProductionLineSQL();
					sql.lineId = request.prodLineData.lineId;
					sql.name = request.prodLineData.name;
					sql.packedId = request.prodLineData.packedId;
					sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IsType").Value) == 0 ?
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value) :
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "eUser").Value);// id del usuario logueado
					sql.sp = sql.lineId == 0 ? "spi_guardarLineaProduccionEmpaqEtiq" : "spu_edicionLineaProduccionEmpaqEtiq";

					sql.SavePackedProdLine();
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

		/// <summary>
		/// Web Method para eliminar una Linea de producción de un Empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deletePackedProdLine")]
		[HttpPost]
		[Authorize]
		public IActionResult deletePackedProdLine([FromBody] SearchInternalPackedRequest request)
		{
			log.trace("deletePackedProdLine");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteInternalPackedResponse response = new DeleteInternalPackedResponse();
			try
			{
				InternalPackedProductionLineSQL sql = new InternalPackedProductionLineSQL();
				sql.lineId = request.lineId;
				sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IsType").Value) == 0 ?
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value) :
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "eUser").Value);// id del usuario logueado

				sql.DeletePackedProdLine();
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
		/// Web Method para obtener los registros para la gestión de cajas
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchBoxManagement")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchBoxManagement([FromBody] SearchBoxManagIntPackedRequest request)
		{
			log.trace("SearchBoxManagement");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchIntPackedBoxManagementResponse response = new SearchIntPackedBoxManagementResponse();
			try
			{
				InternalPackedBoxManagementSQL sql = new InternalPackedBoxManagementSQL();
				sql.packedId = request.packedId;
				sql.productId = request.productId;
				sql.typeView = request.typeView;
				sql.dateStart = request.dateStart;
				sql.dateEnd = request.dateEnd;
				sql.companiaId = request.companiaId;
				sql.searchfield = request.searchfield;
				sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdCompany").Value);// id del usuario logueado
																										   
				response.infoData = sql.SearchPackedBoxManagement();
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
		/// Web Method para obtener el detalle de una agrupación (operación)
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchBoxManagementDetail")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchBoxManagementDetail([FromBody] SearchBoxManagIntPackedRequest request)
		{
			log.trace("SearchBoxManagementDetail");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchIntPackedBoxManagementDetailResponse response = new SearchIntPackedBoxManagementDetailResponse();
			try
			{
				InternalPackedBoxManagementSQL sql = new InternalPackedBoxManagementSQL();
				sql.groupingId = request.groupingId;
				sql.pallet = request.pallet;
				sql.box = request.box;
				sql.typeView = request.typeView;
				sql.productId = request.productId;
				sql.SearchPackedBoxManagementDetail(response);
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
		/// Web Method para obtener el reporte de armados
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchArmingReport")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchArmingReport([FromBody] SearchArmingReportIntPackedRequest request)
		{
			log.trace("SearchArmingReport");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchIntPackedArmingReportResponse response = new SearchIntPackedArmingReportResponse();
			try
			{
				InternalPackedArmingReportSQL sql = new InternalPackedArmingReportSQL();
				sql.typeCompany = request.typeCompany;
				sql.companyId = request.companyId > 0 ? request.companyId : Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdCompany").Value); // compañia id del usuario logueado;
				sql.addressId = request.addressId;
				sql.productId = request.productId;
				sql.searchGeneric = request.searchGeneric;
				sql.dateStart = request.dateStart;
				sql.dateEnd = request.dateEnd;

				response = sql.SearchArmingReport();
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
		/// Web Method para associar productos
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SaveAssociateProducts")]
		[HttpPost]
		[Authorize]
		public IActionResult SaveAssociateProducts([FromBody] SaveProductInternalPackedRequest request)
		{
			log.trace("SaveAssociateProducts");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveInternalPackedResponse response = new SaveInternalPackedResponse();
			try
			{
				ProductInternalPackedSQL sql = new ProductInternalPackedSQL();
				sql.products = request.productData;

				sql.SaveProduct();
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
		/// Web Method para obtener el listado de productos asociados
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchAssociateProducts")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchAssociateProducts([FromBody] SearchProductInternalPackedRequest request)
		{
			log.trace("SearchAssociateProducts");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchProductInternalPackedResponse response = new SearchProductInternalPackedResponse();
			try
			{
				ProductInternalPackedSQL sql = new ProductInternalPackedSQL();
				sql.packedId = request.packedId;
				sql.productId = request.productId;
				sql.companyId = request.companyId;
				sql.packagingId = request.packagingId;
				sql.rawMaterial = request.rawMaterial;
				sql.topc = request.topc;

				response.productData = sql.SearchProductPacked();
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