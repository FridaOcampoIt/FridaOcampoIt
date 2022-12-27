using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;
using WSTraceIT.InterfacesSQL;
using WSTraceIT.Models.Request;
using WSTraceIT.Models.Response;
using WS.Interfaces;

namespace WSTraceIT.Controllers
{
    [Route("User")]
    [ApiController]
    public class UserController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("UserController");
		/// <summary>
		/// Web Method para realizar el login
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("loginUser")]
		[HttpPost]
		public IActionResult loginUser([FromBody]LoginUserRequest request)
		{
			LoginUserResponse response = new LoginUserResponse();

			log.trace("loginUser");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			try
			{
				UserSQL sql = new UserSQL();
				sql.email = request.user;
				sql.password = request.password;
				sql.idProvider = int.Parse(request.id);
				sql.isOrigin = request.isOrigin;

				response.userData = sql.loginUser();
				
				if (response.userData.userData == null) {
					response.messageEng = "Incorrect email or password";
					response.messageEsp = "Correo electrónico o contraseña incorrectos";
					return Ok(response);
				}

				if (request.isOrigin == 1 && response.userData.userData.isType > 0) {
					response.messageEng = "Incorrect email or password";
					response.messageEsp = "Correo electrónico o contraseña incorrectos";
					return Ok(response);
				}

				if(response.userData.userData != null)
				{
					#region Generación del Token
					List<Claim> claimsData = new List<Claim>();
					claimsData.Add(new Claim("Name", response.userData.userData.name));
					claimsData.Add(new Claim("IdUser", response.userData.userData.idUser.ToString()));
					claimsData.Add(new Claim("IdCompany", response.userData.userData.company.ToString()));
					claimsData.Add(new Claim("IsType", response.userData.userData.isType.ToString()));
					claimsData.Add(new Claim("companyName", response.userData.userData.companyName.ToString()));
					claimsData.Add(new Claim("Merma", response.userData.userData.Merma.ToString()));
					claimsData.Add(new Claim("Latitude", request.lat != null ? request.lat : ""));
					claimsData.Add(new Claim("Longitude", request.lon != null ? request.lon : ""));
					claimsData.Add(new Claim("eUser", response.userData.userData.eUser.ToString()));

					foreach (var per in response.userData.userPermissions)
						claimsData.Add(new Claim("Permission", per.permissionId.ToString()));

					response.token = ConfigurationSite.GenerateToken(claimsData.ToArray());
					#endregion
				}
				else
				{
					response.messageEng = "Incorrect email or password";
					response.messageEsp = "Correo electrónico o contraseña incorrectos";
				}
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}

			return Ok(response);
		}

		/// <summary>
		/// Web method para listar usuarios por compañia (uso en reportes)
		/// Desarrollador: Hernán Gómez
		/// 
		[Route("getUsuarioByCompanyId")]
		[HttpPost]
		[Authorize]
		public IActionResult searchUsersByCompanyId([FromBody] UserByCompaniaId request)
        {
			log.trace("getUsuarioByCompanyId");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchUserByCompanyIdResponse response = new SearchUserByCompanyIdResponse();
			try
			{
				UserSQL sql = new UserSQL();
				sql.companyId = request.companyId;

				response.dataUserByCompanyId = sql.searchUserByCompanyId();

			} catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				response.messageEng = "An error occurred: " + ex.Message;
				response.messageEsp = "Ocurrio un error: " + ex.Message;
			}
			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para guardar el usuario  / Agrega usuarios aún acopio (Si es necesario)
		/// Desarrollador: David Martinez / Hernán Gómez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("saveUser")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.agregarUsuarios)]
		public IActionResult saveUser([FromBody]SaveUserRequest request)
		{
			log.trace("saveUser");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SaveUserResponse response = new SaveUserResponse();
			try
			{
				UserSQL sql = new UserSQL();
				sql.name = request.name;
				sql.lastName = request.lastName;
				sql.email = request.email;
				sql.password = request.password;
				sql.position = request.position;
				sql.companyId = request.companyId;
				sql.rolId = request.rolId;
				sql.profile = request.profile;
				sql.acopiosIds = request.acopiosIds;
				sql.SaveUser();
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
		/// Web Method para editar un usuario / Modifica acopios si es necesario 
		/// Desarrollador: David Martinez / Hernán Gómez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("editUser")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarUsuarios)]
		public IActionResult editUser([FromBody]EditUserRequest request)
		{
			log.trace("editUser");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			EditUserResponse response = new EditUserResponse();
			try
			{
				UserSQL sql = new UserSQL();
				sql.idUser = request.idUser;
				sql.name = request.name;
				sql.lastName = request.lastName;
				sql.email = request.email;
				sql.password = request.password;
				sql.position = request.position;
				sql.companyId = request.companyId;
				sql.rolId = request.rolId;
				sql.profile = request.profile;
				sql.acopiosIds = request.acopiosIds;
				sql.auxAcopiosIds = request.auxAcopiosIds;
				sql.EditUser();
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
		/// Web method para eliminar el usuario
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("deleteUser")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.eliminarUsuarios)]
		public IActionResult deleteUser([FromBody]DeleteUserRequest request)
		{
			log.trace("deleteUser");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			DeleteUserResponse response = new DeleteUserResponse();
			try
			{
				UserSQL sql = new UserSQL();
				sql.idUser = request.idUser;

				sql.DeleteUser();
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
		/// Web Method para realizar la consulta de usuarios
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchUser")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloUsuarios)]
		public IActionResult searchUsers([FromBody]SearchUserRequest request)
		{
			log.trace("searchUsers");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchUserResponse response = new SearchUserResponse();
			try
			{
				UserSQL sql = new UserSQL();
				sql.name = request.name;
				sql.rolId = request.rol;
				sql.companyId = request.company;

				response.dataUser = sql.searchUser();
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
		/// Web Method para realizar la consulta de los datos del usuario
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchUserData")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarUsuarios)]
		public IActionResult searchUserData([FromBody]SearchUserDataRequest request)
		{
			log.trace("searchUserData");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchUserDataResponse response = new SearchUserDataResponse();
			try
			{
				UserSQL sql = new UserSQL();
				sql.idUser = request.idUser;

				response.dataUser = sql.searchUserData();
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
		/// Web Method para consultar los combos para el modulo de usuarios
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("searchUserDropDown")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.moduloUsuarios)]
		public IActionResult searchUserDropDown([FromBody]SearchUserDropDownRequest request)
		{
			log.trace("searchUserDropDown");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			SearchUserDropDownResponse response = new SearchUserDropDownResponse();
			try
			{
				UserSQL sql = new UserSQL();
				sql.companyId = request.company;
				sql.idUser = request.user;
				sql.option = request.option;

				response.dropDown = sql.searchUserDropDown();
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
		/// Web Method para generar el código de recuperación
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("recoveryPassword")]
		[HttpPost]
		public IActionResult recoveryPassword([FromBody]RecoverPasswordRequest request)
		{
			log.trace("recoveryPassword");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			RecoverPasswordResponse response = new RecoverPasswordResponse();
			log.info("Recovery password for " + request.email);
			try
			{
				Random random = new Random();
				string codeRecovery = random.Next(0, 1000000).ToString("D6");
				log.trace("codeRecovry generated successfully");
				UserSQL sql = new UserSQL();
				sql.email = request.email;
				sql.recoveryCode = codeRecovery;
				sql.type = 1;

				sql.saveCodeRecovery();
				log.trace("recoveryCode saved to DB");

				EMAILClient email = new EMAILClient();
				string Mensaje = "Su código de recuperación es el siguiente: " + codeRecovery;
				List<string> destinatarios = new List<string>();
				destinatarios.Add(request.email);
				email.EnviarCorreo(destinatarios.ToArray(), Mensaje, "Recuperación de Contraseña");
				log.trace("Recovery email send");

			}
			catch (Exception ex)
			{
				log.error("There was an exception sending the recovery email: " + ex.Message);
				if (ex.Message == "Usuario inexistente")
				{
					response.messageEng = "Username does not exist";
					response.messageEsp = "El usuario no existe";
				}
				else
				{
					response.messageEng = "An error occurred: " + ex.Message;
					response.messageEsp = "Ocurrio un error: " + ex.Message;
				}
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}

		/// <summary>
		/// Web Method para realizar el cambio de contraseña
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("restorePassword")]
		[HttpPost]
		public IActionResult restorePassword([FromBody]RestorePasswordRequest request)
		{
			log.trace("restorePassword");
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(request));
			RestorePasswordResponse response = new RestorePasswordResponse();

			try
			{
				UserSQL sql = new UserSQL();
				sql.email = request.email;
				sql.recoveryCode = request.recoveryCode;
				sql.password = request.password;

				sql.saveCodeRecovery();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
if (ex.Message == "Usuario inexistente")
				{
					response.messageEng = "Username does not exist";
					response.messageEsp = "El usuario no existe";
				}
				else if (ex.Message == "Código de recuperación incorrecto")
				{
					response.messageEng = "Incorrect recovery code";
					response.messageEsp = "Código de recuperación incorrecto";
				}
				else
				{
					response.messageEng = "An error occurred: " + ex.Message;
					response.messageEsp = "Ocurrio un error: " + ex.Message;
				}
			}

			log.trace("Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(response));

			return Ok(response);
		}
	}
}