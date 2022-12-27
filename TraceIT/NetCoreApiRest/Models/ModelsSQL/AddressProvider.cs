using System;
using System.Collections.Generic;

namespace WSTraceIT.Models.ModelsSQL
{
	#region Backoffice
	/// <summary>
	/// Clase que contendra todos los datos necesarios de una derección de proveedor
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class AddressProviderDataSQL
	{
		public int addressId { get; set; }
		public string addressName { get; set; }
		public string phone { get; set; }
		public string country { get; set; }
		public string state { get; set; }
		public string city { get; set; }
		public string cp { get; set; }
		public string address { get; set; }
		public string latitude { get; set; }
		public string longitude { get; set; }
		public bool status { get; set; }
		public string typeCompany { get; set; }
		public int familyId { get; set; }
		public int isType { get; set; }
		public int empacadorId { get; set; }
		public int paisId { get; set; }
		public int estadoId { get; set; }
		public List<ProvidersSelect> providers { get; set; }

		#region Constructor
		public AddressProviderDataSQL()
		{
			this.addressId = 0;
			this.addressName = String.Empty;
			this.phone = String.Empty;
			this.country = String.Empty;
			this.state = String.Empty;
			this.city = String.Empty;
			this.cp = String.Empty;
			this.address = String.Empty;
			this.latitude = String.Empty;
			this.longitude = String.Empty;
			this.status = false;
			this.typeCompany = String.Empty;
			this.familyId = 0;
			this.isType = 0;
			this.providers = new List<ProvidersSelect>();
		}
        #endregion
	}

	/// <summary>
	/// Clase que contendra los datos necesarios para los combos de tipo direcciones de proveedor
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class AddressProviderComboSQL
	{
		public int id { get; set; }
		public string data { get; set; }

		#region Constructor
		public AddressProviderComboSQL()
		{
			this.id = 0;
			this.data = String.Empty;
		}
		#endregion
	}

	public class ProvidersSelect
	{
		public int id { get; set; }
		public string name { get; set; }
		public int type { get; set; }

        #region Constructor
		
		public ProvidersSelect()
		{
			this.id = 0;
			this.name = String.Empty;
			this.type = 0;
        }
        #endregion
    }
    #endregion
}
