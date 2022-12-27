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
    [Route("AddressProvider")]
    [ApiController]
    public class AddressProviderController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("AddressProviderController");

		/// <summary>
		/// Web Method que sirve para realizar la búsqueda de direcciones 
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchAddress")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchAddress([FromBody] SearchAddressProviderRequest request)
		{
			log.trace("SearchAddress");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchAddressProviderResponse response = new SearchAddressProviderResponse();
			try
			{
				AddressProviderSQL sql = new AddressProviderSQL();
				sql.typeCompany = request.typeCompany;
				sql.familyId = request.familyId;
				sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
				sql.tipo = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IsType").Value);

				response.addressLst = sql.SearchAddressProvider();
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
		/// Web Method que sirve para obtener los datos de una dirección
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchAddressData")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchAddressData([FromBody] SearchAddressProviderRequest request)
		{
			log.trace("SearchAddressData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchAddressProviderResponse response = new SearchAddressProviderResponse();
			try
			{
				AddressProviderSQL sql = new AddressProviderSQL();
				sql.addressId = request.addressId;
				
				response.addressLst = sql.SearchAddressProviderData();
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
		/// Web Method que sirve para realizar la búsqueda de direcciones para un combo (select)
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SearchAddressCombo")]
		[HttpPost]
		[Authorize]
		public IActionResult SearchAddressCombo([FromBody] SearchAddressProviderRequest request)
		{
			log.trace("SearchAddressCombo");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchAddressProviderComboResponse response = new SearchAddressProviderComboResponse();
			try
			{
				AddressProviderSQL sql = new AddressProviderSQL();
				//sql.typeCompany = request.typeCompany;
				//sql.typeCompany = request.typeCompany;
				//sql.familyId = request.familyId;
				//sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
				//sql.tipo = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IsType").Value);
				sql.empacadorId = request.empacadorId;
				response.addressComboLst = sql.SearchAddressProviderCombo();
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
		/// Web Method que sirve para guardar / actualizar una dirección
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("SaveAddress")]
		[HttpPost]
		[Authorize]
		public IActionResult SaveAddress([FromBody] SaveAddressProviderRequest request)
		{
			log.trace("SaveAddress");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveProviderResponse response = new SaveProviderResponse();
			try
			{
				if (request.addressName.Trim() == "" || request.phone.Trim() == "" ||
					request.address.Trim() == "" || request.latitude.Trim() == "" ||
					request.longitude.Trim() == "") {
					response.messageEng = "Some fields are required";
					response.messageEsp = "Algunos campos son obligatorios";
				} else {
					AddressProviderSQL sql = new AddressProviderSQL();
					sql.addressId = request.addressId;
					sql.addressName = request.addressName;
					sql.phone = request.phone;
					sql.city = request.city;
					sql.cp = request.cp;
					sql.address = request.address;
					sql.latitude = request.latitude;
					sql.longitude = request.longitude;
					sql.status = request.status;
					sql.typeCompany = request.typeCompany;
					sql.familyId = request.familyId;
					sql.providers = request.providers;
					sql.paisId = request.paisId;
					sql.estadoId = request.estadoId;
					sql.userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value);
					sql.empacadorId = request.empacadorId;
					sql.isType = request.isType;
					sql.sp = sql.addressId == 0 ? "spi_guardarDireccionesProveedor" : "spu_edicionDireccionesProveedor";

					sql.SaveAddressProvider();
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
		/// Web Method para eliminar una dirección
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteAddress")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.eliminarDireccionesProveedor)]
		public IActionResult deleteAddress([FromBody] DeleteAddressProviderRequest request)
		{
			log.trace("deleteAddress");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteAddressProviderResponse response = new DeleteAddressProviderResponse();
			try
			{
				AddressProviderSQL sql = new AddressProviderSQL();
				sql.addressId = request.addressId;

				sql.DeleteAddressProvider();
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

		[Route("SearchProviders")]
		[HttpPost]
		public IActionResult SearchProviders()
		{
			log.trace("SearchProviders");
			SearchProvidersDirectionResponse response = new SearchProvidersDirectionResponse();

            try
            {
				AddressProviderSQL sql = new AddressProviderSQL();

				var idUser = User.Claims.FirstOrDefault(u => u.Type == "IdUser").Value;
				sql.userId = Convert.ToInt32(idUser);

				response.providers = sql.SearchProviders();

				log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			}
            catch (Exception ex)
            {

				log.error($"Exception: {ex.Message}");
				response.messageEng = $"An error ocurred: {ex.Message}";
				response.messageEsp = $"Ocurrio un error: {ex.Message}";
            }

			return Ok(response);

		}
	}
}