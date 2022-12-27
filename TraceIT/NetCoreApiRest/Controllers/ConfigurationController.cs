using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.Configuration;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;

namespace WSTraceIT.Controllers
{
    [Route("Configuration")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
		/// <summary>
		/// Web Method para consultar los valores de las configuraciones generales y los combos
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		[Route("searchDropDownConfiguration")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloConfiguracion)]
		public IActionResult searchDropDownConfiguration()
		{
			SearchDropDownConfigurationResponse response = new SearchDropDownConfigurationResponse();

			try
			{
				ConfigurationSQL sql = new ConfigurationSQL();

				ConfigurationDropDownSQL responseSQL = sql.SearchDropDown();
				response.configuration.companyTraceIT = responseSQL.companies;
				response.configuration.userTraceIT = responseSQL.users;
				response.configuration.generalConfigurations = (from gc in responseSQL.configurationGenerals
																select new GeneralConfigurationData
																{
																	configuration = gc.configuration,
																	value = gc.value.Split(',').ToList()
																}).ToList();
			}
			catch (Exception ex)
			{
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			return Ok(response);
		}

		/// <summary>
		/// Web Method para la busqueda de configuraciones por compañia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		[Route("searchConfigurationCompany")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloConfiguracion)]
		public IActionResult searchConfigurationCompany([FromBody]SearchConfigurationCompanyRequest request)
		{
			SearchConfigurationCompanyResponse response = new SearchConfigurationCompanyResponse();

			try
			{
				ConfigurationSQL sql = new ConfigurationSQL
				{
					company = request.idCompany
				};

				response.companyConfiguration = sql.SearchConfigurationCompany();
			}
			catch (Exception ex)
			{
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			return Ok(response);
		}

		/// <summary>
		/// Web Method para guardar las configuraciones generales
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveGeneralConfiguration")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloConfiguracion)]
		public IActionResult saveGeneralConfiguration([FromBody]SaveGeneralConfigurationRequest request)
		{
			SaveGeneralConfigurationResponse response = new SaveGeneralConfigurationResponse();

			try
			{
				ConfigurationSQL sql = new ConfigurationSQL();
				sql.configurations = request.generalConfiguration;

				sql.SaveGeneralConfiguration();
			}
			catch (Exception ex)
			{
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			return Ok(response);
		}

		/// <summary>
		/// Web Method para guardar la configuracion de la compañia
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveCongfigurationCompany")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloConfiguracion)]
		public IActionResult saveCongfigurationCompany([FromBody]SaveConfigurationCompanyRequest request)
		{
			SaveConfugurationCompanyResponse response = new SaveConfugurationCompanyResponse();

			try
			{
				ConfigurationSQL sql = new ConfigurationSQL();
				sql.company = request.company;
				sql.nUseGuides = request.nUseGuides;
				sql.nInstalationGuides = request.nInstalationGuides;
				sql.nRelatedProduct = request.nRelatedProduct;
				sql.notifyComments = request.notifyComments;
				sql.notifyWarranty = request.notifyWarranty;
				sql.notifyStolen = request.notifyStolen;
				sql.nPDF = request.nPDF;
				sql.nCharFAQ = request.nCharFAQ;
				sql.nCharSpec = request.nCharEspec;
				sql.nImg = request.nImg;
				sql.nVid = request.nVid;

				sql.SaveCompanyConfiguration();
			}
			catch (Exception ex)
			{
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			return Ok(response);
		}
	}
}