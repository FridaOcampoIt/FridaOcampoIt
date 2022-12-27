using System;

namespace WSTraceIT.Models.Base.Address
{
	/// <summary>
	/// Modelo para mapear la busqueda de las direcciones
	/// Desarrollador: David Martinez
	/// </summary>
	public class AddressesData
	{
		public string serviceCenter { get; set; }
		public string company { get; set; }
		public int idAddress { get; set; }
		public string type { get; set; }

		public AddressesData()
		{
			this.company = String.Empty;
			this.serviceCenter = String.Empty;
			this.idAddress = 0;
			this.type = String.Empty;
		}
	}

	/// <summary>
	/// Modelo para mapear los datos para la edicion de una dirección
	/// Desarrollador: David Martinez
	/// </summary>
	public class AddressDataEdition
	{
		public int idAddress { get; set; }
		public string name { get; set; }
		public string phone { get; set; }
		public string address { get; set; }
		public string postalCode { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string country { get; set; }
		public string latitude { get; set; }
		public string longitude { get; set; }
		public bool status { get; set; }
		public int companyId { get; set; }
		public int directionType { get; set; }

		public AddressDataEdition()
		{
			this.idAddress = 0;
			this.address = String.Empty;
			this.city = String.Empty;
			this.companyId = 0;
			this.directionType = 0;
			this.country = String.Empty;
			this.latitude = String.Empty;
			this.longitude = String.Empty;
			this.name = String.Empty;
			this.phone = String.Empty;
			this.postalCode = String.Empty;
			this.state = String.Empty;
			this.status = false;
		}
	}
}
