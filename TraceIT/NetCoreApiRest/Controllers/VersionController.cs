using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiRest.Utils;

namespace NetCoreApiRest.Controllers
{
    [Route("Version")]
    [ApiController]
    public class VersionController : ControllerBase
    {
		private LoggerD4 log = new LoggerD4("VersionController");
		/// <summary>
		/// Web Method de prueba para validar un token que tiene acceso por medio del permiso de agregar usuario y un token valido
		/// NOTA: A cada Web Method se le debe poner el decorador [Authorize] para que pida el token
		///		  se le debe de mandar mediante el header Authorization con valor bearer [espacio] + token
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		[Route("versionPolicy")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.agregarCompania)]
		public IActionResult versionPolicy()
		{
			log.trace("versionPolicy");
			string version = ConfigurationSite._cofiguration["Version:versionWS"];
			return Ok("Version " + version);
		}

		/// <summary>
		/// Web Method de prueba para validar un token que tiene acceso por medio del permiso de agregar usuario el rol llamado admin y un token valido
		/// NOTA: A cada Web Method se le debe poner el decorador [Authorize] para que pida el token
		///		  se le debe de mandar mediante el header Authorization con valor bearer [espacio] + token
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		[Route("versionPolicyRol")]
		[HttpPost]
		[Authorize(Policy = ConstantsPermission.editarCompañia), Authorize(Roles = Role.Admin)]
		public IActionResult versionPolicyRol()
		{
			log.trace("versionPolicyRol");
			string version = ConfigurationSite._cofiguration["Version:versionWS"];
			return Ok("Version " + version);
		}

		/// <summary>
		/// Web Method de prueba para validar un token que tiene acceso por medio del permiso de agregar usuario el rol llamado admin y un token valido
		/// NOTA: A cada Web Method se le debe poner el decorador [Authorize] para que pida el token
		///		  se le debe de mandar mediante el header Authorization con valor bearer [espacio] + token
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		[Route("versionAutorize")]
		[HttpPost]
		[Authorize]
		public IActionResult versionAutorize()
		{
			log.trace("versionAutorize");
			string version = ConfigurationSite._cofiguration["Version:versionWS"];
			return Ok("Version " + version);
		}

		/// <summary>
		/// Web Method de prueba para validar que dicho metodo ignora el token de seguridad
		/// NOTA: para ignorar el token de seguridad se le debe quitar el decorador [Authorize]
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		[Route("version")]
		[HttpGet]
		public IActionResult version()
		{
			log.trace("version");
			string version = ConfigurationSite._cofiguration["Version:versionWS"];
			return Ok("Version " + version);
		}
	}
}