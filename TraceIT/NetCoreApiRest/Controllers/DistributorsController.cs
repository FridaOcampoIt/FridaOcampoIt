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
    [Route("Distributors")]
    [ApiController]
    public class DistributorsController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("DistributorsController");

		/// <summary>
		/// Web Method que sirve para realizar la búsqueda de distribuidores 
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchDistributors")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchDistributors([FromBody] SearchDistributorsRequest request)
		{
			log.trace("SearchDistributors");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDistributorsResponse response = new SearchDistributorsResponse();
			try
			{
				DistributorsSQL sql = new DistributorsSQL();
				sql.distributorName = request.distributor;
				sql.businessName = request.businessName;
				sql.opc = request.opc;
				var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;// id del usuario logueado
				//validar si el usuario es administrador o consultor
				//si el usuario es administrador, se manda el id del usuario logueado
				//si el usuario es consultor, se manda un 0 como idusuario logueado
				sql.userId = Convert.ToInt32(idUser);

				response.distributors = sql.SearchDistributors();
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
		/// Web Method que sirve para obtener los datos de un distribuidor
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchDistributorData")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchDistributorData([FromBody] SearchDistributorsRequest request)
		{
			log.trace("SearchDistributorData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDistributorsResponse response = new SearchDistributorsResponse();
			try
			{
				DistributorsSQL sql = new DistributorsSQL();
				sql.distributorId = request.distributorId;
				
				response.distributors = sql.SearchDistributorData();
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
		/// Web Method que sirve para guardar / actualizar un distribuidor
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SaveDistributor")]
		[HttpPost]
		[Authorize]
		public IActionResult SaveDistributor([FromBody] SaveDistributorRequest request)
		{
			log.trace("SaveDistributor");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveDistributorResponse response = new SaveDistributorResponse();
			try
			{
				if (request.distributorData.distributorNumber.Trim() == "" || request.distributorData.distributorName.Trim() == "") {
					response.messageEng = "Some fields are required";
					response.messageEsp = "Algunos campos son obligatorios";
				} else {
					DistributorsSQL sql = new DistributorsSQL();
					sql.distributorId = request.distributorData.distributorId;
					sql.distributorNumber = request.distributorData.distributorNumber;
					sql.distributorName = request.distributorData.distributorName;
					sql.businessName = request.distributorData.businessName;
					sql.email = request.distributorData.email;
					sql.phone = request.distributorData.phone;
					sql.status = request.distributorData.status;
					sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);// id del usuario logueado
					sql.sp = sql.distributorId == 0 ? "spi_guardarDistribuidorEmpaqEtiq" : "spu_edicionDistribuidorEmpaqEtiq";

					sql.SaveDistributor();
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
		/// Web Method para eliminar un distribuidor
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteDistributor")]
		[HttpPost]
		[Authorize]
		public IActionResult deleteDistributor([FromBody] DeleteDistributorRequest request)
		{
			log.trace("deleteDistributor");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteDistributorResponse response = new DeleteDistributorResponse();
			try
			{
				DistributorsSQL sql = new DistributorsSQL();
				sql.distributorId = request.distributorId;

				sql.DeleteDistributor();
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
		public IActionResult SaveAssociateProducts([FromBody] SaveProductDistributorRequest request)
		{
			log.trace("SaveAssociateProducts");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveDistributorResponse response = new SaveDistributorResponse();
			try
			{
				DistributorsSQL sql = new DistributorsSQL();
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
		public IActionResult SearchAssociateProducts([FromBody] SearchDistributorsRequest request)
		{
			log.trace("SearchAssociateProducts");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchProductDistributorResponse response = new SearchProductDistributorResponse();
			try
			{
				DistributorsSQL sql = new DistributorsSQL();
				sql.distributorId = request.distributorId;
				sql.productId = request.productId;
				sql.companyId = request.companyId;
				sql.packagingId = request.packagingId;
				sql.rawMaterial = request.rawMaterial;
				sql.topc = request.topc;

				response.productData = sql.SearchProductDistributor();
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