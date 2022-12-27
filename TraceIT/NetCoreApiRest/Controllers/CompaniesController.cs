using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;
using System.Linq;
using WSTraceIT.Models.Base.Companies;
using NetCoreApiRest.Utils;

namespace WSTraceIT.Controllers
{
    [Route("Companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
		LoggerD4 log = new LoggerD4("CompaniesController");
		/// <summary>
		/// Web Method para guardar los datos de la compañia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveCompany")]		
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.agregarCompania)]
		public IActionResult saveCompany([FromBody]SaveCompanyRequest request)
		{
			log.trace("saveCompany");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveCompanyResponse response = new SaveCompanyResponse();
			try
			{
				CompaniesSQL sql = new CompaniesSQL();
				sql.name = request.name;
				sql.businessName = request.businessName;
				sql.email = request.email;
				sql.webSite = request.webSite;
				sql.phone = request.phone;
				sql.country = request.country;
				sql.address = request.address;
				sql.status = request.status;
				sql.facebook = request.facebook;
				sql.youtube = request.youtube;
				sql.linkedin = request.linkedin;
				sql.clientNumber = request.clientNumber;
				sql.tipoGiro = request.tipoGiro;

                sql.contactEmailFirst = request.contactCompanies.contactEmailFirst;
				sql.contactEmailSecond = request.contactCompanies.contactEmailSecond;
				sql.contactNameFirst = request.contactCompanies.contactNameFirst;
				sql.contactNameSecond = request.contactCompanies.contactNameSecond;
				sql.contactPhoneFirst = request.contactCompanies.contactPhoneFirst;
				sql.contactPhoneSecond = request.contactCompanies.contactPhoneSecond;
				sql.defaultFirst = request.contactCompanies.defaultFirst;
				sql.defaultSecond = request.contactCompanies.defaultSecond;

				sql.SaveCompany();
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
		/// Web Method para actualizar los datos de la compañia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editCompany")]		
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarCompañia)]
		public IActionResult editCompany([FromBody]EditCompanyRequest request)
		{
			log.trace("editCompany");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditCompanyResponse response = new EditCompanyResponse();
			try
			{
				CompaniesSQL sql = new CompaniesSQL();
				sql.idCompany = request.idCompany;
				sql.name = request.name;
				sql.businessName = request.businessName;
				sql.email = request.email;
				sql.webSite = request.webSite;
				sql.phone = request.phone;
				sql.country = request.country;
				sql.address = request.address;
				sql.status = request.status;
				sql.facebook = request.facebook;
				sql.youtube = request.youtube;
				sql.linkedin = request.linkedin;
				sql.clientNumber = request.clientNumber;
				sql.tipoGiro = request.tipoGiro;

                sql.idContactFirst = request.contactCompanies.idContactFirst;
				sql.idContactSecond = request.contactCompanies.idContactSecond;
				sql.contactEmailFirst = request.contactCompanies.contactEmailFirst;
				sql.contactEmailSecond = request.contactCompanies.contactEmailSecond;
				sql.contactNameFirst = request.contactCompanies.contactNameFirst;
				sql.contactNameSecond = request.contactCompanies.contactNameSecond;
				sql.contactPhoneFirst = request.contactCompanies.contactPhoneFirst;
				sql.contactPhoneSecond = request.contactCompanies.contactPhoneSecond;
				sql.defaultFirst = request.contactCompanies.defaultFirst;
				sql.defaultSecond = request.contactCompanies.defaultSecond;

				sql.UpdateCompany();
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
		/// Web Method para eliminar la compañia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteCompany")]		
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.eliminarCompañia)]
		public IActionResult deleteCompany([FromBody]DeleteCompanyRequest request)
		{
			log.trace("deleteCompany");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteCompanyResponse response = new DeleteCompanyResponse();
			try
			{
				CompaniesSQL sql = new CompaniesSQL();
				sql.idCompany = request.idCompany;

				sql.DeleteCompany();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);
				if (ex.Message.StartsWith("La compañía")){
					response.messageEng = ex.Message;
					response.messageEsp = ex.Message;
				}
				else{
					response.messageEng = "An error occurred: " + ex.Message;
					response.messageEsp = "Ocurrio un error: " + ex.Message;
				}
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para buscar la compañia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchCompany")]		
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloCompañia)]
		public IActionResult searchCompany([FromBody]SearchCompanyRequest request)
		{
			log.trace("searchCompany");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchCompanyResponse response = new SearchCompanyResponse();
			try
			{
				CompaniesSQL sql = new CompaniesSQL();
				sql.name = request.name;
				sql.businessName = request.businessName;
				response.companiesDataList = sql.SeachCompanies();
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

		// <summary>
		/// Web Method para Npmbre de compañía logeadaa
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchCompanyName")]
		[HttpPost]
		[Authorize]
		public IActionResult searchCompanyName([FromBody] SearchCompanyNameRequest request)
		{
			log.trace("searchCompanyName");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchCompanyNameResponse response = new SearchCompanyNameResponse();
			try
			{
				CompaniesSQL sql = new CompaniesSQL();
				sql.idCompany = request.companiaId;
				response.companiaName = sql.searchCompanyName();
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
		/// Web Method para buscar la compañia por empacador
		/// Desarrollador: Hernán Gómez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchCompanyEmpacador")]
		[HttpPost]
		[Authorize]
		public IActionResult searchCompanyEmpacador([FromBody] SearchCompanyRequest request)
		{
			log.trace("searchCompany");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchCompanyResponse response = new SearchCompanyResponse();
			try
			{
				CompaniesSQL sql = new CompaniesSQL();
				sql.packedId = request.packedId;
				response.companiesDataList = sql.searchCompanyEmpacador();
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
		/// Web Method para realizar la busqueda de los datos de una compañia para su edicion
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchCompanyData")]		
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarCompañia)]
		public IActionResult searchCompanyData([FromBody]SearchCompanyDataRequest request)
		{
			log.trace("searchCompanyData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchCompanyDataResponse response = new SearchCompanyDataResponse();
			try
			{
				CompaniesSQL sql = new CompaniesSQL();
				sql.idCompany = request.idCompany;
				CompanyDataSQL respSQL = sql.SearchCompaniesData();

				response.companyData.address = respSQL.companyDataEdition.address;
				response.companyData.businessName = respSQL.companyDataEdition.businessName;
				response.companyData.country = respSQL.companyDataEdition.country;
				response.companyData.email = respSQL.companyDataEdition.email;
				response.companyData.idCompany = respSQL.companyDataEdition.idCompany;
				response.companyData.name = respSQL.companyDataEdition.name;
				response.companyData.phone = respSQL.companyDataEdition.phone;
				response.companyData.status = respSQL.companyDataEdition.status;
				response.companyData.webSite = respSQL.companyDataEdition.webSite;
				response.companyData.facebook = respSQL.companyDataEdition.facebook;
				response.companyData.linkedin = respSQL.companyDataEdition.linkedin;
				response.companyData.youtube = respSQL.companyDataEdition.youtube;
				response.companyData.clientNumber = respSQL.companyDataEdition.clientNumber;
				response.companyData.tipoGiro = respSQL.companyDataEdition.tipoGiro;
               

                response.contactData.contactEmailFirst = (from cd in respSQL.contactCompaniesData
														  orderby cd.idContact ascending
														  select cd.email).FirstOrDefault();

				response.contactData.contactNameFirst = (from cd in respSQL.contactCompaniesData
														 orderby cd.idContact ascending
														 select cd.name).FirstOrDefault();

				response.contactData.contactPhoneFirst = (from cd in respSQL.contactCompaniesData
														  orderby cd.idContact ascending
														  select cd.phone).FirstOrDefault();

				response.contactData.defaultFirst = (from cd in respSQL.contactCompaniesData
													 orderby cd.idContact ascending
													 select cd.defaultContact).FirstOrDefault();

				response.contactData.idContactFirst = (from cd in respSQL.contactCompaniesData
													   orderby cd.idContact ascending
													   select cd.idContact).FirstOrDefault();

				response.contactData.contactEmailSecond = (from cd in respSQL.contactCompaniesData
														  orderby cd.idContact descending
														  select cd.email).FirstOrDefault();

				response.contactData.contactNameSecond = (from cd in respSQL.contactCompaniesData
														 orderby cd.idContact descending
														 select cd.name).FirstOrDefault();

				response.contactData.contactPhoneSecond = (from cd in respSQL.contactCompaniesData
														  orderby cd.idContact descending
														  select cd.phone).FirstOrDefault();

				response.contactData.defaultSecond = (from cd in respSQL.contactCompaniesData
													 orderby cd.idContact descending
													 select cd.defaultContact).FirstOrDefault();

				response.contactData.idContactSecond = (from cd in respSQL.contactCompaniesData
													   orderby cd.idContact descending
													   select cd.idContact).FirstOrDefault();
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