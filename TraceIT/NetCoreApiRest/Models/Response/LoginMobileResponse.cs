using System;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.User;

namespace NetCoreApiRest.Models.Response
{
	/// <summary>
	/// Clase que se utilizara como respuesta de la petición del login para el usuario móvil
	/// Desarrollador: David Martinez
	/// </summary>
	public class LoginMobileResponse: TraceITResponse
	{
		public string token { get; set; }
		public DataUserMobile UserData { get; set; }

		public LoginMobileResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.token = String.Empty;
			this.UserData = new DataUserMobile();
		}
	}
}
