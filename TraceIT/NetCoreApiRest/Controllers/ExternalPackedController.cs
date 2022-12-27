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
    [Route("ExternalPacked")]
    [ApiController]
    public class ExternalPackedController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("ExternalPackedController");

		/// <summary>
		/// Web Method que sirve para realizar la búsqueda de Empacadores 
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchExternalPacked")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchExternalPacked([FromBody] SearchExternalPackedRequest request)
		{
			log.trace("SearchExternalPacked");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchExternalPackedResponse response = new SearchExternalPackedResponse();
			try
			{
				ExternalPackedSQL sql = new ExternalPackedSQL();
				//Nobre de emacador
				sql.packedName = request.packedName;

				//Tipo de empacador (Interno/Externo)  1 = Externo / 2 = Interno (No somos adivinos) 
				sql.typeUsuario = request.type;

				//Mandamos la compañia que se recibio del front (Y por coicidente, usamos el usuario que esta logeado) 
				var companiaId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdCompany").Value);


				//Mandamos el usuario logeado 
				var usuarioId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
				sql.userId = usuarioId;

				//Se usa esta valor, para traer el dato esclusivo de empacadores de una compañia
				sql.companyId = companiaId;

				//Agregamos un valor compañia para busqueda
				sql.companyIdSearch = request.companyIdSearch;

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
		/// Web Method que sirve para obtener los datos de un Empacador
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchPackedData")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchPackedData([FromBody] SearchExternalPackedRequest request)
		{
			log.trace("SearchExternalPackedData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchExternalPackedResponse response = new SearchExternalPackedResponse();
			try
			{
				ExternalPackedSQL sql = new ExternalPackedSQL();
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
		public IActionResult SavePacked([FromBody] SaveExternalPackedRequest request)
		{
			log.trace("SaveExternalPacked");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveExternalPackedResponse response = new SaveExternalPackedResponse();
			try
			{
				if (request.packedData.packedNumber.Trim() == "" || request.packedData.packedName.Trim() == "") {
					response.messageEng = "Some fields are required";
					response.messageEsp = "Algunos campos son obligatorios";
				} else if (request.packedData.packedId == 0  && request.packedData.password.Trim() == "") {
					response.messageEng = "The password field is required";
					response.messageEsp = "El campo contraseña es requerido";
				} else {
					ExternalPackedSQL sql = new ExternalPackedSQL();
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
		public IActionResult deletePacked([FromBody] SearchExternalPackedRequest request)
		{
			log.trace("deleteExternalPacked");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteExternalPackedResponse response = new DeleteExternalPackedResponse();
			try
			{
				ExternalPackedSQL sql = new ExternalPackedSQL();
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
		public IActionResult SearchPackedOperators([FromBody] SearchExternalPackedRequest request)
		{
			log.trace("SearchPackedOperators");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchExtPackedOperatorResponse response = new SearchExtPackedOperatorResponse();
			try
			{
				ExternalPackedOperatorSQL sql = new ExternalPackedOperatorSQL();
				sql.packedId = request.packedId;
				sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IsType").Value) == 0 ? 
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value) : 
							 Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "eUser").Value);

				//response.operatorList = sql.SearchPackedOperatorsList();
				response.operatorList  = (from tsd in sql.SearchPackedOperatorsList()
							  select new ExternalPackedOperatorDataSQL
							  {
								  operatorId = tsd.operatorId,
								  code = tsd.code,
								  name = tsd.name,
								  image = tsd.image == null || tsd.image == "" ? "" : ConfigurationSite._cofiguration["Paths:urlImagesOperatorsEmpEtq"] + "1/" + tsd.image,
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
		public IActionResult SearchPackedOperatorsData([FromBody] SearchExternalPackedRequest request)
		{
			log.trace("SearchPackedOperatorsData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchExtPackedOperatorResponse response = new SearchExtPackedOperatorResponse();
			try
			{
				ExternalPackedOperatorSQL sql = new ExternalPackedOperatorSQL();
				sql.operatorId = request.operatorId;
				sql.packedId = request.packedId;
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
		public IActionResult SavePackedOperator([FromBody] SaveExternalPackedOperatorRequest request)
		{
			log.trace("SavePackedOperator");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveExternalPackedResponse response = new SaveExternalPackedResponse();
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
					ExternalPackedOperatorSQL sql = new ExternalPackedOperatorSQL();
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
						image.SaveTmbn(archives.base64, archives.archiveName, ConfigurationSite._cofiguration["Paths:pathImagesOperatorsEmpEtq"] + "1/");
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
		/// Web Method para eliminar un Operador de empacador externo
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deletePackedOperator")]
		[HttpPost]
		[Authorize]
		public IActionResult deletePackedOperator([FromBody] SearchExternalPackedRequest request)
		{
			log.trace("deletePackedOperator");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteExternalPackedResponse response = new DeleteExternalPackedResponse();
			try
			{
				ExternalPackedOperatorSQL sql = new ExternalPackedOperatorSQL();
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
		public IActionResult SearchPackedProdLines([FromBody] SearchExternalPackedRequest request)
		{
			log.trace("SearchPackedProdLines");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchExtPackedProdLineResponse response = new SearchExtPackedProdLineResponse();
			try {
				ExternalPackedProductionLineSQL sql = new ExternalPackedProductionLineSQL();
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
		public IActionResult SearchPackedProdLinesData([FromBody] SearchExternalPackedRequest request)
		{
			log.trace("SearchPackedProdLinesData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchExtPackedProdLineResponse response = new SearchExtPackedProdLineResponse();
			try
			{
				ExternalPackedProductionLineSQL sql = new ExternalPackedProductionLineSQL();
				sql.lineId = request.lineId;
				sql.packedId = request.packedId;

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
		public IActionResult SavePackedProdLine([FromBody] SaveExternalPackedProdLineRequest request)
		{
			log.trace("SavePackedProdLine");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveExternalPackedResponse response = new SaveExternalPackedResponse();
			try
			{
				if (request.prodLineData.name.Trim() == "") {
					response.messageEng = "The name field is required";
					response.messageEsp = "El campo nombre es requerido";
				} else {
					ExternalPackedProductionLineSQL sql = new ExternalPackedProductionLineSQL();
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
		public IActionResult deletePackedProdLine([FromBody] SearchExternalPackedRequest request)
		{
			log.trace("deletePackedProdLine");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteExternalPackedResponse response = new DeleteExternalPackedResponse();
			try
			{
				ExternalPackedProductionLineSQL sql = new ExternalPackedProductionLineSQL();
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
		/// Desarrollador: Iván Gutiérrez / Hernán Gómez 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchBoxManagement")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchBoxManagement([FromBody] SearchBoxManagExtPackedRequest request)
		{
			log.trace("SearchBoxManagementExt");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchExtPackedBoxManagementResponse response = new SearchExtPackedBoxManagementResponse();
			try
			{
				ExternalPackedBoxManagementSQL sql = new ExternalPackedBoxManagementSQL();
				sql.packedId = request.packedId; //16
				sql.productId = request.productId; // 0
				sql.typeView = request.typeView; //2
				sql.dateStart = request.dateStart; //
				sql.dateEnd = request.dateEnd;
				sql.companiaId = request.companiaId; //0
				sql.searchfield = request.searchfield; //""
				sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdCompany").Value);// id del usuario logueado

				// Obtener lista de agrupacioens
				response.infoData= sql.SearchPackedBoxManagement();
				//-- Es neta?  🙄🙄🙄🙄😒😒😒
				//response.infoData = sql.SearchPackedBoxManagementFilter();
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
		public IActionResult SearchBoxManagementDetail([FromBody] SearchBoxManagExtPackedRequest request)
		{
			log.trace("SearchBoxManagementDetailExt");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchExtPackedBoxManagementDetailResponse response = new SearchExtPackedBoxManagementDetailResponse();
			try
			{
				ExternalPackedBoxManagementSQL sql = new ExternalPackedBoxManagementSQL();
				sql.groupingId = request.groupingId;
				sql.pallet = request.pallet;
				sql.box = request.box;
				sql.typeView = request.typeView;
				sql.productId = request.productId;
				// Obtener lista de agrupacioens
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
		/// Web Method para obtener el detalle de una agrupación (operación)
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SaveUnionOperations")]
		[HttpPost]
		[Authorize]
		public IActionResult SaveUnionOperations([FromBody] SearchUnionOperationsExtPackedRequest request)
		{
			log.trace("SaveUnionOperationsExtPackedRequest");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveExternalPackedResponse response = new SaveExternalPackedResponse();
			try
			{
				if (request.groupingPId == request.groupingSId) {
					response.messageEng = "The selected groupings must not be the same grouping";
					response.messageEsp = "Las agrupaciones seleccionadas no deben ser la misma agrupación";
					throw new Exception(response.messageEsp);
				} else {
					ExternalPackedUnionOperationSQL sql = new ExternalPackedUnionOperationSQL();
					sql.groupingPId = request.groupingPId;
					sql.groupingSId = request.groupingSId;
					sql.groupingType = request.groupingType;
					sql.groupingName = request.groupingName;
					sql.groupings = request.groupings;
					sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdCompany").Value);// id del usuario logueado
					sql.userType = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IsType").Value);// tipo de usuario logueado - Interno, Externo, Usuario

					// Proceso para generar una nueva unión entre operaciones
					sql.UnionProcessExt(response);
					sql.groupingName = request.groupingName;
					// Guardar la unión de operaciones
					sql.SavePackedUnionOperation(response);
                }
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
		/// Web Method para obtener el detalle de una agrupación (operación) al reimprimir etiqueta
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("PrintOperationLabel")]
		[HttpPost]
		[Authorize]
		public IActionResult PrintOperationLabel([FromBody] SearchPrintOperationExtPackedRequest request)
		{
			log.trace("PrintOperationLabelExtPackedRequest");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			PrintOperationExtResponse response = new PrintOperationExtResponse();
			try
			{
				ExternalPackedPrintOperationSQL sql = new ExternalPackedPrintOperationSQL();
				sql.groupingId = request.groupingId;
				sql.groupingType = request.groupingType;
				sql.box = request.box;
				sql.pallet = request.pallet;
				
				// Proceso para obtener la info para reimprimir etiqueta de una operación
				sql.PrintProcessExt(response);
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
		public IActionResult SearchArmingReport([FromBody] SearchArmingReportExtPackedRequest request)
		{
			log.trace("SearchArmingReport");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchExtPackedArmingReportResponse response = new SearchExtPackedArmingReportResponse();
			try
			{
				ExternalPackedArmingReportSQL sql = new ExternalPackedArmingReportSQL();
				sql.packagingId = request.packagingId;
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
		public IActionResult SaveAssociateProducts([FromBody] SaveProductExternalPackedRequest request)
		{
			log.trace("SaveAssociateProducts");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveExternalPackedResponse response = new SaveExternalPackedResponse();
			try
			{
				ProductExternalPackedSQL sql = new ProductExternalPackedSQL();
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
		public IActionResult SearchAssociateProducts([FromBody] SearchProductExternalPackedRequest request)
		{
			log.trace("SearchAssociateProducts");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchProductExternalPackedResponse response = new SearchProductExternalPackedResponse();
			try
			{
				ProductExternalPackedSQL sql = new ProductExternalPackedSQL();
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