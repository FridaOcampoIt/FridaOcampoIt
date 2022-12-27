using System;

namespace WSTraceIT.Models.Base.Acopios
{
	/// <summary>
	/// Modelo base de acopios, para guardar información
	/// Desarrollador: Hernán Gómez
	/// </summary>
	public class AcopiosData
	{
		public int acopioId { get; set; }
		public Boolean activo { get; set; }
		public Boolean estatus { get; set; }
		public int numeroAcopio { get; set; }
		public string nombreAcopio { get; set; }
		public int paisId { get; set; }	
		public int estadoId { get; set; }
		public string ciudad { get; set; }
		public int codigoPostal { get; set; }
		public string direccion { get; set; }
		public string latitude {  get; set; }
		public string longitude { get; set; }
		public int usuarioCreador { get; set; }
		public int companiaId { get; set; }
		public DateTime fechaCreacion	{ get; set; }
		public DateTime fechaModificacion { get; set; }
		public int usuarioModificador	{ get; set; }

		public AcopiosData()
		{
			this.acopioId = 0;
			this.estatus = true;
			this.activo = true;
			this.numeroAcopio = 0;
			this.nombreAcopio = String.Empty;
			this.paisId = 0;
			this.estadoId = 0;
			this.ciudad = String.Empty;
			this.codigoPostal = 0;
			this.direccion = String.Empty;
			this.latitude = String.Empty;
			this.longitude = String.Empty;
			this.usuarioCreador = 0;
			this.companiaId = 0;
			this.fechaCreacion = DateTime.Now;
			this.fechaModificacion = DateTime.Now;
			this.usuarioModificador = 0;
		}
	}

}
