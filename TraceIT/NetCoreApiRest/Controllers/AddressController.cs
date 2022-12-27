using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;

namespace WSTraceIT.Controllers
{
    [Route("Address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("AddressController");
		/// <summary>
		/// Web Method para guardar los datos de una dirección
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveAddres")]		
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.agregarDirecciones)]
		public IActionResult saveAddress([FromBody]SaveAddressRequest request)
		{
			log.trace("saveAddress");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveAddressResponse response = new SaveAddressResponse();
			try
			{
				AddressSQL sql = new AddressSQL();
				sql.name = request.name;
				sql.phone = request.phone;
				sql.address = request.address;
				sql.postalCode = request.postalCode;
				sql.city = request.city;
				sql.state = request.state;
				sql.country = request.country;
				sql.latitude = request.latitude;
				sql.longitude = request.longitude;
				sql.status = request.status;
				sql.idCompany = request.idCompany;
				sql.idTypeAddress = request.idTypeAddress;

				sql.SaveAddress();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para editar los datos de una dirección
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editAddress")]		
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarDirecciones)]
		public IActionResult editAddress([FromBody]EditAddressRequest request)
		{
			log.trace("editAddress");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditAddressResponse response = new EditAddressResponse();
			try
			{
				AddressSQL sql = new AddressSQL();
				sql.idAddress = request.idAddress;
				sql.name = request.name;
				sql.phone = request.phone;
				sql.address = request.address;
				sql.postalCode = request.postalCode;
				sql.city = request.city;
				sql.state = request.state;
				sql.country = request.country;
				sql.latitude = request.latitude;
				sql.longitude = request.longitude;
				sql.status = request.status;
				sql.idCompany = request.idCompany;
				sql.idTypeAddress = request.idTypeAddress;

				sql.UpdateAddress();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para eliminar una dirección
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		[Route("deleteAddress")]		
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.eliminarDirecciones)]
		public IActionResult deleteAddress([FromBody]DeleteAddressRequest request)
		{
			log.trace("deleteAddress");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteAddressResponse response = new DeleteAddressResponse();
			try
			{
				AddressSQL sql = new AddressSQL();
				sql.idAddress = request.idAddress;

				sql.DeleteAddress();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para consultar las direcciones
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchAddress")]		
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloDirecciones)]
		public IActionResult searchAddress([FromBody]SearchAddressRequest request)
		{
			log.trace("searchAddress");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchAddressResponse response = new SearchAddressResponse();
			try
			{
				AddressSQL sql = new AddressSQL();
				sql.idFamily = request.idFamily;
				sql.idCompany = request.idCompany;

				response.addressDataList = sql.SearchAddress();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para consultar los datos de las direcciones para su edición
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchAddressData")]		
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarDirecciones)]
		public IActionResult searchAddressData([FromBody]SearchAddressDataRequest request)
		{
			log.trace("searchAddressData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchAddressDataResponse response = new SearchAddressDataResponse();
			try
			{
				AddressSQL sql = new AddressSQL();
				sql.idAddress = request.idAddress;

				response.addressData = sql.SearchAddresData();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para consultar los dropDown list que seran utilizados dentro del modulo
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		[Route("searchDropDownListAddress")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloDirecciones)]
		public IActionResult searchDropDownListAddress([FromBody]SearchDropDownListAddressRequest request)
		{
			log.trace("searchDropDownListAddress");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDropDownListAddressResponse response = new SearchDropDownListAddressResponse();
			try
			{
				AddressSQL sql = new AddressSQL();
				sql.idCompany = request.idCompany;

				AddressListDropDownSQL respSQL = sql.SearchDropDownList();
				response.addressTypeData = respSQL.addressTypeData;
				response.companyData = respSQL.companyData;
				response.familyData = respSQL.familyData;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}
	}
}