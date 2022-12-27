using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.Request
{
	public class SaveProductorRequest
	{ 
		public Boolean activo { get; set; }
		public int numeroProductor { get; set; }
		public string nombreProductor { get; set; } 
		public string nombreContacto { get; set; }
		public string apellidoContacto { get; set; }
		public string telefonoContacto { get; set; }
		public string nombreRancho { get; set; }
		public string direccion { get; set; }
		public string latitud { get; set; }
		public string longitud { get; set; }
		public int usuarioCreador { get; set; }
		public int companiaId { get; set; }
		public int usuarioModificador { get; set; }
		public List<int> acopiosId { get; set; }

		//Variable Auxiliares para guardar la ubicación
		public string address { get; set; }
		public string latitude { get; set; }
		public string longitude { get; set; }
	}

	public class UpdateProductorByIdRequest
    {
		public int productorId { get; set; }
		public Boolean activo { get; set; }
		public int numeroProductor { get; set; }
		public string nombreProductor { get; set; }
		public string nombreContacto { get; set; }
		public string apellidoContacto { get; set; }
		public string telefonoContacto { get; set; }
		public string nombreRancho { get; set; }
		public string direccion { get; set; }
		public string latitud { get; set; }
		public string longitud { get; set; }
		public int companiaId { get; set; }
		public int usuarioModificador { get; set; }
		public List<int> acopiosId { get; set; }
		public List<int> auxAcopiosId { get; set; }

		//Variable Auxiliares para guardar la ubicación
		public string address { get; set; }
		public string latitude { get; set; }
		public string longitude { get; set; }

	}
}
