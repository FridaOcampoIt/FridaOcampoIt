using System;
namespace WSTraceIT.Models.Request
{
	#region Busquedas de datos para tabla movimientos por acopio
	//Busqueda de movimientos
	public class SearchMovimientoByCompanyIdRequest
	{
		public string producto { get; set; }
		public int tipoMovimientoId { get; set; }
		public string fechaCaducidad { get; set; }
		public string fechaIngresoDe { get; set; }
		public string datestart { get; set; }
		public string dateEnd { get; set; }
		public string fechaIngresoHasta { get; set; }
		public int companiaId { get; set; }
	}
	#endregion
	public class SaveAcopioRequest
	{
		public int acopioId { get; set; }
		public Boolean estatus { get; set; }
		public Boolean activo { get; set; }
		public int numeroAcopio { get; set; }
		public string nombreAcopio { get; set; }
		public int paisId { get; set; }
		public int estadoId { get; set; }
		public string ciudad { get; set; }
		public int codigoPostal { get; set; }
		public string direccion { get; set; }
		public string latitud { get; set; }
		public string longitud { get; set; }
		public int usuarioCreador { get; set; }
		public int companiaId { get; set; }
		public DateTime fechaCreacion { get; set; }
		public DateTime fechaModificacion { get; set; }
		public int usuarioModificador { get; set; }

		//Variable Auxiliares para guardar la ubicación

		public string address { get; set; }
		public string latitude { get; set; }
		public string longitude { get; set; }
	}
}
