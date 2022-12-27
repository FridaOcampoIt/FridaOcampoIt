using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Address;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;

namespace WSTraceIT.InterfacesSQL
{
	public class AddressSQL: DBHelperDapper
	{
		#region Properties
		public int idAddress;
		public string name;
		public string phone;
		public string address;
		public string postalCode;
		public string city;
		public string state;
		public string country;
		public string latitude;
		public string longitude;
		public bool status;
		public int idCompany;
		public int idTypeAddress;
		public int idFamily;
		private LoggerD4 log = new LoggerD4("AddressSQL");
		#endregion

		#region Constructor
		public AddressSQL()
		{
			this.idAddress = 0;
			this.name = String.Empty;
			this.phone = String.Empty;
			this.address = String.Empty;
			this.postalCode = String.Empty;
			this.city = String.Empty;
			this.state = String.Empty;
			this.country = String.Empty;
			this.latitude = String.Empty;
			this.longitude = String.Empty;
			this.status = false;
			this.idCompany = 0;
			this.idTypeAddress = 0;
			this.idFamily = 0;
		}
		#endregion

		#region Public method
		/// <summary>
		/// Metodo para guardar las direcciones
		/// Desarrollador: David Martinez
		/// </summary>
		public void SaveAddress()
		{
			log.trace("SaveAddress");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_phone", phone, DbType.String);
				parameters.Add("_address", address, DbType.String);
				parameters.Add("_postalCode", postalCode, DbType.String);
				parameters.Add("_city", city, DbType.String);
				parameters.Add("_state", state, DbType.String);
				parameters.Add("_country", country, DbType.String);
				parameters.Add("_latitude", latitude, DbType.String);
				parameters.Add("_longitude", longitude, DbType.String);
				parameters.Add("_status", status, DbType.Boolean);
				parameters.Add("_idCompany", idCompany, DbType.Int32);
				parameters.Add("_idTypeAddress", idTypeAddress, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spi_guardarDirecciones", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el sp");

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para realizar la actualizacion de los datos de las direcciones
		/// Desarrollador: David Martinez
		/// </summary>
		public void UpdateAddress()
		{
			log.trace("UpdateAddress");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_idAddress", idAddress, DbType.Int32);
				parameters.Add("_name", name, DbType.String);
				parameters.Add("_phone", phone, DbType.String);
				parameters.Add("_address", address, DbType.String);
				parameters.Add("_postalCode", postalCode, DbType.String);
				parameters.Add("_city", city, DbType.String);
				parameters.Add("_state", state, DbType.String);
				parameters.Add("_country", country, DbType.String);
				parameters.Add("_latitude", latitude, DbType.String);
				parameters.Add("_longitude", longitude, DbType.String);
				parameters.Add("_status", status, DbType.Boolean);
				parameters.Add("_idCompany", idCompany, DbType.Int32);
				parameters.Add("_idTypeAddress", idTypeAddress, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spu_edicionDirecciones", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el sp");

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
TransRollback();
				CerrarConexion();
				throw ex;
			}
		}
	
		/// <summary>
		/// Metodo para eliminar las direcciones
		/// Desarrollador: David Martinez
		/// </summary>
		public void DeleteAddress()
		{
			log.trace("DeleteAddress");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_idAddress", idAddress, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarDireccion", parameters);
				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el sp");
				else if (response == -1)
					throw new Exception("La direccion contiene familias y/o productos relacionados");

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar el listado de direcciones
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public List<AddressesData> SearchAddress()
		{
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_idFamily", idFamily, DbType.Int32);
				parameters.Add("_idCompany", idCompany, DbType.Int32);

				List<AddressesData> resp = Consulta<AddressesData>("spc_consultaDirecciones", parameters);
				CerrarConexion();

				return resp;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar los datos de las direcciones
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public AddressDataEdition SearchAddresData()
		{
			log.trace("SearchAddresData");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_idAddress", idAddress, DbType.Int32);

				AddressDataEdition resp = Consulta<AddressDataEdition>("spc_consultaDireccionesDatos", parameters).FirstOrDefault();
				CerrarConexion();

				return resp;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Web Method para realizar la busqueda de los combos que se utilizaran en el modulo
		/// Desarrollador: David Martinez
		/// </summary>
		/// <returns></returns>
		public AddressListDropDownSQL SearchDropDownList()
		{
			log.trace("SearchDropDownList");
			try
			{
				AddressListDropDownSQL resp = new AddressListDropDownSQL();
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				List<Type> types = new List<Type>();
				types.Add(typeof(TraceITListDropDown));
				types.Add(typeof(TraceITListDropDown));
				types.Add(typeof(TraceITListDropDown));

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_idCompany", idCompany, DbType.Int32);

				var respSQL = ConsultaMultiple("spc_consultaCombosCompania", types, parameters);

				resp.companyData = respSQL[0].Cast<TraceITListDropDown>().ToList();
				resp.addressTypeData = respSQL[1].Cast<TraceITListDropDown>().ToList();
				resp.familyData = respSQL[2].Cast<TraceITListDropDown>().ToList();

				CerrarConexion();
				return resp;
			}
			catch (Exception ex)
			{	log.error("Exception: " + ex.Message);	
CerrarConexion();
				throw ex;
			}
		}
		#endregion
	}
}
