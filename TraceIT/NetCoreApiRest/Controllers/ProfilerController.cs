using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Base.Profiles;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;

namespace WSTraceIT.Controllers
{
    [Route("Profiler")]
    [ApiController]
    public class ProfilerController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("ProfilerController");
		/// <summary>
		/// Web Method para guardar el perfil
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveProfile")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.agregarPerfiles)]
		public IActionResult saveProfile([FromBody]SaveProfileRequest request)
		{
			log.trace("saveProfile");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveProfileResponse response = new SaveProfileResponse();
			try
			{
				ProfilesSQL sql = new ProfilesSQL();
				sql.name = request.name;
				sql.company = request.company;
				sql.permission = request.permission;

				sql.SaveProfile();
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
		/// Web Method para editar el perfil 
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editProfile")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarPerfiles)]
		public IActionResult editProfile([FromBody]EditProfileRequest request)
		{
			log.trace("editProfile");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditProfileResponse response = new EditProfileResponse();
			try
			{
				ProfilesSQL sql = new ProfilesSQL();
				sql.profileId = request.profileId;
				sql.name = request.name;
				sql.company = request.company;
				sql.permission = request.permission;

				sql.EditProfile();
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
		/// Web Method para eliminar un perfil
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteProfile")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.eliminarPerfiles)]
		public IActionResult deleteProfile([FromBody]DeleteProfileRequest request)
		{
			log.trace("deleteProfile");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteProfileResponse response = new DeleteProfileResponse();
			try
			{
				ProfilesSQL sql = new ProfilesSQL();
				sql.profileId = request.profileId;

				sql.DeleteProfile();
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
		/// Web Method para buscar los perfiles
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchProfile")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloPerfiles)]
		public IActionResult searchProfile([FromBody]SearchProfileRequest request)
		{
			log.trace("searchProfile");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchProfileResponse response = new SearchProfileResponse();
			try
			{
				ProfilesSQL sql = new ProfilesSQL();
				sql.name = request.name;
				sql.company = request.company;

				response.profiles = sql.SearchProfiles();
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
		/// Web Method para la consulta de los datos de los perfiles para su edición
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchProfileData")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarPerfiles)]
		public IActionResult searchProfileData([FromBody]SearchProfileDataRequest request)
		{
			log.trace("searchProfileData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchProfileDataResponse response = new SearchProfileDataResponse();
			try
			{
				ProfilesSQL sql = new ProfilesSQL();
				sql.profileId = request.profileId;

				response.permissionProfileData = sql.SearchProfileData();
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
		/// Web Method para buscar los combos y los permisos de los perfiles
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchDropDownPermission")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloPerfiles)]
		public IActionResult searchDropDownPermission([FromBody]SearchDropDownPermissionRequest request)
		{
			log.trace("searchDropDownPermission");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchDropDownPermissionResponse response = new SearchDropDownPermissionResponse();
			try
			{
				ProfilesSQL sql = new ProfilesSQL();
				sql.company = request.company;

				DropDownProfilesPermissionSQL responseSQL = sql.SearchDropDownPermission();
				response.dropDownProfilesPermission.companyList = responseSQL.companyList;

				response.dropDownProfilesPermission.permissions = (from permisos in responseSQL.permissions
																   where permisos.isFather == true
																   select new PermissionEstructure
																   {
																	   idPermission = permisos.idPermission,
																	   name = permisos.name,
																	   permission = (from children in responseSQL.permissions
																					 where children.fatherPermissionId == permisos.idPermission
																					 select new PermissionChildren
																					 {
																						 idPermission = children.idPermission,
																						 name = children.name,
																						 isForCompany = children.forUserCompany

																					 }).ToList()
																   }).ToList();
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