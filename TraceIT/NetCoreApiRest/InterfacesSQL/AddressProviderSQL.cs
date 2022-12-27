using Dapper;
using DWUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WSTraceIT.Models.ModelsSQL;
using NetCoreApiRest.Utils;

namespace WSTraceIT.InterfacesSQL
{
	public class AddressProviderSQL : DBHelperDapper
	{
		#region Properties
		//Datos dirección de proveedor
		public int addressId;
		public string addressName;
		public string phone;
		public string country;
		public string state;
		public string city;
		public string cp;
		public string address;
		public string latitude;
		public string longitude;
		public bool status;
		public string typeCompany;
		public int familyId;
		public int userId;
		public int tipo;
		public int isType; //Tipo TraceIt/Compania o Empacador
		public int empacadorId;
		public int paisId;
		public int estadoId;

		//Nombre SP a ejecutar
		public string sp;

		//Datos del filtro para direcciones de proveedor
		public List<AddressProviderDataSQL> addressLst;

		public List<ProvidersSelect> providers;

		// Variable para registrar los logs
		private LoggerD4 log = new LoggerD4("AddressProviderSQL");
		#endregion

		#region Constructor
		public AddressProviderSQL()
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
			this.userId = 0;
			this.tipo = 0;
			this.isType = 0;

			this.paisId = 0;
			this.estadoId = 0;

			this.empacadorId = 0;
			this.sp = String.Empty;

			this.addressLst = new List<AddressProviderDataSQL>();

			this.providers = new List<ProvidersSelect>();
		}
		#endregion

		#region Public methods

		#region BackOffice
		/// <summary>
		/// Metodo para consultar las direcciones de proveedor
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<AddressProviderDataSQL> SearchAddressProvider()
		{
			log.trace("SearchAddressProviderSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_typeCompany", typeCompany, DbType.String);
				parameters.Add("_familyId", familyId, DbType.Int32); //Familia
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_tipo", tipo, DbType.Int32);

				List<AddressProviderDataSQL> response = Consulta<AddressProviderDataSQL>("spc_consultaDireccionesProveedor", parameters);
				CerrarConexion();

				return response;
			}
			catch (Exception ex)
			{	
				log.error("Exception: " + ex.Message);	
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar los datos de una direccion de proveedor
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<AddressProviderDataSQL> SearchAddressProviderData()
		{
			log.trace("SearchAddressProviderDataSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_addressId", addressId, DbType.Int32);

				List<Type> types = new List<Type>();
				types.Add(typeof(AddressProviderDataSQL));
				types.Add(typeof(ProvidersSelect));

				var results = ConsultaMultiple("spc_consultaDireccionesProveedorData", types, parameters);
				//List<AddressProviderDataSQL> response = Consulta<AddressProviderDataSQL>("spc_consultaDireccionesProveedorData", parameters);

				List<AddressProviderDataSQL> response = results[0].Cast<AddressProviderDataSQL>().ToList();
				List<ProvidersSelect> sect = results[1].Cast<ProvidersSelect>().ToList();
				response[0].providers = sect;

				CerrarConexion();

				return response;
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para consultar las direcciones de proveedor a una lista de combo (select)
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <returns></returns>
		public List<AddressProviderComboSQL> SearchAddressProviderCombo()
		{
			log.trace("SearchAddressProviderComboSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				// Este es el que modificaremos
				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_fk_empacadorId", empacadorId, DbType.Int32);

				List<AddressProviderComboSQL> response = Consulta<AddressProviderComboSQL>("spc_consultaDireccionesProveedorCombo", parameters);
				CerrarConexion();

				return response;
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				CerrarConexion();
				throw ex;
			}
		}

		public List<ProvidersSelect> SearchProviders()	
		{
			log.trace("SearchProviders");
            try
            {
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_userId", userId, DbType.Int32);


				List<ProvidersSelect> response = Consulta<ProvidersSelect>("spc_consultaEmpacadoresDirec", parameters);

				return response;

			}
            catch (Exception ex)
            {
				log.error($"Exception: {ex.Message}");
				CerrarConexion();
				throw ex;
            }
        }

		/// <summary>
		/// Metodo para guardar / actualizar direcciones de proveedor
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		public void SaveAddressProvider()
		{
			log.trace("SaveAddressProviderSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				if (addressId > 0)
					parameters.Add("_addressId", addressId, DbType.Int32);
				parameters.Add("_addressName", addressName, DbType.String);
				parameters.Add("_phone", phone, DbType.String);
				parameters.Add("_paisId", paisId, DbType.Int32);
				parameters.Add("_estadoId", estadoId, DbType.Int32);
				parameters.Add("_city", city, DbType.String);
				parameters.Add("_cp", cp, DbType.String);
				parameters.Add("_address", address, DbType.String);
				parameters.Add("_latitude", latitude, DbType.String);
				parameters.Add("_longitude", longitude, DbType.String);
				parameters.Add("_typeCompany", typeCompany, DbType.String);
				parameters.Add("_status", status, DbType.Boolean);
				parameters.Add("_familyId", familyId, DbType.Int32);
				parameters.Add("_userId", userId, DbType.Int32);
				parameters.Add("_isType", isType, DbType.Int32);
				parameters.Add("_empacadorId", empacadorId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut(sp, parameters);

				int response = parameters.Get<int>("_response");

				if (response == 0)
					throw new Exception("Error al ejecutar el SP de " + (addressId > 0 ? "editar" : "agregar" ) + " dirección de proveedor");
				else if (response == -1)
					throw new Exception("Ya existe una dirección de proveedor con el mismo nombre");
				/*
                if (addressId > 0)
                {
					string query = $"DELETE FROM rel_048_empacadorDireccionProveedor WHERE DireccionId = {addressId};";
					ConsultaCommand<string>(query);

					response = addressId;
				}

                foreach (ProvidersSelect provider in providers)
                {
					string query = $"INSERT INTO rel_048_empacadorDireccionProveedor VALUES ({provider.id},{response});";
					ConsultaCommand<string>(query);
				}
				*/
				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}

		/// <summary>
		/// Metodo para eliminar direcciones de proveedor
		/// Desarollador: Iván Gutiérrez
		/// </summary>
		public void DeleteAddressProvider()
		{
			log.trace("DeleteAddressProviderSQL");
			try
			{
				CrearConexion(TipoConexion.Mysql);
				AbrirConexion();
				CrearTransaccion();

				DynamicParameters parameters = new DynamicParameters();
				parameters.Add("_addressId", addressId, DbType.Int32);
				parameters.Add("_response", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

				parameters = EjecutarSPOutPut("spd_eliminarDireccionesProveedor", parameters);

				if (parameters.Get<int>("_response") == 0)
					throw new Exception("Error al ejecutar el SP");
				else if (parameters.Get<int>("_response") == -1)
					throw new Exception("No se puede eliminar una dirección de proveedor con estatus activo");

				TransComit();
				CerrarConexion();
			}
			catch (Exception ex)
			{
				log.error("Exception: " + ex.Message);
				TransRollback();
				CerrarConexion();
				throw ex;
			}
		}
		#endregion

		#endregion
	}
}
