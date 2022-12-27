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
    [Route("Providers")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("ProvidersController");

		/// <summary>
		/// Web Method que sirve para realizar la búsqueda de Proveedores 
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchProviders")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchProviders([FromBody] SearchProvidersRequest request)
		{
			log.trace("SearchProviders");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchProvidersResponse response = new SearchProvidersResponse();
			try
			{
				ProvidersSQL sql = new ProvidersSQL();
				sql.providerName = request.provider;
				sql.businessName = request.businessName;
				sql.opc = request.opc;
				var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
				//validar si el usuario es administrador o consultor
				//si el usuario es administrador, se manda el id del usuario logueado
				//si el usuario es consultor, se manda un 0 como idusuario logueado
				sql.userId = Convert.ToInt32(idUser);

				response.providers = sql.SearchProviders();
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
		/// Web Method que sirve para obtener los datos de un proveedor
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchProviderData")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchProviderData([FromBody] SearchProvidersRequest request)
		{
			log.trace("SearchProviderData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchProvidersResponse response = new SearchProvidersResponse();
			try
			{
				ProvidersSQL sql = new ProvidersSQL();
				sql.providerId = request.providerId;
				
				response.providers = sql.SearchProviderData();
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
		/// Web Method que sirve para guardar / actualizar un Proveedor
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SaveProvider")]
		[HttpPost]
		[Authorize]
		public IActionResult SaveProvider([FromBody] SaveProviderRequest request)
		{
			log.trace("SaveProvider");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveProviderResponse response = new SaveProviderResponse();
			try
			{
				if (request.providerData.providerNumber.Trim() == "" || request.providerData.providerName.Trim() == "") {
					response.messageEng = "Some fields are required";
					response.messageEsp = "Algunos campos son obligatorios";
				} else {
					ProvidersSQL sql = new ProvidersSQL();
					sql.providerId = request.providerData.providerId;
					sql.providerNumber = request.providerData.providerNumber;
					sql.providerName = request.providerData.providerName;
					sql.businessName = request.providerData.businessName;
					sql.email = request.providerData.email;
					sql.phone = request.providerData.phone;
					sql.status = request.providerData.status;
					var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
					sql.userId = Convert.ToInt32(idUser);
					sql.sp = sql.providerId == 0 ? "spi_guardarProveedorEmpaqEtiq" : "spu_edicionProveedorEmpaqEtiq";

					sql.SaveProvider();
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
		/// Web Method para eliminar un proveedor
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteProvider")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.eliminarModuloEmpacadoEtiquetado)]
		public IActionResult deleteProvider([FromBody] DeleteProviderRequest request)
		{
			log.trace("deleteProvider");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteProviderResponse response = new DeleteProviderResponse();
			try
			{
				ProvidersSQL sql = new ProvidersSQL();
				sql.providerId = request.providerId;

				sql.DeleteProvider();
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
		public IActionResult SaveAssociateProducts([FromBody] SaveProductProviderRequest request)
		{
			log.trace("SaveAssociateProducts");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveProviderResponse response = new SaveProviderResponse();
			try
			{
				ProvidersSQL sql = new ProvidersSQL();
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
		public IActionResult SearchAssociateProducts([FromBody] SearchProvidersRequest request)
		{
			log.trace("SearchAssociateProducts");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchProductProviderResponse response = new SearchProductProviderResponse();
			try
			{
				ProvidersSQL sql = new ProvidersSQL();
				sql.providerId = request.providerId;
				sql.productId = request.productId;
				sql.companyId = request.companyId;
				sql.packagingId = request.packagingId;
				sql.rawMaterial = request.rawMaterial;
				sql.topc = request.topc;

				response.productData = sql.SearchProductProvider();
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