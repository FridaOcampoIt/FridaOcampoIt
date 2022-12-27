using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.Configuration;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Controllers
{
    [Route("PackedLabeled")]
    [ApiController]
    public class PackedLabeledController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("PackedLabeledController");

		/// <summary>
		/// Web Method que sirve para realizar la búsqueda de las agrupaciones de acuerdo a las opciones seleccionadas en el filtro 
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchGrouping")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchGrouping([FromBody] SearchPackedLabeledRequest request)
		{
			log.trace("SearchGrouping");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchGroupingResponse response = new SearchGroupingResponse();
			try {
				// Proveedores
				if (request.opc == 1) {
					response.groupingList = new List<GroupingDataSQL>();
					return Ok(response);
				}

				GroupingSQL sql = new GroupingSQL();
				sql.companyId = request.companyId;
				sql.productId = request.productId;
				sql.empacadorId = request.empacadorId;
				// sql.searchGeneric = request.searchGeneric;
				sql.dateStart = request.dateStart;
				sql.dateEnd = request.dateEnd;
				sql.opc = request.opc;
				// sql.chkDistributor = request.chkDistributor;
				// sql.usuarioId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);

				// Tipo de empacador: 0 - Usuario compañía; 1 - Usuario externo; 2 - Usuario interno
				int isEmp = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IsType").Value);


				response.groupingList = sql.SearchGroupsPackedLabeled();

			} catch (Exception ex) {
				log.error("There was an error " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);
		}

		/// <summary>
		/// Web Method que sirve para obtener la lista de compañias para los combos de empacado y etiquetado
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchCompanyCombo")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchCompanyCombo()
		{
			log.trace("SearchCompanyCombo");
			SearchCompanyPackLabComboResponse response = new SearchCompanyPackLabComboResponse();
			try {
				PackedLabeledSQL sql = new PackedLabeledSQL();
				sql.opc = 1;
				//var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
				//sql.userId = Convert.ToInt32(idUser);

				response.companiescombo = sql.SearchCompanyCombo();
			} catch (Exception ex) {
				log.error("There was an error " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method que sirve para obtener la lista de productos para los combos de empacado y etiquetado
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchProductCombo")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchProductCombo([FromBody] SearchPackedLabeledCombosRequest request)
		{
			log.trace("SearchProductCombo");
			SearchProductPackLabComboResponse response = new SearchProductPackLabComboResponse();
			try {
				PackedLabeledSQL sql = new PackedLabeledSQL();
				sql.id = request.id > 0 ? request.id : Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdCompany").Value);
				sql.opc = 2;
				//var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
				//sql.userId = Convert.ToInt32(idUser);

				response.productscombo = sql.SearchProductCombo();
			} catch (Exception ex) {
				log.error("There was an error " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method que sirve para obtener la lista de embalajes para los combos de empacado y etiquetado
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchPackagingCombo")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchPackagingCombo([FromBody] SearchPackedLabeledCombosRequest request)
		{
			log.trace("SearchPackagingCombo");
			SearchPackagingPackLabComboResponse response = new SearchPackagingPackLabComboResponse();
			try {
				PackedLabeledSQL sql = new PackedLabeledSQL();
				sql.id = request.id;
				sql.opc = 3;
				//var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
				//sql.userId = Convert.ToInt32(idUser);

				response.packagingcombo = sql.SearchPackagingCombo();
			} catch (Exception ex) {
				log.error("There was an error " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method que sirve para obtener la lista de compañias, productos y embalajes para los combos de empacado y etiquetado
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchComProdPackCombo")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchComProdPackCombo()
		{
			log.trace("SearchComProdPackCombo");
			SearchComProdPackComboResponse response = new SearchComProdPackComboResponse();
			try {
				PackedLabeledSQL sql = new PackedLabeledSQL();
				sql.opc = 4;
				//var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
				//sql.userId = Convert.ToInt32(idUser);

				response = sql.SearchComProdPackCombos();
			} catch (Exception ex) {
				log.error("There was an error " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method que sirve para obtener la lista de todas Familias de productos
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchFamilyProductCombo")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchFamilyProductCombo([FromBody] SearchPackedLabeledCombosRequest request)
		{
			log.trace("SearchFamilyProductCombo");
			SearchFamilyProductPackLabComboResponse response = new SearchFamilyProductPackLabComboResponse();
			try
			{
				PackedLabeledSQL sql = new PackedLabeledSQL();
				sql.id = request.id > 0 ? request.id : (request.id == -1 ? Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdCompany").Value) : 0);
				sql.opc = 5;

				response.familiescombo = sql.SearchFamilyProductCombo();
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
		/// Web Method que sirve para obtener la lista de operadores y lineas de producción en base a una compañia
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchCompanyInfoCombo")]
		[HttpPost]
		//[Authorize]
		public IActionResult SearchCompanyInfoCombo([FromBody] SearchCompanyInfoCombosRequest request)
		{
			log.trace("SearchCompanyInfoCombo");
			SearchCompanyInfoComboResponse response = new SearchCompanyInfoComboResponse();
			try
			{
				PackedLabeledSQL sql = new PackedLabeledSQL();
				sql.id = request.id; //id compañia
				// Valores para opc:
				// 1 - Operadores
				// 2 - Lineas de producción
				// 3 - Productos
				// 4 - Embalajes
				sql.opc = request.opc; 
				//var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
				//sql.userId = Convert.ToInt32(idUser);

				response.infocombo = sql.SearchCompanyInfoCombo();
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
		/// Web Method que sirve para obtener la lista de operadores y lineas de producción en base a una compañia
		/// Desarrollador: 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchCompanyInfoComboEmbalaje")]
		[HttpPost]
		//[Authorize]
		public IActionResult SearchCompanyInfoComboEmabalaje([FromBody] SearchCompanyInfoCombosEmbalajeRequest request)
		{
			log.trace("SearchCompanyInfoCombo");
			SearchCompanyInfoComboResponse response = new SearchCompanyInfoComboResponse();
			try
			{
				PackedLabeledSQL sql = new PackedLabeledSQL();
				sql.id = request.id; //id compañia
									 // Valores para opc:
									 // 1 - Operadores
									 // 2 - Lineas de producción
									 // 3 - Productos
									 // 4 - Embalajes
				sql.opc = request.opc;
				sql.productId = request.familiaId;
				sql.providerId = request.providerId;
				//var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
				//sql.userId = Convert.ToInt32(idUser);

				response.infocombo = sql.SearchCompanyInfoComboEmbalaje();
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
		/// Web Method que obtiene las materias primas de un proveedor de acuerdo al filtro aplicado
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("getRawMaterials")]
		[HttpPost]
		[Authorize]
		public IActionResult getRawMaterials([FromBody] SearchRawMaterialsRequest request)
		{
			log.trace("getRawMaterials");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchRawMaterialsResponse response = new SearchRawMaterialsResponse();
			try
			{
				PackedLabeledSQL sql = new PackedLabeledSQL();
				sql.companyId = request.companyId;
				sql.productId = request.productId;
				sql.packagingId = request.packagingId;
				sql.providerId = request.providerId;

				response.materialsLst = sql.SearchRawMaterialsList();
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
		/// Web Method que sirve para realizar la búsqueda de direcciones para un combo (select) de acuerdo a la compañia del usuario autenticado
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchAddressCombo")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchAddressCombo([FromBody] SearchPackedLabeledCombosRequest request)
		{
			log.trace("SearchAddressCombo");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchAddressCompanyResponse response = new SearchAddressCompanyResponse();
			try
			{
				PackedLabeledSQL sql = new PackedLabeledSQL();
				sql.typeCompany = request.typeCompany;
				sql.id = request.id == -1 ? 0 : request.id;
				sql.companyId = request.id == -1 ? request.id2 : Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdCompany").Value);
				sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);

				response.addressLst = sql.SearchAddressComboList();
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
		/// Web Method para consultar la existencia de un ciu,
		/// Desarrollador: Omar Larrion
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchExisteCIU")]
		[HttpPost]
		public IActionResult existeCIU([FromBody] SearchExisteCIURequest request)
		{
			log.trace("searchExisteCIU");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchExisteCIUResponse response = new SearchExisteCIUResponse();
            try
            {
				PackedLabeledSQL sql = new PackedLabeledSQL();
				sql.ciu = request.ciu;

				response.existeLst = sql.SearchExisteCIU();

            }
            catch (Exception ex)
            {
				log.error("There was an error " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
				throw;
            }

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);

		}

		/// <summary>
		/// Web Method para obtener la información de un pallet
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns>List</returns>
		[Route("searchInfoQrCode")]
		[HttpPost]
		[Authorize]
		public IActionResult searchInfoQrCode([FromBody] SearchQrCodeRequest request)
		{
			log.trace("searchInfoQrCode");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchInfoQRResponse response = new SearchInfoQRResponse();
			try
			{
				PackedLabeledSQL sql = new PackedLabeledSQL();
				sql.ciu = request.ciuI;
				sql.ciuF = request.ciuF;
				sql.init = request.init;
				sql.productId = request.productId;
				sql.packagingId = request.packId;

				response.infoLst = sql.SearchInfoQRCodeCIU();

			}
			catch (Exception ex)
			{
				log.error("There was an error " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
				throw;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);

		}

		/// <summary>
		/// Web Method guardar 
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns>Object JSON</returns>
		[Route("saveOperationPallet")]
		[HttpPost]
		[Authorize]
		public IActionResult saveOperationPallet([FromBody] SearchOperationPallet request)
		{
			log.trace("saveOperationPallet");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			saveOperationPalletResponse response = new saveOperationPalletResponse();
			try
			{
				PackedLabeledSQL sql = new PackedLabeledSQL();
				sql.pallets = request.pallets;
				string latmv = User.Claims.FirstOrDefault(u => u.Type == "Latitude").Value;
				string longmv = User.Claims.FirstOrDefault(u => u.Type == "Longitude").Value;
				int user_idmv = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
				response = sql.saveOperationPallet(latmv, longmv, user_idmv);

			}
			catch (Exception ex)
			{
				log.error("There was an error " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
				throw;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));
			return Ok(response);

		}
	}
}