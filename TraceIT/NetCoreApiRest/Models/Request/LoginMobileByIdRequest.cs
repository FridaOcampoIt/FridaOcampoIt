namespace WSTraceIT.Models.Request
{
	/// <summary>
	/// Clase para la autenticacion de un usuario móvil por medio de facebook o google
	/// Desarrollador: David Martinez
	/// </summary>
	public class LoginMobileByIdRequest
	{
		public int type { get; set; }
		public string idLogin { get; set; }
        public int idioma { get; set; }
	}
}
